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

        SeedBodyClasses(modelBuilder, AleoidaRule.Arcus);
        SeedAtmosphere(modelBuilder, AleoidaRule.Arcus, AtmosphereEnum.CarbonDioxide);
        SeedVolcanism(modelBuilder, AleoidaRule.Arcus, VolcanismEnum.None);
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

        SeedBodyClasses(modelBuilder, AleoidaRule.Coronamus);
        SeedAtmosphere(modelBuilder, AleoidaRule.Coronamus, AtmosphereEnum.CarbonDioxide);
        SeedVolcanism(modelBuilder, AleoidaRule.Coronamus, VolcanismEnum.None);
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

        SeedBodyClasses(modelBuilder, AleoidaRule.Spica);
        SeedAtmosphere(modelBuilder, AleoidaRule.Spica, AtmosphereEnum.Ammonia);
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

        SeedBodyClasses(modelBuilder, AleoidaRule.Laminiae);
        SeedAtmosphere(modelBuilder, AleoidaRule.Laminiae, AtmosphereEnum.Ammonia);
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

        SeedBodyClasses(modelBuilder, AleoidaRule.Gravis);
        SeedAtmosphere(modelBuilder, AleoidaRule.Gravis, AtmosphereEnum.CarbonDioxide);
        SeedVolcanism(modelBuilder, AleoidaRule.Gravis, VolcanismEnum.None);
    }

    private static void SeedBodyClasses(ModelBuilder modelBuilder, AleoidaRule rule)
    {
        modelBuilder.Entity("RuleBodyClass").HasData(
            new
            {
                RuleId = (int)rule,
                BodyClassId = (int)BodyClassEnum.RockyBody
            },
            new
            {
                RuleId = (int)rule,
                BodyClassId = (int)BodyClassEnum.HighMetalContentBody
            });
    }

    private static void SeedAtmosphere(ModelBuilder modelBuilder, AleoidaRule rule, AtmosphereEnum atmosphere) =>
        modelBuilder.Entity("RuleAtmosphere").HasData(
            new
            {
                RuleId = (int)rule,
                AtmosphereId = (int)atmosphere
            });

    private static void SeedVolcanism(ModelBuilder modelBuilder, AleoidaRule rule, VolcanismEnum volcanism) =>
        modelBuilder.Entity("RuleVolcanism").HasData(
            new
            {
                RuleId = (int)rule,
                VolcanismId = (int)volcanism
            });
}