namespace ED.Assistant.Data.Seed;

class BioDataSeeder : IBioDataSeeder
{
	private readonly AppDbContext _db;
	private readonly BioDataSeed _seed;

	public BioDataSeeder(AppDbContext db, BioDataSeed seed)
	{
		_db = db;
		_seed = seed;
	}

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		await _db.Database.MigrateAsync(cancellationToken);

		if (await _db.BioGenuses.AnyAsync(cancellationToken))
			return;

		var basePath = IOPath.Combine(AppContext.BaseDirectory, "Data", "Seed", "DataBase");

		var genera = await _seed.LoadGeneraAsync(basePath);
		var species = await _seed.LoadSpeciesAsync(basePath);
		var variants = await _seed.LoadVariantsAsync(basePath);
		var rules = await _seed.LoadVariantRulesAsync(basePath);
		var sources = await _seed.LoadSourcesAsync(basePath);

		await _db.BioGenuses.AddRangeAsync(genera, cancellationToken);
		await _db.BioSpecies.AddRangeAsync(species, cancellationToken);
		await _db.BioVariants.AddRangeAsync(variants, cancellationToken);
		await _db.BioVariantRules.AddRangeAsync(rules, cancellationToken);
		await _db.BioSources.AddRangeAsync(sources, cancellationToken);

		await _db.SaveChangesAsync(cancellationToken);
	}
}
