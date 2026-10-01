using System.Diagnostics;
using ED.Assistant.Plugins.Explorer.Data;
using ED.Assistant.Plugins.Explorer.Journal;
using Microsoft.EntityFrameworkCore;

namespace ED.Assistant.Plugins.Explorer.Services;

public sealed record ExplorerSyncProgress(int FilesRead, int FilesTotal);

public sealed record ExplorerSyncResult(bool IsInitialImport, string? Folder, int FilesRead,
    int BodiesAdded, int BodiesChanged, string? Error)
{
    public bool IsSuccess => Error is null;
    public bool HasChanges => BodiesChanged > 0;

    public static ExplorerSyncResult Failed(string error, string? folder = null) =>
        new(false, folder, 0, 0, 0, error);
}

/// <summary>
/// Keeps the ExplorerBody table in step with the journals. The first run reads every journal
/// in the folder; later runs continue from the saved cursor (file + byte offset), triggered
/// whenever the host reads new journal lines.
/// </summary>
public sealed class ExplorerSyncService : IPluginBackgroundService, IDisposable
{
    // Write in batches during the initial import instead of once per journal file
    private const int FlushEveryChanges = 1000;
    private const int FlushEveryFiles = 100;

    private readonly ExplorerDbFactory _dbFactory;
    private readonly IPluginHost _host;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly TaskCompletionSource _ready = new(TaskCreationOptions.RunContinuationsAsynchronously);

    private CancellationToken _lifetime;
    private int _syncRequested;

    // In-memory state, reloaded from the DB after any failure
    private ExplorerJournalProcessor? _processor;
    private ExplorerJournalCursor? _cursor;

    /// <summary>Completes once the plugin's tables exist.</summary>
    public Task Ready => _ready.Task;

    /// <summary>Initial import progress (background thread).</summary>
    public event EventHandler<ExplorerSyncProgress>? Progress;

    /// <summary>Raised (background thread) after a run that changed data, failed, or was the initial import.</summary>
    public event EventHandler<ExplorerSyncResult>? Synced;

    public ExplorerSyncService(ExplorerDbFactory dbFactory, IPluginHost host)
    {
        _dbFactory = dbFactory;
        _host = host;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _lifetime = cancellationToken;

        try
        {
            await using var db = _dbFactory.Create();
            await db.Database.MigrateAsync(cancellationToken);
            _ready.TrySetResult();
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _ready.TrySetException(ex);
            Synced?.Invoke(this, ExplorerSyncResult.Failed($"Database setup failed: {ex.Message}"));
            return;
        }

        _host.JournalChanged += OnJournalChanged;
        await SyncAsync(cancellationToken);
    }

    public void Dispose()
    {
        _host.JournalChanged -= OnJournalChanged;
        _gate.Dispose();
    }

    private void OnJournalChanged(object? sender, EventArgs e) => RequestSync();

    /// <summary>Coalesces requests: at most one run waiting behind the one in progress.</summary>
    private void RequestSync()
    {
        if (Interlocked.Exchange(ref _syncRequested, 1) == 1)
            return;

        _ = SyncSafeAsync();
    }

    private async Task SyncSafeAsync()
    {
        try
        {
            await SyncAsync(_lifetime);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Explorer sync failed: {ex}");
        }
    }

    private async Task SyncAsync(CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            // Anything requested from here on needs another run
            Interlocked.Exchange(ref _syncRequested, 0);

            ExplorerSyncResult result;
            try
            {
                result = await CatchUpAsync(cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // The DB still matches its saved cursor; the in-memory copy may not
                _processor = null;
                _cursor = null;
                result = ExplorerSyncResult.Failed(ex.Message);
            }

            if (result.IsInitialImport || !result.IsSuccess || result.HasChanges)
                Synced?.Invoke(this, result);
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task<ExplorerSyncResult> CatchUpAsync(CancellationToken cancellationToken)
    {
        var folder = await _host.GetJournalFolderAsync(cancellationToken);
        if (!Directory.Exists(folder))
            return ExplorerSyncResult.Failed($"Journal folder not found: {folder}", folder);

        if (_processor is null)
        {
            await using var db = _dbFactory.Create();
            _cursor = await db.Cursors.AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == ExplorerJournalCursor.SingletonId, cancellationToken);
            var existing = await db.Bodies.AsNoTracking().ToListAsync(cancellationToken);
            _processor = new ExplorerJournalProcessor(existing);
        }

        var processor = _processor;
        var isInitial = _cursor is null;

        var files = JournalFiles.List(folder)
            .Where(f => _cursor is null ||
                        string.CompareOrdinal(Path.GetFileName(f), _cursor.FileName) >= 0)
            .ToList();

        if (isInitial)
            Progress?.Invoke(this, new ExplorerSyncProgress(0, files.Count));

        int filesRead = 0, added = 0, changed = 0, filesSinceFlush = 0;
        string? lastFile = null;
        long lastPosition = 0;

        for (var i = 0; i < files.Count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var name = Path.GetFileName(files[i]);
            var start = string.Equals(_cursor?.FileName, name, StringComparison.Ordinal) ? _cursor!.Position : 0;

            var (lines, position) = await JournalFiles.ReadCompleteLinesAsync(files[i], start, cancellationToken);
            if (position != start)
            {
                foreach (var line in lines)
                    processor.Apply(line);

                lastFile = name;
                lastPosition = position;
                filesRead++;
                filesSinceFlush++;
            }

            var isLast = i == files.Count - 1;
            if (lastFile is not null &&
                (isLast || processor.PendingChanges >= FlushEveryChanges || filesSinceFlush >= FlushEveryFiles))
            {
                var (newRows, updatedRows) = await FlushAsync(processor, lastFile, lastPosition, cancellationToken);
                added += newRows;
                changed += newRows + updatedRows;
                filesSinceFlush = 0;
                lastFile = null;
            }

            if (isInitial)
                Progress?.Invoke(this, new ExplorerSyncProgress(i + 1, files.Count));
        }

        return new ExplorerSyncResult(isInitial, folder, filesRead, added, changed, null);
    }

    /// <summary>Saves the changed rows and the cursor in one SaveChanges (one transaction).</summary>
    private async Task<(int Added, int Updated)> FlushAsync(ExplorerJournalProcessor processor,
        string fileName, long position, CancellationToken cancellationToken)
    {
        var (newRows, updatedRows) = processor.GetChanges();

        await using var db = _dbFactory.Create();
        db.Bodies.AddRange(newRows);
        db.Bodies.UpdateRange(updatedRows);

        var cursor = await db.Cursors.FindAsync(new object[] { ExplorerJournalCursor.SingletonId }, cancellationToken);
        if (cursor is null)
        {
            cursor = new ExplorerJournalCursor { Id = ExplorerJournalCursor.SingletonId };
            db.Cursors.Add(cursor);
        }

        cursor.FileName = fileName;
        cursor.Position = position;
        cursor.DateUpdated = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);

        processor.AcceptChanges();
        _cursor = new ExplorerJournalCursor
        {
            Id = cursor.Id,
            FileName = fileName,
            Position = position,
            DateUpdated = cursor.DateUpdated
        };

        return (newRows.Count, updatedRows.Count);
    }
}
