namespace ED.Assistant.Data.Seed.ExoBiology;

internal static class FrutexaDataSeed
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

        SeedHelpers.Atmospheres(modelBuilder, FrutexaRule.CollumRocky, AtmosphereEnum.SulphurDioxide);
        SeedHelpers.Atmospheres(modelBuilder, FrutexaRule.CollumHighMetalContent, AtmosphereEnum.SulphurDioxide);

        SeedHelpers.BodyClasses(modelBuilder, FrutexaRule.CollumRocky, BodyClassEnum.RockyBody);
        SeedHelpers.BodyClasses(modelBuilder, FrutexaRule.CollumHighMetalContent, BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(modelBuilder, FrutexaRule.CollumHighMetalContent, VolcanismEnum.None);
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

        SeedHelpers.Atmospheres(modelBuilder, FrutexaRule.SponsaeNoVolcanism, AtmosphereEnum.Water);
        SeedHelpers.Atmospheres(modelBuilder, FrutexaRule.SponsaeWaterVolcanism, AtmosphereEnum.Water);

        SeedHelpers.BodyClasses(modelBuilder, FrutexaRule.SponsaeNoVolcanism, BodyClassEnum.RockyBody);
        SeedHelpers.BodyClasses(modelBuilder, FrutexaRule.SponsaeWaterVolcanism, BodyClassEnum.RockyBody);

        SeedHelpers.Volcanisms(modelBuilder, FrutexaRule.SponsaeNoVolcanism, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, FrutexaRule.SponsaeWaterVolcanism, VolcanismEnum.Water);
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

        SeedHelpers.Atmospheres(modelBuilder, FrutexaRule.Fera, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.BodyClasses(modelBuilder, FrutexaRule.Fera, BodyClassEnum.RockyBody);
        SeedHelpers.Volcanisms(modelBuilder, FrutexaRule.Fera, VolcanismEnum.None);
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

        SeedHelpers.Atmospheres(modelBuilder, FrutexaRule.Flammasis, AtmosphereEnum.Ammonia);
        SeedHelpers.BodyClasses(modelBuilder, FrutexaRule.Flammasis, BodyClassEnum.RockyBody);
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

        SeedHelpers.Atmospheres(modelBuilder, FrutexaRule.MetallicumAmmonia, AtmosphereEnum.Ammonia);
        SeedHelpers.Atmospheres(modelBuilder, FrutexaRule.MetallicumCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, FrutexaRule.MetallicumMethane, AtmosphereEnum.Methane);
        SeedHelpers.Atmospheres(modelBuilder, FrutexaRule.MetallicumWater, AtmosphereEnum.Water);

        SeedHelpers.BodyClasses(modelBuilder, FrutexaRule.MetallicumAmmonia, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, FrutexaRule.MetallicumCarbonDioxide, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, FrutexaRule.MetallicumMethane, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, FrutexaRule.MetallicumWater, BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(modelBuilder, FrutexaRule.MetallicumAmmonia, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, FrutexaRule.MetallicumCarbonDioxide, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, FrutexaRule.MetallicumWater, VolcanismEnum.None);
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

        SeedHelpers.Atmospheres(modelBuilder, FrutexaRule.Acus, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.BodyClasses(modelBuilder, FrutexaRule.Acus, BodyClassEnum.RockyBody);
        SeedHelpers.Volcanisms(modelBuilder, FrutexaRule.Acus, VolcanismEnum.None);
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

        SeedHelpers.Atmospheres(modelBuilder, FrutexaRule.Flabellum, AtmosphereEnum.Ammonia);
        SeedHelpers.BodyClasses(modelBuilder, FrutexaRule.Flabellum, BodyClassEnum.RockyBody);
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
}