namespace ED.Assistant.Data.Seed.ExoBiology;

static class AleoidaDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedArcus(modelBuilder);
        SeedCoronamus(modelBuilder);
        SeedSpica(modelBuilder);
        SeedLaminiae(modelBuilder);
        SeedGravis(modelBuilder);
    }

    private static void SeedArcus(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AleoidaGenus.Arcus,
                CodexType = "$Codex_Ent_Aleoids_Genus_Name;",
                CodexName = "$Codex_Ent_Aleoids_01_Name;",
                Name = "Aleoida Arcus",
                Value = 7_252_500,
                Distance = 150
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AleoidaRule.Arcus,
                GenusId = (int)AleoidaGenus.Arcus,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 175.0,
                MaxTemperature = 180.0,
                MinPressure = 0.0161
            });

        SeedHelpers.BodyClasses(modelBuilder, AleoidaRule.Arcus, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.Atmospheres(modelBuilder, AleoidaRule.Arcus, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Volcanisms(modelBuilder, AleoidaRule.Arcus, VolcanismEnum.None);
    }

    private static void SeedCoronamus(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AleoidaGenus.Coronamus,
                CodexType = "$Codex_Ent_Aleoids_Genus_Name;",
                CodexName = "$Codex_Ent_Aleoids_02_Name;",
                Name = "Aleoida Coronamus",
                Value = 6_284_600,
                Distance = 150
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AleoidaRule.Coronamus,
                GenusId = (int)AleoidaGenus.Coronamus,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 180.0,
                MaxTemperature = 190.0,
                MinPressure = 0.025
            });

        SeedHelpers.BodyClasses(modelBuilder, AleoidaRule.Coronamus, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.Atmospheres(modelBuilder, AleoidaRule.Coronamus, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Volcanisms(modelBuilder, AleoidaRule.Coronamus, VolcanismEnum.None);
    }

    private static void SeedSpica(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AleoidaGenus.Spica,
                CodexType = "$Codex_Ent_Aleoids_Genus_Name;",
                CodexName = "$Codex_Ent_Aleoids_03_Name;",
                Name = "Aleoida Spica",
                Value = 3_385_200,
                Distance = 150
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AleoidaRule.Spica,
                GenusId = (int)AleoidaGenus.Spica,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 170.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            });

        SeedHelpers.BodyClasses(modelBuilder, AleoidaRule.Spica, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.Atmospheres(modelBuilder, AleoidaRule.Spica, AtmosphereEnum.Ammonia);
    }

    private static void SeedLaminiae(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AleoidaGenus.Laminiae,
                CodexType = "$Codex_Ent_Aleoids_Genus_Name;",
                CodexName = "$Codex_Ent_Aleoids_04_Name;",
                Name = "Aleoida Laminiae",
                Value = 3_385_200,
                Distance = 150
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AleoidaRule.Laminiae,
                GenusId = (int)AleoidaGenus.Laminiae,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 152.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            });

        SeedHelpers.BodyClasses(modelBuilder, AleoidaRule.Laminiae, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.Atmospheres(modelBuilder, AleoidaRule.Laminiae, AtmosphereEnum.Ammonia);
    }

    private static void SeedGravis(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AleoidaGenus.Gravis,
                CodexType = "$Codex_Ent_Aleoids_Genus_Name;",
                CodexName = "$Codex_Ent_Aleoids_05_Name;",
                Name = "Aleoida Gravis",
                Value = 12_934_900,
                Distance = 150
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AleoidaRule.Gravis,
                GenusId = (int)AleoidaGenus.Gravis,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 190.0,
                MaxTemperature = 197.0,
                MinPressure = 0.054
            });

        SeedHelpers.BodyClasses(modelBuilder, AleoidaRule.Gravis, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.Atmospheres(modelBuilder, AleoidaRule.Gravis, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Volcanisms(modelBuilder, AleoidaRule.Gravis, VolcanismEnum.None);
    }
}