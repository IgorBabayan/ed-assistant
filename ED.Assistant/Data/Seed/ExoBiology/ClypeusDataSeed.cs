namespace ED.Assistant.Data.Seed.ExoBiology;

static class ClypeusDataSeed
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
                MinPressure = 0.055,
            },
            new Rule
            {
                Id = (int)ClypeusRule.SpeculumiWaterNone,
                GenusId = (int)ClypeusGenus.Speculumi,
                MinGravity = 0.04,
                MaxGravity = 0.276,
            },
            new Rule
            {
                Id = (int)ClypeusRule.SpeculumiWaterVolcanism,
                GenusId = (int)ClypeusGenus.Speculumi,
                MinGravity = 0.04,
                MaxGravity = 0.276,
            });

        SeedAtmospheres(modelBuilder, ClypeusRule.SpeculumiCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedAtmospheres(modelBuilder, ClypeusRule.SpeculumiWaterNone, AtmosphereEnum.Water);
        SeedAtmospheres(modelBuilder, ClypeusRule.SpeculumiWaterVolcanism, AtmosphereEnum.Water);

        SeedBodyClasses(modelBuilder, ClypeusRule.SpeculumiCarbonDioxide, BodyClassEnum.RockyBody);
        SeedBodyClasses(modelBuilder, ClypeusRule.SpeculumiWaterNone, BodyClassEnum.RockyBody);
        SeedBodyClasses(modelBuilder, ClypeusRule.SpeculumiWaterVolcanism, BodyClassEnum.RockyBody);

        SeedVolcanisms(modelBuilder, ClypeusRule.SpeculumiCarbonDioxide, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, ClypeusRule.SpeculumiWaterNone, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, ClypeusRule.SpeculumiWaterVolcanism, VolcanismEnum.Water);
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

        SeedAtmospheres(modelBuilder, ClypeusRule.MargaritusCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedAtmospheres(modelBuilder, ClypeusRule.MargaritusWater, AtmosphereEnum.Water);

        SeedBodyClasses(modelBuilder, ClypeusRule.MargaritusCarbonDioxide, BodyClassEnum.HighMetalContentBody);
        SeedBodyClasses(modelBuilder, ClypeusRule.MargaritusWater, BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(modelBuilder, ClypeusRule.MargaritusCarbonDioxide, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, ClypeusRule.MargaritusWater, VolcanismEnum.None);
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

        SeedAtmospheres(modelBuilder, ClypeusRule.LacrimamCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedAtmospheres(modelBuilder, ClypeusRule.LacrimamWaterNone, AtmosphereEnum.Water);
        SeedAtmospheres(modelBuilder, ClypeusRule.LacrimamWaterVolcanism, AtmosphereEnum.Water);

        SeedBodyClasses(modelBuilder, ClypeusRule.LacrimamCarbonDioxide, BodyClassEnum.RockyBody);
        SeedBodyClasses(modelBuilder, ClypeusRule.LacrimamWaterNone, BodyClassEnum.RockyBody);
        SeedBodyClasses(modelBuilder, ClypeusRule.LacrimamWaterVolcanism, BodyClassEnum.RockyBody);

        SeedVolcanisms(modelBuilder, ClypeusRule.LacrimamCarbonDioxide, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, ClypeusRule.LacrimamWaterNone, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, ClypeusRule.LacrimamWaterVolcanism, VolcanismEnum.Water);
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

    private static void SeedAtmospheres(
        ModelBuilder modelBuilder,
        ClypeusRule rule,
        params AtmosphereEnum[] atmospheres)
    {
        modelBuilder.Entity("RuleAtmosphere").HasData(
            atmospheres.Select(x => new
            {
                RuleId = (int)rule,
                AtmosphereId = (int)x
            }));
    }

    private static void SeedBodyClasses(
        ModelBuilder modelBuilder,
        ClypeusRule rule,
        params BodyClassEnum[] bodyClasses)
    {
        modelBuilder.Entity("RuleBodyClass").HasData(
            bodyClasses.Select(x => new
            {
                RuleId = (int)rule,
                BodyClassId = (int)x
            }));
    }

    private static void SeedVolcanisms(
        ModelBuilder modelBuilder,
        ClypeusRule rule,
        params VolcanismEnum[] volcanisms)
    {
        modelBuilder.Entity("RuleVolcanism").HasData(
            volcanisms.Select(x => new
            {
                RuleId = (int)rule,
                VolcanismId = (int)x
            }));
    }
}