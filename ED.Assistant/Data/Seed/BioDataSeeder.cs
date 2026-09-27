using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ED.Assistant.Data.Seed;

sealed class BioDataSeeder : IBioDataSeeder
{
	private const string CatalogId = "EDMC-BioScan-without-regions-v1";
	private readonly AppDbContext _db;

	public BioDataSeeder(AppDbContext db) => _db = db;

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		using var stream = typeof(BioDataSeeder).Assembly.GetManifestResourceStream(
			"ED.Assistant.Data.Seed.Catalog.bioscan.json")
			?? throw new InvalidOperationException("Embedded biology catalog is missing.");
		using var reader = new StreamReader(stream);
		var json = await reader.ReadToEndAsync(cancellationToken);
		var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(json)));
		var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
		options.Converters.Add(new JsonStringEnumConverter());
		var catalog = JsonSerializer.Deserialize<BioCatalog>(json, options)
			?? throw new InvalidOperationException("Invalid biology catalog.");
		if (catalog.SchemaVersion != 1 || catalog.Species.Count == 0)
			throw new InvalidOperationException("Unsupported or empty biology catalog.");

		await _db.Database.MigrateAsync(cancellationToken);
		await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
		var version = await _db.BioCatalogVersions.FindAsync([CatalogId], cancellationToken);
		if (version?.ContentHash == hash)
			return;

		// Catalog IDs are references within the JSON. Resolve to existing database rows,
		// preserving installed IDs, including databases with different insertion orders.
		var bodyTypes = await LoadBodyTypesAsync(catalog.BodyTypes, cancellationToken);
		var atmospheres = await LoadAtmospheresAsync(catalog.Atmospheres, cancellationToken);
		var genera = await _db.BioGenera.ToListAsync(cancellationToken);
		var determinants = await _db.VariantDeterminants.ToListAsync(cancellationToken);
		var species = await _db.BioSpecies.Include(x => x.SpawnRules).ToListAsync(cancellationToken);

		foreach (var item in catalog.Species)
		{
			var genus = genera.FirstOrDefault(x => x.JournalName == item.GenusJournalName)
				?? genera.FirstOrDefault(x => x.Name.Equals(item.GenusName, StringComparison.OrdinalIgnoreCase));
			if (genus is null)
			{
				genus = new() { Name = item.GenusName, DisplayName = item.GenusName };
				genera.Add(genus);
				_db.BioGenera.Add(genus);
			}
			genus.JournalName = item.GenusJournalName;

			var entity = species.FirstOrDefault(x => x.JournalName == item.JournalName)
				?? species.FirstOrDefault(x => x.Genus == genus &&
					x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
			if (entity is null)
			{
				var determinant = determinants.FirstOrDefault(x => x.Name == item.VariantDeterminant);
				if (determinant is null)
				{
					determinant = new() { Name = item.VariantDeterminant };
					determinants.Add(determinant);
					_db.VariantDeterminants.Add(determinant);
				}
				entity = new()
				{
					Name = item.Name, DisplayName = item.Name, Genus = genus,
					MinScanDistanceM = item.MinScanDistanceM, VariantDeterminant = determinant
				};
				species.Add(entity);
				_db.BioSpecies.Add(entity);
			}

			entity.JournalName = item.JournalName;
			entity.BaseValue = item.BaseValue;
			// Replace the reference rules for this species only. Other species and all
			// existing sampling distances, display names and determinant IDs survive.
			_db.BioSpawnRules.RemoveRange(entity.SpawnRules);
			entity.SpawnRules.Clear();
			foreach (var rule in item.Rules)
				entity.SpawnRules.Add(CreateRule(rule, item.SourceFile, bodyTypes, atmospheres));
		}

		if (version is null)
		{
			version = new() { Id = CatalogId };
			_db.BioCatalogVersions.Add(version);
		}
		version.ContentHash = hash;
		version.SourceCommit = catalog.SourceCommit;
		await _db.SaveChangesAsync(cancellationToken);
		await transaction.CommitAsync(cancellationToken);
	}

	private async Task<Dictionary<int, BodyType>> LoadBodyTypesAsync(
		IEnumerable<CatalogLookup> items, CancellationToken cancellationToken)
	{
		var existing = await _db.BodyTypes.ToListAsync(cancellationToken);
		var result = new Dictionary<int, BodyType>();
		foreach (var item in items)
		{
			var entity = existing.FirstOrDefault(x => NormalizeBodyType(x.Name) == NormalizeBodyType(item.Name));
			if (entity is null)
			{
				entity = new() { Name = item.Name };
				existing.Add(entity);
				_db.BodyTypes.Add(entity);
			}
			result.Add(item.Id, entity);
		}
		return result;
	}

	private async Task<Dictionary<int, Atmosphere>> LoadAtmospheresAsync(
		IEnumerable<CatalogLookup> items, CancellationToken cancellationToken)
	{
		var existing = await _db.Atmospheres.ToListAsync(cancellationToken);
		var result = new Dictionary<int, Atmosphere>();
		foreach (var item in items)
		{
			var entity = existing.FirstOrDefault(x => Normalize(x.Name) == Normalize(item.Name));
			if (entity is null)
			{
				entity = new() { Name = item.Name };
				existing.Add(entity);
				_db.Atmospheres.Add(entity);
			}
			result.Add(item.Id, entity);
		}
		return result;
	}

	private static string Normalize(string value) =>
		new(value.Where(char.IsLetterOrDigit).Select(char.ToLowerInvariant).ToArray());

	private static string NormalizeBodyType(string value) => Normalize(value) switch
	{
		"rocky" => "rockybody", "icy" => "icybody", "rockyice" => "rockyicebody",
		var normalized => normalized
	};

	private static BioSpawnRule CreateRule(CatalogRule item, string sourceFile,
		IReadOnlyDictionary<int, BodyType> bodyTypes, IReadOnlyDictionary<int, Atmosphere> atmospheres) => new()
	{
		SourceFile = sourceFile, SourceIndex = item.SourceIndex,
		MinTemperatureK = item.MinTemperatureK, MaxTemperatureK = item.MaxTemperatureK,
		MinGravityG = item.MinGravityG, MaxGravityG = item.MaxGravityG,
		MinPressureAtmospheres = item.MinPressureAtmospheres, MaxPressureAtmospheres = item.MaxPressureAtmospheres,
		MaxOrbitalPeriodSeconds = item.MaxOrbitalPeriodSeconds, MinArrivalDistanceLs = item.MinArrivalDistanceLs,
		Nebula = item.Nebula, VolcanismMode = item.VolcanismMode,
		BodyTypes = item.BodyTypeIds.Select(id => new BioSpawnRuleBodyType
			{ BodyType = bodyTypes[id], Mode = ConditionMode.Required }).ToList(),
		SystemBodyTypes = item.SystemBodyTypeIds.Select(id => new BioSpawnRuleSystemBodyType
			{ BodyType = bodyTypes[id] }).ToList(),
		Atmospheres = item.AtmosphereIds.Select(id => new BioSpawnRuleAtmosphere
			{ Atmosphere = atmospheres[id], Mode = ConditionMode.Required }).ToList(),
		AtmosphereComponents = item.AtmosphereComponents.Select(x => new BioSpawnRuleAtmosphereComponent
			{ Atmosphere = atmospheres[x.AtmosphereId], MinPercent = x.MinPercent }).ToList(),
		VolcanismPatterns = item.VolcanismPatterns.Select(x => new BioSpawnRuleVolcanism
			{ Pattern = x.Pattern, Match = x.Match }).ToList(),
		Stars = item.Stars.Select(x => new BioSpawnRuleStar
			{ StarType = x.StarType, Luminosity = x.Luminosity, Scope = x.Scope }).ToList()
	};
}
