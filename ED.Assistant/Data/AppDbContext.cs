using ED.Assistant.Data.Seed;
using ED.Assistant.Data.Seed.ExoBiology;

namespace ED.Assistant.Data;

public sealed class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options) { }

	public DbSet<BodyClass> BodyClasses => Set<BodyClass>();
	public DbSet<Genus> Genuses => Set<Genus>();
	public DbSet<Rule> Rules => Set<Rule>();
	public DbSet<Volcanism> Volcanisms => Set<Volcanism>();
	public DbSet<Atmosphere> Atmospheres => Set<Atmosphere>();
	public DbSet<AtmosphereComponentRule> AtmosphereComponentRules => Set<AtmosphereComponentRule>();
	public DbSet<RuleStar> RuleStars => Set<RuleStar>();
	public DbSet<StarClass> StarClasses => Set<StarClass>();
	public DbSet<ParentBodyClass> ParentBodyClasses => Set<ParentBodyClass>();
	public DbSet<Evaluator.Evaluator> Evaluators => Set<Evaluator.Evaluator>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

		ExoBilogyDataSeed.Seed(modelBuilder);
	}
}
