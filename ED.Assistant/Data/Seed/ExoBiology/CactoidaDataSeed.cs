namespace ED.Assistant.Data.Seed.ExoBiology;

static class CactoidaDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedCortexum(modelBuilder);
        SeedLapis(modelBuilder);
        SeedVermis(modelBuilder);
        SeedPullulanta(modelBuilder);
        SeedPeperatis(modelBuilder);
    }

    private static void SeedPeperatis(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            CactoidaGenus.Peperatis,
            "$Codex_Ent_Cactoid_05_Name;",
            "Cactoida Peperatis",
            2_483_600);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)CactoidaRule.Peperatis,
                GenusId = (int)CactoidaGenus.Peperatis,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 160.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            });

        SeedAtmospheres(
            modelBuilder,
            CactoidaRule.Peperatis,
            AtmosphereEnum.Ammonia);

        SeedBodyClasses(
            modelBuilder,
            CactoidaRule.Peperatis,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
    }

    private static void SeedPullulanta(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            CactoidaGenus.Pullulanta,
            "$Codex_Ent_Cactoid_04_Name;",
            "Cactoida Pullulanta",
            3_667_600);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)CactoidaRule.Pullulanta,
                GenusId = (int)CactoidaGenus.Pullulanta,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 180.0,
                MaxTemperature = 197.0,
                MinPressure = 0.025
            });

        SeedAtmospheres(
            modelBuilder,
            CactoidaRule.Pullulanta,
            AtmosphereEnum.CarbonDioxide);

        SeedBodyClasses(
            modelBuilder,
            CactoidaRule.Pullulanta,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(
            modelBuilder,
            CactoidaRule.Pullulanta,
            VolcanismEnum.None);
    }

    private static void SeedVermis(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            CactoidaGenus.Vermis,
            "$Codex_Ent_Cactoid_03_Name;",
            "Cactoida Vermis",
            16_202_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)CactoidaRule.VermisSulphurDioxide,
                GenusId = (int)CactoidaGenus.Vermis,
                MinGravity = 0.265,
                MaxGravity = 0.276,
                MinTemperature = 160.0,
                MaxTemperature = 210.0,
                MaxPressure = 0.005
            },
            new Rule
            {
                Id = (int)CactoidaRule.VermisWaterNone,
                GenusId = (int)CactoidaGenus.Vermis,
                MinGravity = 0.04,
                MaxGravity = 0.276
            },
            new Rule
            {
                Id = (int)CactoidaRule.VermisWaterVolcanism,
                GenusId = (int)CactoidaGenus.Vermis,
                MinGravity = 0.04,
                MaxGravity = 0.276
            });

        SeedAtmospheres(
            modelBuilder,
            CactoidaRule.VermisSulphurDioxide,
            AtmosphereEnum.SulphurDioxide);

        SeedAtmospheres(
            modelBuilder,
            CactoidaRule.VermisWaterNone,
            AtmosphereEnum.Water);

        SeedAtmospheres(
            modelBuilder,
            CactoidaRule.VermisWaterVolcanism,
            AtmosphereEnum.Water);

        SeedBodyClasses(
            modelBuilder,
            CactoidaRule.VermisSulphurDioxide,
            BodyClassEnum.RockyBody);

        SeedBodyClasses(
            modelBuilder,
            CactoidaRule.VermisWaterNone,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedBodyClasses(
            modelBuilder,
            CactoidaRule.VermisWaterVolcanism,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(
            modelBuilder,
            CactoidaRule.VermisSulphurDioxide,
            VolcanismEnum.None);

        SeedVolcanisms(
            modelBuilder,
            CactoidaRule.VermisWaterNone,
            VolcanismEnum.None);

        SeedVolcanisms(
            modelBuilder,
            CactoidaRule.VermisWaterVolcanism,
            VolcanismEnum.Water);
    }

    private static void SeedLapis(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            CactoidaGenus.Lapis,
            "$Codex_Ent_Cactoid_02_Name;",
            "Cactoida Lapis",
            2_483_600);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)CactoidaRule.Lapis,
                GenusId = (int)CactoidaGenus.Lapis,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 160.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            });

        SeedAtmospheres(
            modelBuilder,
            CactoidaRule.Lapis,
            AtmosphereEnum.Ammonia);

        SeedBodyClasses(
            modelBuilder,
            CactoidaRule.Lapis,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
    }

    private static void SeedCortexum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            CactoidaGenus.Cortexum,
            "$Codex_Ent_Cactoid_01_Name;",
            "Cactoida Cortexum",
            3_667_600);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)CactoidaRule.Cortexum,
                GenusId = (int)CactoidaGenus.Cortexum,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 180.0,
                MaxTemperature = 197.0,
                MinPressure = 0.025
            });

        SeedAtmospheres(
            modelBuilder,
            CactoidaRule.Cortexum,
            AtmosphereEnum.CarbonDioxide);

        SeedBodyClasses(
            modelBuilder,
            CactoidaRule.Cortexum,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(
            modelBuilder,
            CactoidaRule.Cortexum,
            VolcanismEnum.None);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        CactoidaGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Cactoid_Genus_Name;",
                CodexName = codexName,
                Name = name,
                Value = value
            });
    }

    private static void SeedAtmospheres(
        ModelBuilder modelBuilder,
        CactoidaRule rule,
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
        CactoidaRule rule,
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
        CactoidaRule rule,
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