using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ED.Assistant.Plugins.Explorer.Data;

public sealed class ExplorerDbFactory(string databasePath)
{
    private readonly DbContextOptions<ExplorerDbContext> _options = BuildOptions(databasePath);

    public ExplorerDbContext Create() => new(_options);

    internal static DbContextOptions<ExplorerDbContext> BuildOptions(string databasePath) =>
        new DbContextOptionsBuilder<ExplorerDbContext>()
            .UseSqlite(new SqliteConnectionStringBuilder { DataSource = databasePath }.ToString(),
                sqlite => sqlite.MigrationsHistoryTable(ExplorerDbContext.MigrationsHistoryTable))
            // The snapshot is hand-maintained; don't let a cosmetic difference block startup
            .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
            .Options;
}

/// <summary>Used by `dotnet ef migrations add` only.</summary>
public sealed class DesignTimeExplorerDbFactory : IDesignTimeDbContextFactory<ExplorerDbContext>
{
    public ExplorerDbContext CreateDbContext(string[] args) =>
        new(ExplorerDbFactory.BuildOptions("explorer-design-time.db"));
}
