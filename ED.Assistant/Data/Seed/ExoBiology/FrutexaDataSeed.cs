namespace ED.Assistant.Data.Seed.ExoBiology;

static class FrutexaDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedFlabellum(modelBuilder);
        SeedAcus(modelBuilder);
        SeedMetallicum(modelBuilder);
        SeedFlammasis(modelBuilder);
        SeedFera(modelBuilder);
        SeedSponsae(modelBuilder);
        SeedCollum(modelBuilder);
    }

    private static void SeedCollum(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, FrutexaGenus.Collum, "$Codex_Ent_Shrubs_07_Name;", "Frutexa Collum", 1_639_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FrutexaRule.CollumRocky,
                GenusId = (int)FrutexaGenus.Collum,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 132.0,
                MaxTemperature = 215.0,
                MaxPressure = 0.004
            },
            new Rule
            {
                Id = (int)FrutexaRule.CollumHighMetalContent,
                GenusId = (int)FrutexaGenus.Collum,
                MinGravity = 0.265,
                MaxGravity = 0.276,
                MinTemperature = 132.0,
                MaxTemperature = 135.0,
                MaxPressure = 0.004
            });

        SeedAtmospheres(modelBuilder, FrutexaRule.CollumRocky, AtmosphereEnum.SulphurDioxide);
        SeedAtmospheres(modelBuilder, FrutexaRule.CollumHighMetalContent, AtmosphereEnum.SulphurDioxide);

        SeedBodyClasses(modelBuilder, FrutexaRule.CollumRocky, BodyClassEnum.RockyBody);
        SeedBodyClasses(modelBuilder, FrutexaRule.CollumHighMetalContent, BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(modelBuilder, FrutexaRule.CollumHighMetalContent, VolcanismEnum.None);
    }

    private static void SeedSponsae(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, FrutexaGenus.Sponsae, "$Codex_Ent_Shrubs_06_Name;", "Frutexa Sponsae", 5_988_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FrutexaRule.SponsaeNoVolcanism,
                GenusId = (int)FrutexaGenus.Sponsae,
                MinGravity = 0.04,
                MaxGravity = 0.056
            },
            new Rule
            {
                Id = (int)FrutexaRule.SponsaeWaterVolcanism,
                GenusId = (int)FrutexaGenus.Sponsae,
                MinGravity = 0.04,
                MaxGravity = 0.056
            });

        SeedAtmospheres(modelBuilder, FrutexaRule.SponsaeNoVolcanism, AtmosphereEnum.Water);
        SeedAtmospheres(modelBuilder, FrutexaRule.SponsaeWaterVolcanism, AtmosphereEnum.Water);

        SeedBodyClasses(modelBuilder, FrutexaRule.SponsaeNoVolcanism, BodyClassEnum.RockyBody);
        SeedBodyClasses(modelBuilder, FrutexaRule.SponsaeWaterVolcanism, BodyClassEnum.RockyBody);

        SeedVolcanisms(modelBuilder, FrutexaRule.SponsaeNoVolcanism, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, FrutexaRule.SponsaeWaterVolcanism, VolcanismEnum.Water);
    }

    private static void SeedFera(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, FrutexaGenus.Fera, "$Codex_Ent_Shrubs_05_Name;", "Frutexa Fera", 1_632_500);

        modelBuilder.Entity<Rule>().HasData(new Rule
        {
            Id = (int)FrutexaRule.Fera,
            GenusId = (int)FrutexaGenus.Fera,
            MinGravity = 0.04,
            MaxGravity = 0.276,
            MinTemperature = 146.0,
            MaxTemperature = 197.0,
            MinPressure = 0.003
        });

        SeedAtmospheres(modelBuilder, FrutexaRule.Fera, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(modelBuilder, FrutexaRule.Fera, BodyClassEnum.RockyBody);
        SeedVolcanisms(modelBuilder, FrutexaRule.Fera, VolcanismEnum.None);
    }

    private static void SeedFlammasis(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, FrutexaGenus.Flammasis, "$Codex_Ent_Shrubs_04_Name;", "Frutexa Flammasis", 10_326_000);

        modelBuilder.Entity<Rule>().HasData(new Rule
        {
            Id = (int)FrutexaRule.Flammasis,
            GenusId = (int)FrutexaGenus.Flammasis,
            MinGravity = 0.04,
            MaxGravity = 0.276,
            MinTemperature = 152.0,
            MaxTemperature = 177.0,
            MaxPressure = 0.0135
        });

        SeedAtmospheres(modelBuilder, FrutexaRule.Flammasis, AtmosphereEnum.Ammonia);
        SeedBodyClasses(modelBuilder, FrutexaRule.Flammasis, BodyClassEnum.RockyBody);
    }

    private static void SeedMetallicum(ModelBuilder modelBuilder)
    {
                SeedGenus(modelBuilder, FrutexaGenus.Metallicum, "$Codex_Ent_Shrubs_03_Name;", "Frutexa Metallicum", 1_632_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FrutexaRule.MetallicumAmmonia,
                GenusId = (int)FrutexaGenus.Metallicum,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 152.0,
                MaxTemperature = 176.0,
                MaxPressure = 0.01
            },
            new Rule
            {
                Id = (int)FrutexaRule.MetallicumCarbonDioxide,
                GenusId = (int)FrutexaGenus.Metallicum,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 146.0,
                MaxTemperature = 197.0,
                MinPressure = 0.002
            },
            new Rule
            {
                Id = (int)FrutexaRule.MetallicumMethane,
                GenusId = (int)FrutexaGenus.Metallicum,
                MinGravity = 0.05,
                MaxGravity = 0.1,
                MinTemperature = 100.0,
                MaxTemperature = 300.0
            },
            new Rule
            {
                Id = (int)FrutexaRule.MetallicumWater,
                GenusId = (int)FrutexaGenus.Metallicum,
                MinGravity = 0.04,
                MaxGravity = 0.07,
                MaxTemperature = 400.0,
                MaxPressure = 0.07
            });

        SeedAtmospheres(modelBuilder, FrutexaRule.MetallicumAmmonia, AtmosphereEnum.Ammonia);
        SeedAtmospheres(modelBuilder, FrutexaRule.MetallicumCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedAtmospheres(modelBuilder, FrutexaRule.MetallicumMethane, AtmosphereEnum.Methane);
        SeedAtmospheres(modelBuilder, FrutexaRule.MetallicumWater, AtmosphereEnum.Water);

        SeedBodyClasses(modelBuilder, FrutexaRule.MetallicumAmmonia, BodyClassEnum.HighMetalContentBody);
        SeedBodyClasses(modelBuilder, FrutexaRule.MetallicumCarbonDioxide, BodyClassEnum.HighMetalContentBody);
        SeedBodyClasses(modelBuilder, FrutexaRule.MetallicumMethane, BodyClassEnum.HighMetalContentBody);
        SeedBodyClasses(modelBuilder, FrutexaRule.MetallicumWater, BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(modelBuilder, FrutexaRule.MetallicumAmmonia, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, FrutexaRule.MetallicumCarbonDioxide, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, FrutexaRule.MetallicumWater, VolcanismEnum.None);
    }

    private static void SeedAcus(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, FrutexaGenus.Acus, "$Codex_Ent_Shrubs_02_Name;", "Frutexa Acus", 7_774_700);

        modelBuilder.Entity<Rule>().HasData(new Rule
        {
            Id = (int)FrutexaRule.Acus,
            GenusId = (int)FrutexaGenus.Acus,
            MinGravity = 0.04,
            MaxGravity = 0.237,
            MinTemperature = 146.0,
            MaxTemperature = 197.0,
            MinPressure = 0.0029
        });

        SeedAtmospheres(modelBuilder, FrutexaRule.Acus, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(modelBuilder, FrutexaRule.Acus, BodyClassEnum.RockyBody);
        SeedVolcanisms(modelBuilder, FrutexaRule.Acus, VolcanismEnum.None);
    }

    private static void SeedFlabellum(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, FrutexaGenus.Flabellum, "$Codex_Ent_Shrubs_01_Name;", "Frutexa Flabellum", 1_808_900);

        modelBuilder.Entity<Rule>().HasData(new Rule
        {
            Id = (int)FrutexaRule.Flabellum,
            GenusId = (int)FrutexaGenus.Flabellum,
            MinGravity = 0.04,
            MaxGravity = 0.276,
            MinTemperature = 152.0,
            MaxTemperature = 177.0,
            MaxPressure = 0.0135
        });

        SeedAtmospheres(modelBuilder, FrutexaRule.Flabellum, AtmosphereEnum.Ammonia);
        SeedBodyClasses(modelBuilder, FrutexaRule.Flabellum, BodyClassEnum.RockyBody);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        FrutexaGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(new Genus
        {
            Id = (int)id,
            CodexType = "$Codex_Ent_Shrubs_Genus_Name;",
            CodexName = codexName,
            Name = name,
            Value = value,
            Distance = 150
        });
    }

    private static void SeedAtmospheres(
        ModelBuilder modelBuilder,
        FrutexaRule rule,
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
        FrutexaRule rule,
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
        FrutexaRule rule,
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