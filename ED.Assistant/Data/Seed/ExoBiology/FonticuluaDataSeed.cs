namespace ED.Assistant.Data.Seed.ExoBiology;

static class FonticuluaDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedSegmentatus(modelBuilder);
        SeedCampestris(modelBuilder);
        SeedUpupam(modelBuilder);
        SeedLapida(modelBuilder);
        SeedFluctus(modelBuilder);
        SeedDigitos(modelBuilder);
    }

    private static void SeedDigitos(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            FonticuluaGenus.Digitos,
            "$Codex_Ent_Fonticulus_06_Name;",
            "Fonticulua Digitos",
            1_804_100);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FonticuluaRule.Digitos,
                GenusId = (int)FonticuluaGenus.Digitos,
                MinGravity = 0.025,
                MaxGravity = 0.07,
                MinTemperature = 83.0,
                MaxTemperature = 109.0,
                MinPressure = 0.03
            });

        SeedAtmospheres(
            modelBuilder,
            FonticuluaRule.Digitos,
            AtmosphereEnum.Methane);

        SeedBodyClasses(
            modelBuilder,
            FonticuluaRule.Digitos,
            BodyClassEnum.IcyBody,
            BodyClassEnum.RockyIceBody);
    }

    private static void SeedFluctus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            FonticuluaGenus.Fluctus,
            "$Codex_Ent_Fonticulus_05_Name;",
            "Fonticulua Fluctus",
            20_000_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FonticuluaRule.Fluctus,
                GenusId = (int)FonticuluaGenus.Fluctus,
                MinGravity = 0.235,
                MaxGravity = 0.276,
                MinTemperature = 143.0,
                MaxTemperature = 200.0,
                MinPressure = 0.012
            });

        SeedAtmospheres(
            modelBuilder,
            FonticuluaRule.Fluctus,
            AtmosphereEnum.Oxygen);

        SeedBodyClasses(
            modelBuilder,
            FonticuluaRule.Fluctus,
            BodyClassEnum.IcyBody);
    }

    private static void SeedLapida(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            FonticuluaGenus.Lapida,
            "$Codex_Ent_Fonticulus_04_Name;",
            "Fonticulua Lapida",
            3_111_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FonticuluaRule.Lapida,
                GenusId = (int)FonticuluaGenus.Lapida,
                MinGravity = 0.19,
                MaxGravity = 0.276,
                MinTemperature = 50.0,
                MaxTemperature = 81.0
            });

        SeedAtmospheres(
            modelBuilder,
            FonticuluaRule.Lapida,
            AtmosphereEnum.Nitrogen);

        SeedBodyClasses(
            modelBuilder,
            FonticuluaRule.Lapida,
            BodyClassEnum.IcyBody,
            BodyClassEnum.RockyIceBody);
    }

    private static void SeedUpupam(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            FonticuluaGenus.Upupam,
            "$Codex_Ent_Fonticulus_03_Name;",
            "Fonticulua Upupam",
            5_727_600);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FonticuluaRule.Upupam,
                GenusId = (int)FonticuluaGenus.Upupam,
                MinGravity = 0.209,
                MaxGravity = 0.276,
                MinTemperature = 61.0,
                MaxTemperature = 125.0,
                MinPressure = 0.0175
            });

        SeedAtmospheres(
            modelBuilder,
            FonticuluaRule.Upupam,
            AtmosphereEnum.ArgonRich);

        SeedBodyClasses(
            modelBuilder,
            FonticuluaRule.Upupam,
            BodyClassEnum.IcyBody,
            BodyClassEnum.RockyIceBody);
    }

    private static void SeedCampestris(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            FonticuluaGenus.Campestris,
            "$Codex_Ent_Fonticulus_02_Name;",
            "Fonticulua Campestris",
            1_000_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FonticuluaRule.Campestris,
                GenusId = (int)FonticuluaGenus.Campestris,
                MinGravity = 0.027,
                MaxGravity = 0.276,
                MinTemperature = 50.0,
                MaxTemperature = 150.0
            });

        SeedAtmospheres(
            modelBuilder,
            FonticuluaRule.Campestris,
            AtmosphereEnum.Argon);

        SeedBodyClasses(
            modelBuilder,
            FonticuluaRule.Campestris,
            BodyClassEnum.IcyBody,
            BodyClassEnum.RockyIceBody);
    }

    private static void SeedSegmentatus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            FonticuluaGenus.Segmentatus,
            "$Codex_Ent_Fonticulus_01_Name;",
            "Fonticulua Segmentatus",
            19_010_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FonticuluaRule.Segmentatus,
                GenusId = (int)FonticuluaGenus.Segmentatus,
                MinGravity = 0.25,
                MaxGravity = 0.276,
                MinTemperature = 50.0,
                MaxTemperature = 75.0,
                MaxPressure = 0.006
            });

        SeedAtmospheres(
            modelBuilder,
            FonticuluaRule.Segmentatus,
            AtmosphereEnum.Neon,
            AtmosphereEnum.NeonRich);

        SeedBodyClasses(
            modelBuilder,
            FonticuluaRule.Segmentatus,
            BodyClassEnum.IcyBody);

        SeedVolcanisms(
            modelBuilder,
            FonticuluaRule.Segmentatus,
            VolcanismEnum.None);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        FonticuluaGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Fonticulus_Genus_Name;",
                CodexName = codexName,
                Name = name,
                Value = value,
                Distance = 500
            });
    }

    private static void SeedAtmospheres(
        ModelBuilder modelBuilder,
        FonticuluaRule rule,
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
        FonticuluaRule rule,
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
        FonticuluaRule rule,
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