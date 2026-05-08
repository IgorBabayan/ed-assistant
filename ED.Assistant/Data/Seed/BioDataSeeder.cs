namespace ED.Assistant.Data.Seed;

sealed class BioDataSeeder : IBioDataSeeder
{
	private readonly AppDbContext _db;

	public BioDataSeeder(AppDbContext db) => _db = db;

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		await _db.Database.MigrateAsync(cancellationToken);

		if (await _db.BioSpecies.AnyAsync(cancellationToken))
			return;

		var atmosphereCache = new Dictionary<string, Atmosphere>(StringComparer.OrdinalIgnoreCase);
		var bodyTypeCache = new Dictionary<string, BodyType>(StringComparer.OrdinalIgnoreCase);
		var determinantCache = new Dictionary<string, VariantDeterminant>(StringComparer.OrdinalIgnoreCase);
		var genera = SeedData
			.GroupBy(x => x.Genus)
			.Select(group =>
			{
				var genus = new BioGenus
				{
					Name = group.Key,
					DisplayName = group.Key
				};

				foreach (var item in group)
				{
					var species = CreateSpecies(item, atmosphereCache, bodyTypeCache, determinantCache);
					genus.Species.Add(species);
				}

				return genus;
			}).ToList();

		await _db.BioGenera.AddRangeAsync(genera, cancellationToken);
		await _db.SaveChangesAsync(cancellationToken);
	}

	private static BioSpecies CreateSpecies(BioSeedItem item, Dictionary<string, Atmosphere> atmosphereCache,
		Dictionary<string, BodyType> bodyTypeCache, Dictionary<string, VariantDeterminant> determinantCache)
	{
		var species = new BioSpecies
		{
			Name = item.Name,
			DisplayName = item.Name,
			BaseValue = item.BaseValue,
			MinScanDistanceM = item.ColonyDistanceM,
			SpawnRule = new BioSpawnRule
			{
				AtmosphereRaw = item.AtmosphereRaw,
				VolcanismRaw = item.VolcanismRaw
			}
		};

		foreach (var atmosphereName in Split(item.AtmosphereRaw))
		{
			if (!atmosphereCache.TryGetValue(atmosphereName, out var atmosphere))
			{
				atmosphere = new()
				{
					Name = atmosphereName
				};

				atmosphereCache[atmosphereName] = atmosphere;
			}

			species.AtmosphereConditions.Add(new()
			{
				Species = species,
				Atmosphere = atmosphere,
				Mode = GetConditionMode(item.AtmosphereRaw)
			});
		}

		foreach (var bodyTypeName in Split(item.BodyTypesRaw))
		{
			if (!bodyTypeCache.TryGetValue(bodyTypeName, out var bodyType))
			{
				bodyType = new()
				{
					Name = bodyTypeName
				};

				bodyTypeCache[bodyTypeName] = bodyType;
			}

			species.SpawnRule!.BodyTypes.Add(new()
			{
				SpawnRule = species.SpawnRule,
				BodyType = bodyType,
				Mode = ConditionMode.Required
			});
		}

		if (!determinantCache.TryGetValue(item.VariantDeterminantName, out var determinant))
		{
			determinant = new()
			{
				Name = item.VariantDeterminantName
			};

			determinantCache[item.VariantDeterminantName] = determinant;
		}

		species.VariantDeterminant = determinant;
		return species;
	}

	private static ConditionMode GetConditionMode(string value)
	{
		if (value.StartsWith("Not:", StringComparison.OrdinalIgnoreCase))
			return ConditionMode.Excluded;

		if (value.StartsWith("Any", StringComparison.OrdinalIgnoreCase))
			return ConditionMode.Any;

		return ConditionMode.Required;
	}

	private static IEnumerable<string> Split(string value)
	{
		value = value
			.Replace("Not:", "", StringComparison.OrdinalIgnoreCase)
			.Replace("Any except", "", StringComparison.OrdinalIgnoreCase)
			.Replace("(bugged!)", "", StringComparison.OrdinalIgnoreCase)
			.Trim();

		if (value.Equals("Any", StringComparison.OrdinalIgnoreCase))
			yield break;

		foreach (var item in value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
		{
			if (string.IsNullOrWhiteSpace(item) || item == "-")
				continue;

			yield return item;
		}
	}

	private sealed record BioSeedItem(string Genus, string Name, int BaseValue, string AtmosphereRaw,
		string BodyTypesRaw, string VariantDeterminantName, string VolcanismRaw, int ColonyDistanceM);

	private static readonly BioSeedItem[] SeedData =
	[
		new("Aleoida", "Aleoida Arcus", 7252500, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 150),
		new("Aleoida", "Aleoida Coronamus", 6284600, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 150),
		new("Aleoida", "Aleoida Gravis", 12934900, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 150),
		new("Aleoida", "Aleoida Laminiae", 3385200, "Ammonia", "HIGH METAL CONTENT BODY, Rocky", "Stars", "-", 150),
		new("Aleoida", "Aleoida Spica", 3385200, "Ammonia", "HIGH METAL CONTENT BODY, Rocky", "Stars", "-", 150),

		new("Bacterium", "Bacterium Acies", 1000000, "Neon", "Icy body, Rocky Ice body", "Materials", "-", 500),
		new("Bacterium", "Bacterium Alcyoneum", 1658500, "Ammonia", "HIGH METAL CONTENT BODY, Rocky body, Rocky Ice body", "Stars", "-", 500),
		new("Bacterium", "Bacterium Aurasus", 1000000, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "-", 500),
		new("Bacterium", "Bacterium Bullaris", 1152500, "Methane, Methane-Rich", "HIGH METAL CONTENT BODY, Icy body, Rocky", "Materials", "-", 500),
		new("Bacterium", "Bacterium Cerbrus", 1689800, "Sulphur Dioxide, Water, Water-Rich", "HIGH METAL CONTENT BODY, Rocky body, Rocky Ice body", "Stars", "None, Minor Water Magma", 500),
		new("Bacterium", "Bacterium Informem", 8418000, "Nitrogen", "Any", "Materials", "-", 500),
		new("Bacterium", "Bacterium Nebulus", 5289900, "Helium", "Icy body", "Materials", "-", 500),
		new("Bacterium", "Bacterium Omentum", 4638900, "Not: Carbon Dioxide, Oxygen, Sulphur Dioxide", "Icy body", "Materials", "-", 500),
		new("Bacterium", "Bacterium Scopulum", 4934500, "Not: Carbon Dioxide, Oxygen, Sulphur Dioxide", "Icy body", "Materials", "Carbon Dioxide, Methane", 500),
		new("Bacterium", "Bacterium Tela", 1949000, "Not: Methane-rich, Sulphur Dioxide-rich", "Any", "Materials", "- / Yes / Water", 500),
		new("Bacterium", "Bacterium Verrata", 3897000, "Not: Sulphur Dioxide", "Icy body, Rocky body, Rocky Ice body", "Materials", "Water", 500),
		new("Bacterium", "Bacterium Vesicula", 1000000, "Argon", "Icy body, Rocky body, Rocky Ice body", "Materials", "-", 500),
		new("Bacterium", "Bacterium Volu", 7774700, "Oxygen", "HIGH METAL CONTENT BODY, Icy body, Rocky Ice body", "Materials", "-", 500),

		new("Cactoida", "Cactoida Cortexum", 3667600, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 300),
		new("Cactoida", "Cactoida Lapis", 2483600, "Ammonia", "HIGH METAL CONTENT BODY, Rocky", "Stars", "-", 300),
		new("Cactoida", "Cactoida Peperatis", 2483600, "Ammonia", "HIGH METAL CONTENT BODY, Rocky", "Stars", "-", 300),
		new("Cactoida", "Cactoida Pullulanta", 3667600, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 300),
		new("Cactoida", "Cactoida Vermis", 16202800, "Water", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None, Minor Water Magma", 300),

		new("Clypeus", "Clypeus Lacrimam", 8418000, "Carbon Dioxide, Water", "Rocky", "Stars", "None", 150),
		new("Clypeus", "Clypeus Margaritus", 11873200, "Carbon Dioxide, Water", "HIGH METAL CONTENT BODY", "Stars", "None", 150),
		new("Clypeus", "Clypeus Speculumi", 16202800, "Carbon Dioxide, Water", "Rocky", "Stars", "None", 150),

		new("Concha", "Concha Aureolas", 7774700, "Ammonia (bugged!)", "HIGH METAL CONTENT BODY, Rocky", "Stars", "-", 150),
		new("Concha", "Concha Biconcavis", 19010800, "Nitrogen", "HIGH METAL CONTENT BODY, Rocky", "Materials", "None", 150),
		new("Concha", "Concha Labiata", 2352400, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 150),
		new("Concha", "Concha Renibus", 4572400, "Ammonia, Carbon Dioxide, Methane, Water", "HIGH METAL CONTENT BODY, Rocky", "Materials", "None", 150),

		new("Electricae", "Electricae Pluma", 6284600, "Argon, Argon-rich, Neon, Neon-rich", "Icy body", "Materials", "-", 1000),
		new("Electricae", "Electricae Radialem", 6284600, "Argon, Argon-rich, Neon, Neon-rich", "Icy body", "Materials", "-", 1000),

		new("Fonticulua", "Fonticulua Campestris", 1000000, "Argon", "Icy body, Rocky Ice body", "Stars", "-", 500),
		new("Fonticulua", "Fonticulua Digitos", 1804100, "Methane", "Icy body, Rocky Ice body", "Stars", "None, Minor Methane Magma", 500),
		new("Fonticulua", "Fonticulua Fluctus", 20000000, "Oxygen", "Icy body", "Stars", "-", 500),
		new("Fonticulua", "Fonticulua Lapida", 3111000, "Nitrogen", "Icy body", "Stars", "-", 500),
		new("Fonticulua", "Fonticulua Segmentatus", 19010800, "Neon, Neon-rich", "Icy body", "Stars", "-", 500),
		new("Fonticulua", "Fonticulua Upupam", 5727600, "Argon-rich", "Icy body", "Stars", "-", 500),

		new("Frutexa", "Frutexa Acus", 7774700, "Carbon Dioxide", "Rocky", "Stars", "None", 150),
		new("Frutexa", "Frutexa Collum", 1639800, "Sulphur Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "-", 150),
		new("Frutexa", "Frutexa Fera", 1632500, "Carbon Dioxide", "Rocky", "Stars", "None", 150),
		new("Frutexa", "Frutexa Flabellum", 1808900, "Ammonia", "Rocky", "Stars", "-", 150),
		new("Frutexa", "Frutexa Flammasis", 10326000, "Ammonia", "Rocky", "Stars", "-", 150),
		new("Frutexa", "Frutexa Metallicum", 1632500, "Ammonia, Carbon Dioxide, Water", "HIGH METAL CONTENT BODY", "Stars", "None", 150),
		new("Frutexa", "Frutexa Sponsae", 5988000, "Water", "Rocky", "Stars", "None, Minor Water Magma", 150),

		new("Fumerola", "Fumerola Aquatis", 6284600, "Any except Helium", "Icy body, Rocky Ice body, Rocky", "Materials", "Water", 100),
		new("Fumerola", "Fumerola Carbosis", 6284600, "Any except Helium", "Icy body, Rocky", "Materials", "CO2 Geysers, Min. Meth. Magma", 100),
		new("Fumerola", "Fumerola Extremus", 16202800, "Any except Helium", "HIGH METAL CONTENT BODY, Rocky body, Rocky Ice body", "Materials", "Min. Amm. Magma, Min. Nitr. Magma", 100),
		new("Fumerola", "Fumerola Nitris", 7500900, "Any except Helium", "Icy body", "Materials", "Min. Amm. Magma, Min. Nitr. Magma", 100),

		new("Fungoida", "Fungoida Bullarum", 3703200, "Argon, Nitrogen", "HIGH METAL CONTENT BODY, Rocky body, Rocky Ice body", "Materials", "None", 300),
		new("Fungoida", "Fungoida Gelata", 3330300, "Ammonia, Carbon Dioxide, Methane, Water", "HIGH METAL CONTENT BODY, Rocky body, Rocky Ice body", "Materials", "None, Maj. Sil. Vapour Geysers", 300),
		new("Fungoida", "Fungoida Setisis", 1670100, "Ammonia, Methane", "HIGH METAL CONTENT BODY, Rocky body, Rocky Ice body", "Materials", "-", 300),
		new("Fungoida", "Fungoida Stabitis", 2680300, "Carbon Dioxide, Water", "HIGH METAL CONTENT BODY, Rocky", "Materials", "None, Maj. Sil. Vapour Geysers", 300),

		new("Osseus", "Osseus Cornibus", 1483000, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 800),
		new("Osseus", "Osseus Discus", 12934900, "Ammonia, Argon, Methane, Water", "HIGH METAL CONTENT BODY, Rocky body, Rocky Ice body", "Materials", "Yes", 800),
		new("Osseus", "Osseus Fractus", 4027800, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 800),
		new("Osseus", "Osseus Pellebantus", 9739000, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 800),
		new("Osseus", "Osseus Pumice", 3156300, "Argon, Argon-rich, Methane, Nitrogen", "HIGH METAL CONTENT BODY, Rocky body, Rocky Ice body", "Materials", "None", 800),
		new("Osseus", "Osseus Spiralis", 2404700, "Ammonia", "HIGH METAL CONTENT BODY, Rocky body, Rocky Ice body", "Stars", "-", 800),

		new("Recepta", "Recepta Conditivus", 14313700, "Carbon Dioxide, Oxygen, Sulphur Dioxide", "Rocky body, Icy body", "Materials", "None", 150),
		new("Recepta", "Recepta Deltahedronix", 16202800, "Carbon Dioxide (bugged!), Sulphur Dioxide", "HIGH METAL CONTENT BODY, Rocky body, Icy body", "Materials", "None", 150),
		new("Recepta", "Recepta Umbrux", 12934900, "Carbon Dioxide, Sulphur Dioxide", "Icy body, Rocky body, Rocky Ice body", "Stars", "-", 150),

		new("Stratum", "Stratum Araneamus", 2448900, "Sulphur Dioxide", "Rocky", "Stars", "-", 500),
		new("Stratum", "Stratum Cucumisis", 16202800, "Carbon Dioxide, Oxygen, Sulphur Dioxide", "Rocky", "Stars", "-", 500),
		new("Stratum", "Stratum Excutitus", 2448900, "Argon-rich, Carbon Dioxide, Sulphur Dioxide", "Rocky", "Stars", "-", 500),
		new("Stratum", "Stratum Frigus", 2637500, "Carbon Dioxide, Sulphur Dioxide", "Rocky", "Stars", "None", 500),
		new("Stratum", "Stratum Laminamus", 2788300, "Ammonia", "Rocky", "Stars", "-", 500),
		new("Stratum", "Stratum Limaxus", 1362000, "Carbon Dioxide, Oxygen, Sulphur Dioxide", "Rocky", "Stars", "-", 500),
		new("Stratum", "Stratum Paleas", 1362000, "Ammonia, Carbon Dioxide, Oxygen, Water", "Rocky", "Stars", "-", 500),
		new("Stratum", "Stratum Tectonicas", 19010800, "Any except Helium, Methane, Neon", "HIGH METAL CONTENT BODY", "Stars", "-, for Water only: None", 500),

		new("Tubus", "Tubus Cavas", 11873200, "Carbon Dioxide", "Rocky", "Stars", "None", 800),
		new("Tubus", "Tubus Compagibus", 7774700, "Carbon Dioxide", "Rocky", "Stars", "None", 800),
		new("Tubus", "Tubus Conifer", 2415500, "Carbon Dioxide", "Rocky", "Stars", "None", 800),
		new("Tubus", "Tubus Rosarium", 2637500, "Ammonia", "Rocky", "Stars", "-", 800),
		new("Tubus", "Tubus Sororibus", 5727600, "Ammonia, Carbon Dioxide", "HIGH METAL CONTENT BODY", "Stars", "None", 800),

		new("Tussock", "Tussock Albata", 3252500, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 200),
		new("Tussock", "Tussock Capillum", 7025800, "Argon, Methane", "Rocky body, Rocky Ice body", "Stars", "None", 200),
		new("Tussock", "Tussock Caputus", 3472400, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 200),
		new("Tussock", "Tussock Catena", 1766600, "Ammonia", "HIGH METAL CONTENT BODY, Rocky", "Stars", "-", 200),
		new("Tussock", "Tussock Cultro", 1766600, "Ammonia", "HIGH METAL CONTENT BODY, Rocky", "Stars", "-", 200),
		new("Tussock", "Tussock Divisa", 1766600, "Ammonia", "HIGH METAL CONTENT BODY, Rocky", "Stars", "-", 200),
		new("Tussock", "Tussock Ignis", 1849000, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 200),
		new("Tussock", "Tussock Pennata", 5853800, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 200),
		new("Tussock", "Tussock Pennatis", 1000000, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 200),
		new("Tussock", "Tussock Propagito", 1000000, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 200),
		new("Tussock", "Tussock Serrati", 4447100, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 200),
		new("Tussock", "Tussock Stigmasis", 19010800, "Sulphur Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "-", 200),
		new("Tussock", "Tussock Triticum", 7774700, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 200),
		new("Tussock", "Tussock Ventusa", 3227700, "Carbon Dioxide", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None", 200),
		new("Tussock", "Tussock Virgam", 14313700, "Water", "HIGH METAL CONTENT BODY, Rocky", "Stars", "None, Minor Water Magma", 200)
	];
}
