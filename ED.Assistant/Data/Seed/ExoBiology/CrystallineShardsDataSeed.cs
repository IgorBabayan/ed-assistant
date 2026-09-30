namespace ED.Assistant.Data.Seed.ExoBiology;

internal static class CrystallineShardsDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder);
        SeedRule(modelBuilder);
        SeedAtmospheres(modelBuilder);
        SeedSystemBodyClasses(modelBuilder);
        SeedStars(modelBuilder);
    }
    
    private static void SeedGenus(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)ShardGenus.CrystallineShards,
                CodexType = "$Codex_Ent_Ground_Struct_Ice_Name;",
                CodexName = "$Codex_Ent_Ground_Struct_Ice_Name;",
                Name = "Crystalline Shards",
                Value = 1_628_800,
                Distance = 100
            });
    }

    private static void SeedRule(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ShardRule.CrystallineShards,
                GenusId = (int)ShardGenus.CrystallineShards,
                MaxGravity = 2.0,
                MaxTemperature = 273.0
            });
    }

    private static void SeedAtmospheres(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity("RuleAtmosphere").HasData(
            new[]
            {
                AtmosphereEnum.None,
                AtmosphereEnum.Argon,
                AtmosphereEnum.ArgonRich,
                AtmosphereEnum.CarbonDioxide,
                AtmosphereEnum.CarbonDioxideRich,
                AtmosphereEnum.Helium,
                AtmosphereEnum.Methane,
                AtmosphereEnum.Neon,
                AtmosphereEnum.NeonRich
            }.Select(x => new
            {
                RuleId = (int)ShardRule.CrystallineShards,
                AtmosphereId = (int)x
            }));
    }

    private static void SeedSystemBodyClasses(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity("RuleSystemBodyClass").HasData(
            new[]
            {
                BodyClassEnum.EarthLikeBody,
                BodyClassEnum.AmmoniaWorld,
                BodyClassEnum.WaterWorld,
                BodyClassEnum.GasGiantWithWaterBasedLife,
                BodyClassEnum.GasGiantWithAmmoniaBasedLife,
                BodyClassEnum.WaterGiant
            }.Select(x => new
            {
                RuleId = (int)ShardRule.CrystallineShards,
                BodyClassId = (int)x
            }));
    }

    private static void SeedStars(ModelBuilder modelBuilder)
    {
        var starClasses = new[]
        {
            StarClassEnum.A,
            StarClassEnum.F,
            StarClassEnum.G,
            StarClassEnum.K,
            StarClassEnum.MS,
            StarClassEnum.S
        };

        modelBuilder.Entity<RuleStar>().HasData(
            starClasses.Select((starClass, index) => new RuleStar
            {
                Id = (int)ShardRule.CrystallineShards * 100 + index + 1,
                RuleId = (int)ShardRule.CrystallineShards,
                StarClassId = (int)starClass,
                Type = RuleStarType.Star
            }));
    }
}