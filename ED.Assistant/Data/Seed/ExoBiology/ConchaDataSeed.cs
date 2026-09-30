namespace ED.Assistant.Data.Seed.ExoBiology;

internal static class ConchaDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedRenibus(modelBuilder);
        SeedAureolas(modelBuilder);
        SeedLabiata(modelBuilder);
        SeedBiconcavis(modelBuilder);
    }

    private static void SeedBiconcavis(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, ConchaGenus.Biconcavis, "$Codex_Ent_Conchas_04_Name;", "Concha Biconcavis", 19_010_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ConchaRule.Biconcavis,
                GenusId = (int)ConchaGenus.Biconcavis,
                MinGravity = 0.053,
                MaxGravity = 0.275,
                MinTemperature = 42.0,
                MaxTemperature = 52.0,
                MaxPressure = 0.0047
            });

        SeedHelpers.Atmospheres(modelBuilder, ConchaRule.Biconcavis, AtmosphereEnum.Nitrogen);
        SeedHelpers.BodyClasses(modelBuilder, ConchaRule.Biconcavis,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
        SeedHelpers.Volcanisms(modelBuilder, ConchaRule.Biconcavis, VolcanismEnum.None);
    }

    private static void SeedLabiata(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, ConchaGenus.Labiata, "$Codex_Ent_Conchas_03_Name;", "Concha Labiata", 2_352_400);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ConchaRule.Labiata,
                GenusId = (int)ConchaGenus.Labiata,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 150.0,
                MaxTemperature = 200.0,
                MinPressure = 0.002
            });

        SeedHelpers.Atmospheres(modelBuilder, ConchaRule.Labiata, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.BodyClasses(modelBuilder, ConchaRule.Labiata,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
        SeedHelpers.Volcanisms(modelBuilder, ConchaRule.Labiata, VolcanismEnum.None);
    }

    private static void SeedAureolas(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, ConchaGenus.Aureolas, "$Codex_Ent_Conchas_02_Name;", "Concha Aureolas", 7_774_700);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ConchaRule.Aureolas,
                GenusId = (int)ConchaGenus.Aureolas,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 152.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            });

        SeedHelpers.Atmospheres(modelBuilder, ConchaRule.Aureolas, AtmosphereEnum.Ammonia);
        SeedHelpers.BodyClasses(modelBuilder, ConchaRule.Aureolas,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
    }

    private static void SeedRenibus(ModelBuilder modelBuilder)
    {
                SeedGenus(modelBuilder, ConchaGenus.Renibus, "$Codex_Ent_Conchas_01_Name;", "Concha Renibus", 4_572_400);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ConchaRule.RenibusAmmonia,
                GenusId = (int)ConchaGenus.Renibus,
                MinGravity = 0.04,
                MaxGravity = 0.045,
                MinTemperature = 176.0,
                MaxTemperature = 177.0
            },
            new Rule
            {
                Id = (int)ConchaRule.RenibusCarbonDioxide,
                GenusId = (int)ConchaGenus.Renibus,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 180.0,
                MinPressure = 0.025
            },
            new Rule
            {
                Id = (int)ConchaRule.RenibusMethane,
                GenusId = (int)ConchaGenus.Renibus,
                MinGravity = 0.04,
                MaxGravity = 0.15,
                MinTemperature = 78.0,
                MaxTemperature = 100.0,
                MinPressure = 0.01
            },
            new Rule
            {
                Id = (int)ConchaRule.RenibusWaterNone,
                GenusId = (int)ConchaGenus.Renibus,
                MinGravity = 0.04,
                MaxGravity = 0.65
            },
            new Rule
            {
                Id = (int)ConchaRule.RenibusWaterVolcanism,
                GenusId = (int)ConchaGenus.Renibus,
                MinGravity = 0.04,
                MaxGravity = 0.65
            });

        SeedHelpers.Atmospheres(modelBuilder, ConchaRule.RenibusAmmonia, AtmosphereEnum.Ammonia);
        SeedHelpers.Atmospheres(modelBuilder, ConchaRule.RenibusCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, ConchaRule.RenibusMethane, AtmosphereEnum.Methane);
        SeedHelpers.Atmospheres(modelBuilder, ConchaRule.RenibusWaterNone, AtmosphereEnum.Water);
        SeedHelpers.Atmospheres(modelBuilder, ConchaRule.RenibusWaterVolcanism, AtmosphereEnum.Water);

        foreach (var rule in new[]
        {
            ConchaRule.RenibusAmmonia,
            ConchaRule.RenibusCarbonDioxide,
            ConchaRule.RenibusMethane,
            ConchaRule.RenibusWaterNone,
            ConchaRule.RenibusWaterVolcanism
        })
        {
            SeedHelpers.BodyClasses(modelBuilder, rule,
                BodyClassEnum.RockyBody,
                BodyClassEnum.HighMetalContentBody);
        }

        SeedHelpers.Volcanisms(modelBuilder, ConchaRule.RenibusAmmonia,
            VolcanismEnum.Silicate,
            VolcanismEnum.Metallic);

        SeedHelpers.Volcanisms(modelBuilder, ConchaRule.RenibusCarbonDioxide,
            VolcanismEnum.None);

        SeedHelpers.Volcanisms(modelBuilder, ConchaRule.RenibusMethane,
            VolcanismEnum.Silicate,
            VolcanismEnum.Metallic);

        SeedHelpers.Volcanisms(modelBuilder, ConchaRule.RenibusWaterNone,
            VolcanismEnum.None);

        SeedHelpers.Volcanisms(modelBuilder, ConchaRule.RenibusWaterVolcanism,
            VolcanismEnum.Water);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        ConchaGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Conchas_Genus_Name;",
                CodexName = codexName,
                Name = name,
                Value = value,
                Distance = 150
            });
    }
}