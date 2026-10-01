using Microsoft.EntityFrameworkCore;

namespace ED.Assistant.Plugins.Explorer.Data;

/// <summary>
/// Lives in the host's database but only maps the plugin's own tables, with its own
/// migrations history table, so it never touches the host's schema or migrations.
/// </summary>
public sealed class ExplorerDbContext(DbContextOptions<ExplorerDbContext> options) : DbContext(options)
{
    public const string MigrationsHistoryTable = "__ExplorerMigrationsHistory";

    public DbSet<ExplorerBody> Bodies => Set<ExplorerBody>();
    public DbSet<ExplorerJournalCursor> Cursors => Set<ExplorerJournalCursor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExplorerBody>(b =>
        {
            b.ToTable("ExplorerBody");
            b.HasKey(x => x.Id);
            b.Property(x => x.StarSystem).IsRequired();
            b.Property(x => x.BodyName).IsRequired();
            b.Property(x => x.PlanetClass).IsRequired();
            b.HasIndex(x => new { x.SystemAddress, x.BodyId });
            b.HasIndex(x => new { x.IsActive, x.DateScanned });
        });

        modelBuilder.Entity<ExplorerJournalCursor>(b =>
        {
            b.ToTable("ExplorerJournalCursor");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.FileName).IsRequired();
        });
    }
}
