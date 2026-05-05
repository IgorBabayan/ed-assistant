namespace ED.Assistant.Data;

public sealed class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<BioGenus> BioGenuses => Set<BioGenus>();
	public DbSet<BioSpecies> BioSpecies => Set<BioSpecies>();
	public DbSet<BioVariant> BioVariants => Set<BioVariant>();
	public DbSet<BioSpawnCondition> BioSpawnConditions => Set<BioSpawnCondition>();
	public DbSet<BioSource> BioSources => Set<BioSource>();
	public DbSet<BioReference> BioReferences => Set<BioReference>();
	public DbSet<BioVariantRule> BioVariantRules => Set<BioVariantRule>();

	protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
