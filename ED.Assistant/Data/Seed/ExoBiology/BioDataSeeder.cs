namespace ED.Assistant.Data.Seed.ExoBiology;

internal static class ExoBilogyDataSeed
{
	public static void Seed(ModelBuilder modelBuilder)
	{
		SeedBodyClass(modelBuilder);
		SeedAtmosphere(modelBuilder);
		SeedVolcanism(modelBuilder);
		SeedStarClass(modelBuilder);
		SeedGenus(modelBuilder);
	}

	private static void SeedGenus(ModelBuilder modelBuilder)
	{
		AleoidaDataSeed.Seed(modelBuilder);
		AnemoneDataSeed.Seed(modelBuilder);
		BacteriumDataSeed.Seed(modelBuilder);
		BrainTreeDataSeed.Seed(modelBuilder);
		CactoidaDataSeed.Seed(modelBuilder);
		ClypeusDataSeed.Seed(modelBuilder);
		ConchaDataSeed.Seed(modelBuilder);
		ElectricaeDataSeed.Seed(modelBuilder);
		FonticuluaDataSeed.Seed(modelBuilder);
		FrutexaDataSeed.Seed(modelBuilder);
		FumerolaDataSeed.Seed(modelBuilder);
		FungoidaDataSeed.Seed(modelBuilder);
		OsseusDataSeed.Seed(modelBuilder);
		ReceptaDataSeed.Seed(modelBuilder);
		CrystallineShardsDataSeed.Seed(modelBuilder);
		StratumDataSeed.Seed(modelBuilder);
		TubersDataSeed.Seed(modelBuilder);
		TubusDataSeed.Seed(modelBuilder);
		TussockDataSeed.Seed(modelBuilder);
	}

	private static void SeedStarClass(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<StarClass>().HasData(
			Enum.GetValues<StarClassEnum>()
				.Select(x => new StarClass
				{
					Id = (int)x,
					Name = x.ToString()
				}));
	}

	private static void SeedVolcanism(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Volcanism>().HasData(
			new Volcanism { Id = (int)VolcanismEnum.None, Name = nameof(VolcanismEnum.None) },
			new Volcanism { Id = (int)VolcanismEnum.Any, Name = nameof(VolcanismEnum.Any) },
			new Volcanism { Id = (int)VolcanismEnum.Metallic, Name = nameof(VolcanismEnum.Metallic).ToLowerInvariant() },
			new Volcanism { Id = (int)VolcanismEnum.Silicate, Name = nameof(VolcanismEnum.Silicate).ToLowerInvariant() },
			new Volcanism { Id = (int)VolcanismEnum.Rocky, Name = nameof(VolcanismEnum.Rocky).ToLowerInvariant() },
			new Volcanism { Id = (int)VolcanismEnum.Water, Name = nameof(VolcanismEnum.Water).ToLowerInvariant() },
			new Volcanism { Id = (int)VolcanismEnum.CarbonDioxideGeysers, Name = "carbon dioxide geysers" },
			new Volcanism { Id = (int)VolcanismEnum.CarbonDioxide, Name = "carbon dioxide" },
			new Volcanism { Id = (int)VolcanismEnum.Methane, Name = nameof(VolcanismEnum.Methane).ToLowerInvariant() },
			new Volcanism { Id = (int)VolcanismEnum.Ammonia, Name = nameof(VolcanismEnum.Ammonia).ToLowerInvariant() },
			new Volcanism { Id = (int)VolcanismEnum.Nitrogen, Name = nameof(VolcanismEnum.Nitrogen).ToLowerInvariant() },
			new Volcanism { Id = (int)VolcanismEnum.Carbon, Name = nameof(VolcanismEnum.Carbon).ToLowerInvariant() },
			new Volcanism { Id = (int)VolcanismEnum.MethaneMagma, Name = "methane magma" },
			new Volcanism { Id = (int)VolcanismEnum.MajorSilicate, Name = "major silicate" },
			new Volcanism { Id = (int)VolcanismEnum.MajorRocky, Name = "major rocky" },
			new Volcanism { Id = (int)VolcanismEnum.MajorMetallic, Name = "major metallic" },
			new Volcanism { Id = (int)VolcanismEnum.Geysers, Name = nameof(VolcanismEnum.Geysers).ToLowerInvariant() },
			new Volcanism { Id = (int)VolcanismEnum.RockyMagma, Name = "rocky magma" },
			new Volcanism { Id = (int)VolcanismEnum.MajorRockyMagma, Name = "major rocky magma" },
			new Volcanism { Id = (int)VolcanismEnum.MajorSilicateVapour, Name = "major silicate vapour" },
			new Volcanism { Id = (int)VolcanismEnum.MajorMetallicMagma, Name = "major metallic magma" },
			new Volcanism { Id = (int)VolcanismEnum.MetallicMagmaVolcanism, Name = "metallic magma volcanism" },
			new Volcanism { Id = (int)VolcanismEnum.RockyMagmaVolcanism, Name = "rocky magma volcanism" }
		);
	}

