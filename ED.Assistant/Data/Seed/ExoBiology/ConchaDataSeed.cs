namespace ED.Assistant.Data.Seed.ExoBiology;

static class ConchaDataSeed
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
        SeedGenus(modelBuilder, ConchaGenus.Biconcavis, "$Codex_Ent_Conchas_04_Name;", "Concha Biconcavis", 16_777_215);

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

        SeedAtmospheres(modelBuilder, ConchaRule.Biconcavis, AtmosphereEnum.Nitrogen);
        SeedBodyClasses(modelBuilder, ConchaRule.Biconcavis,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, ConchaRule.Biconcavis, VolcanismEnum.None);
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

        SeedAtmospheres(modelBuilder, ConchaRule.Labiata, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(modelBuilder, ConchaRule.Labiata,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, ConchaRule.Labiata, VolcanismEnum.None);
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

        SeedAtmospheres(modelBuilder, ConchaRule.Aureolas, AtmosphereEnum.Ammonia);
        SeedBodyClasses(modelBuilder, ConchaRule.Aureolas,
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

        SeedAtmospheres(modelBuilder, ConchaRule.RenibusAmmonia, AtmosphereEnum.Ammonia);
        SeedAtmospheres(modelBuilder, ConchaRule.RenibusCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedAtmospheres(modelBuilder, ConchaRule.RenibusMethane, AtmosphereEnum.Methane);
        SeedAtmospheres(modelBuilder, ConchaRule.RenibusWaterNone, AtmosphereEnum.Water);
        SeedAtmospheres(modelBuilder, ConchaRule.RenibusWaterVolcanism, AtmosphereEnum.Water);

        foreach (var rule in new[]
        {
            ConchaRule.RenibusAmmonia,
            ConchaRule.RenibusCarbonDioxide,
            ConchaRule.RenibusMethane,
            ConchaRule.RenibusWaterNone,
            ConchaRule.RenibusWaterVolcanism
        })
        {
            SeedBodyClasses(modelBuilder, rule,
                BodyClassEnum.RockyBody,
                BodyClassEnum.HighMetalContentBody);
        }

        SeedVolcanisms(modelBuilder, ConchaRule.RenibusAmmonia,
            VolcanismEnum.Silicate,
            VolcanismEnum.Metallic);

        SeedVolcanisms(modelBuilder, ConchaRule.RenibusCarbonDioxide,
            VolcanismEnum.None);

        SeedVolcanisms(modelBuilder, ConchaRule.RenibusMethane,
            VolcanismEnum.Silicate,
            VolcanismEnum.Metallic);

        SeedVolcanisms(modelBuilder, ConchaRule.RenibusWaterNone,
            VolcanismEnum.None);

        SeedVolcanisms(modelBuilder, ConchaRule.RenibusWaterVolcanism,
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

    private static void SeedAtmospheres(
        ModelBuilder modelBuilder,
        ConchaRule rule,
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
        ConchaRule rule,
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
        ConchaRule rule,
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