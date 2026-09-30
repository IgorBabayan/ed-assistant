namespace ED.Assistant.Data.Seed.ExoBiology;

internal static class ClypeusDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedLacrimam(modelBuilder);
        SeedMargaritus(modelBuilder);
        SeedSpeculumi(modelBuilder);
    }

    private static void SeedSpeculumi(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            ClypeusGenus.Speculumi,
            "$Codex_Ent_Clypeus_03_Name;",
            "Clypeus Speculumi",
            16_202_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ClypeusRule.SpeculumiCarbonDioxide,
                GenusId = (int)ClypeusGenus.Speculumi,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 190.0,
                MaxTemperature = 197.0,
                MinPressure = 0.055
            },
            new Rule
            {
                Id = (int)ClypeusRule.SpeculumiWaterNone,
                GenusId = (int)ClypeusGenus.Speculumi,
                MinGravity = 0.04,
                MaxGravity = 0.276
            },
            new Rule
            {
                Id = (int)ClypeusRule.SpeculumiWaterVolcanism,
                GenusId = (int)ClypeusGenus.Speculumi,
                MinGravity = 0.04,
                MaxGravity = 0.276
            });

        SeedHelpers.Atmospheres(modelBuilder, ClypeusRule.SpeculumiCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, ClypeusRule.SpeculumiWaterNone, AtmosphereEnum.Water);
        SeedHelpers.Atmospheres(modelBuilder, ClypeusRule.SpeculumiWaterVolcanism, AtmosphereEnum.Water);

        SeedHelpers.BodyClasses(modelBuilder, ClypeusRule.SpeculumiCarbonDioxide, BodyClassEnum.RockyBody);
        SeedHelpers.BodyClasses(modelBuilder, ClypeusRule.SpeculumiWaterNone, BodyClassEnum.RockyBody);
        SeedHelpers.BodyClasses(modelBuilder, ClypeusRule.SpeculumiWaterVolcanism, BodyClassEnum.RockyBody);

        SeedHelpers.Volcanisms(modelBuilder, ClypeusRule.SpeculumiCarbonDioxide, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, ClypeusRule.SpeculumiWaterNone, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, ClypeusRule.SpeculumiWaterVolcanism, VolcanismEnum.Water);
    }

    private static void SeedMargaritus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            ClypeusGenus.Margaritus,
            "$Codex_Ent_Clypeus_02_Name;",
            "Clypeus Margaritus",
            11_873_200);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ClypeusRule.MargaritusCarbonDioxide,
                GenusId = (int)ClypeusGenus.Margaritus,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 190.0,
                MaxTemperature = 197.0,
                MinPressure = 0.054
            },
            new Rule
            {
                Id = (int)ClypeusRule.MargaritusWater,
                GenusId = (int)ClypeusGenus.Margaritus,
                MinGravity = 0.04,
                MaxGravity = 0.276
            });

        SeedHelpers.Atmospheres(modelBuilder, ClypeusRule.MargaritusCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, ClypeusRule.MargaritusWater, AtmosphereEnum.Water);

        SeedHelpers.BodyClasses(modelBuilder, ClypeusRule.MargaritusCarbonDioxide, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, ClypeusRule.MargaritusWater, BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(modelBuilder, ClypeusRule.MargaritusCarbonDioxide, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, ClypeusRule.MargaritusWater, VolcanismEnum.None);
    }

    private static void SeedLacrimam(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            ClypeusGenus.Lacrimam,
            "$Codex_Ent_Clypeus_01_Name;",
            "Clypeus Lacrimam",
            8_418_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ClypeusRule.LacrimamCarbonDioxide,
                GenusId = (int)ClypeusGenus.Lacrimam,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 190.0,
                MinPressure = 0.054
            },
            new Rule
            {
                Id = (int)ClypeusRule.LacrimamWaterNone,
                GenusId = (int)ClypeusGenus.Lacrimam,
                MinGravity = 0.04,
                MaxGravity = 0.276
            },
            new Rule
            {
                Id = (int)ClypeusRule.LacrimamWaterVolcanism,
                GenusId = (int)ClypeusGenus.Lacrimam,
                MinGravity = 0.04,
                MaxGravity = 0.276
            });

        SeedHelpers.Atmospheres(modelBuilder, ClypeusRule.LacrimamCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, ClypeusRule.LacrimamWaterNone, AtmosphereEnum.Water);
        SeedHelpers.Atmospheres(modelBuilder, ClypeusRule.LacrimamWaterVolcanism, AtmosphereEnum.Water);

        SeedHelpers.BodyClasses(modelBuilder, ClypeusRule.LacrimamCarbonDioxide, BodyClassEnum.RockyBody);
        SeedHelpers.BodyClasses(modelBuilder, ClypeusRule.LacrimamWaterNone, BodyClassEnum.RockyBody);
        SeedHelpers.BodyClasses(modelBuilder, ClypeusRule.LacrimamWaterVolcanism, BodyClassEnum.RockyBody);

        SeedHelpers.Volcanisms(modelBuilder, ClypeusRule.LacrimamCarbonDioxide, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, ClypeusRule.LacrimamWaterNone, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, ClypeusRule.LacrimamWaterVolcanism, VolcanismEnum.Water);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        ClypeusGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Clypeus_Genus_Name;",
                CodexName = codexName,
                Name = name,
                Value = value,
                Distance = 150
            });
    }
}