	private static void SeedAtmosphere(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Atmosphere>().HasData(
			new Atmosphere { Id = (int)AtmosphereEnum.None, Name = nameof(AtmosphereEnum.None) },		
			new Atmosphere { Id = (int)AtmosphereEnum.CarbonDioxide, Name = nameof(AtmosphereEnum.CarbonDioxide) },		
			new Atmosphere { Id = (int)AtmosphereEnum.Ammonia, Name = nameof(AtmosphereEnum.Ammonia) },
			new Atmosphere { Id = (int)AtmosphereEnum.Helium, Name = nameof(AtmosphereEnum.Helium) },
			new Atmosphere { Id = (int)AtmosphereEnum.Argon, Name = nameof(AtmosphereEnum.Argon) },
			new Atmosphere { Id = (int)AtmosphereEnum.Methane, Name = nameof(AtmosphereEnum.Methane) },
			new Atmosphere { Id = (int)AtmosphereEnum.Neon, Name = nameof(AtmosphereEnum.Neon) },
			new Atmosphere { Id = (int)AtmosphereEnum.NeonRich, Name = nameof(AtmosphereEnum.NeonRich) },
			new Atmosphere { Id = (int)AtmosphereEnum.Nitrogen, Name = nameof(AtmosphereEnum.Nitrogen) },
			new Atmosphere { Id = (int)AtmosphereEnum.Oxygen, Name = nameof(AtmosphereEnum.Oxygen) },
			new Atmosphere { Id = (int)AtmosphereEnum.ArgonRich, Name = nameof(AtmosphereEnum.ArgonRich) },
			new Atmosphere { Id = (int)AtmosphereEnum.CarbonDioxideRich, Name = nameof(AtmosphereEnum.CarbonDioxideRich) },
			new Atmosphere { Id = (int)AtmosphereEnum.SulphurDioxide, Name = nameof(AtmosphereEnum.SulphurDioxide) },
			new Atmosphere { Id = (int)AtmosphereEnum.Water, Name = nameof(AtmosphereEnum.Water) },
			new Atmosphere { Id = (int)AtmosphereEnum.WaterRich, Name = nameof(AtmosphereEnum.WaterRich) },
			new Atmosphere { Id = (int)AtmosphereEnum.MethaneRich, Name = nameof(AtmosphereEnum.MethaneRich) }
		);
	}

	private static void SeedBodyClass(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<BodyClass>().HasData(
			new BodyClass { Id = (int)BodyClassEnum.RockyBody, Name = "Rocky body" },
			new BodyClass { Id = (int)BodyClassEnum.HighMetalContentBody, Name = "High metal content body" },
			new BodyClass { Id = (int)BodyClassEnum.IcyBody, Name = "Icy body" },
			new BodyClass { Id = (int)BodyClassEnum.RockyIceBody, Name = "Rocky ice body" },
			new BodyClass { Id = (int)BodyClassEnum.MetalRichBody, Name = "Metal rich body" },
			new BodyClass { Id = (int)BodyClassEnum.EarthLikeBody, Name = "Earthlike body" },
			new BodyClass { Id = (int)BodyClassEnum.GasGiantWithWaterBasedLife, Name = "Gas giant with water based life" },
			new BodyClass { Id = (int)BodyClassEnum.WaterGiant, Name = "Water giant" },
			new BodyClass { Id = (int)BodyClassEnum.AmmoniaWorld, Name = "Ammonia world" },
			new BodyClass { Id = (int)BodyClassEnum.WaterWorld, Name = "Water world" },
			new BodyClass { Id = (int)BodyClassEnum.GasGiantWithAmmoniaBasedLife, Name = "Gas giant with ammonia based life" }
		);
	}
}
