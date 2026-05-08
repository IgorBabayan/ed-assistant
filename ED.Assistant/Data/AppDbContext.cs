namespace ED.Assistant.Data;

public sealed class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<BioGenus> BioGenera => Set<BioGenus>();
	public DbSet<BioSpecies> BioSpecies => Set<BioSpecies>();
	public DbSet<BioSpawnRule> BioSpawnRules => Set<BioSpawnRule>();
	public DbSet<Atmosphere> Atmospheres => Set<Atmosphere>();
	public DbSet<BodyType> BodyTypes => Set<BodyType>();
	public DbSet<SpeciesAtmosphereCondition> SpeciesAtmosphereConditions => Set<SpeciesAtmosphereCondition>();
	public DbSet<VariantDeterminant> VariantDeterminants => Set<VariantDeterminant>();
	public DbSet<BioSpawnRuleBodyType> BioSpawnRuleBodyTypes => Set<BioSpawnRuleBodyType>();

	protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
