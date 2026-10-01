using System.Diagnostics;
using System.IO;
using System.Text.Json;
using ED.Assistant.Data;
using ED.Assistant.Data.Storage;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Plugins;

public enum PluginRemovalResult
{
    Removed,
    PendingRestart
}

public interface IPluginUninstaller
{
    /// <summary>Drops the addon's tables, then deletes its data and folder (or schedules it for the next start).</summary>
    Task<PluginRemovalResult> RemoveAsync(InstalledPlugin plugin, CancellationToken cancellationToken = default);

    /// <summary>Finishes removals scheduled in a previous session. Call after the host migration.</summary>
    void CompletePendingRemovals();
}

internal sealed class PluginUninstaller(
    PluginCatalog catalog,
    IDbPathProvider dbPathProvider,
    IServiceScopeFactory scopeFactory) : IPluginUninstaller
{
    public const string ManifestFile = "addon.json";
    private const string HostMigrationsHistoryTable = "__EFMigrationsHistory";

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private sealed record AddonManifest(string? Id, List<string>? Tables);

    public async Task<PluginRemovalResult> RemoveAsync(InstalledPlugin plugin, CancellationToken cancellationToken = default)
    {
        if (plugin.IsRemovalPending)
            return PluginRemovalResult.PendingRestart;

        // Its assembly is in the process: the DLL is locked (Windows) and its background
        // service may still be using its tables. Finish on the next start, before it loads.
        if (plugin.Status != PluginLoadStatus.Disabled)
        {
            SchedulePending(plugin);
            return PluginRemovalResult.PendingRestart;
        }

        var hostTables = GetHostTables();
        var removed = await Task.Run(() => Remove(plugin.Descriptor.Directory, hostTables), cancellationToken);

        if (!removed)
        {
            SchedulePending(plugin);
            return PluginRemovalResult.PendingRestart;
        }

        catalog.Forget(plugin);
        return PluginRemovalResult.Removed;
    }

    public void CompletePendingRemovals()
    {
        if (!Directory.Exists(catalog.Root))
            return;

        var pending = Directory.GetDirectories(catalog.Root)
            .Where(d => File.Exists(IOPath.Combine(d, PluginLoader.REMOVAL_MARKER)))
            .ToList();

        if (pending.Count == 0)
            return;

        var hostTables = GetHostTables();
        foreach (var directory in pending)
        {
            try
            {
                Remove(directory, hostTables);
            }
            catch (Exception ex)
            {
                // Marker stays, so it is retried on the next start
                Trace.WriteLine($"Pending removal of '{directory}' failed: {ex}");
            }
        }
    }

    /// <summary>
    /// Tables first, then files: the manifest naming the tables lives in the folder being deleted.
    /// Throws if the tables can't be dropped (nothing is deleted then).
    /// Returns false if the folder is locked.
    /// </summary>
    private bool Remove(string directory, IReadOnlySet<string> hostTables)
    {
        var manifest = ReadManifest(directory);
        if (manifest is not null)
        {
            DropTables(manifest.Tables ?? [], hostTables);
            DeleteDataDirectory(manifest.Id);
        }

        try
        {
            Directory.Delete(directory, recursive: true);
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Trace.WriteLine($"Can't delete addon folder '{directory}' now: {ex.Message}");
            return false;
        }
    }

    private void DropTables(IReadOnlyList<string> tables, IReadOnlySet<string> hostTables)
    {
        var databasePath = dbPathProvider.GetDatabasePath();
        if (tables.Count == 0 || !File.Exists(databasePath))
            return;

        using var connection = new SqliteConnection(new SqliteConnectionStringBuilder
        {
            DataSource = databasePath,
            Mode = SqliteOpenMode.ReadWrite, // never create an empty database here
            Pooling = false                  // don't keep the file open afterwards
        }.ToString());
        connection.Open();

        // Plugin tables may reference each other; drop order must not matter.
        // Has to run outside the transaction to take effect.
        using (var pragma = connection.CreateCommand())
        {
            pragma.CommandText = "PRAGMA foreign_keys = OFF;";
            pragma.ExecuteNonQuery();
        }

        using var transaction = connection.BeginTransaction();
        foreach (var table in tables)
        {
            if (!CanDrop(table, hostTables))
            {
                Trace.WriteLine($"Addon manifest lists table '{table}', which an addon may not drop. Skipped.");
                continue;
            }

            using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = $"DROP TABLE IF EXISTS \"{table.Replace("\"", "\"\"")}\";";
            command.ExecuteNonQuery();
        }

        transaction.Commit();
    }

    // A broken or malicious manifest must never drop the host's own tables
    private static bool CanDrop(string table, IReadOnlySet<string> hostTables) =>
        !string.IsNullOrWhiteSpace(table)
        && !table.StartsWith("sqlite_", StringComparison.OrdinalIgnoreCase)
        && !hostTables.Contains(table);

    private void DeleteDataDirectory(string? id)
    {
        if (string.IsNullOrWhiteSpace(id) || id is "." or ".."
            || id.IndexOfAny(IOPath.GetInvalidFileNameChars()) >= 0)
            return;

        var dataDir = IOPath.Combine(catalog.Root, "_data", id);
        if (Directory.Exists(dataDir))
            Directory.Delete(dataDir, recursive: true);
    }

    private static AddonManifest? ReadManifest(string directory)
    {
        var path = IOPath.Combine(directory, ManifestFile);
        if (!File.Exists(path))
        {
            Trace.WriteLine($"No {ManifestFile} in '{directory}': its tables are left in the database.");
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<AddonManifest>(File.ReadAllText(path), JsonOptions);
        }
        catch (JsonException ex)
        {
            Trace.WriteLine($"Invalid {ManifestFile} in '{directory}': {ex.Message}");
            return null;
        }
    }

    private IReadOnlySet<string> GetHostTables()
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var tables = db.Model.GetEntityTypes()
            .Select(e => e.GetTableName())
            .OfType<string>()
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        tables.Add(HostMigrationsHistoryTable);
        return tables;
    }

    private static void SchedulePending(InstalledPlugin plugin)
    {
        File.WriteAllText(IOPath.Combine(plugin.Descriptor.Directory, PluginLoader.REMOVAL_MARKER), string.Empty);
        plugin.IsRemovalPending = true;
    }
}