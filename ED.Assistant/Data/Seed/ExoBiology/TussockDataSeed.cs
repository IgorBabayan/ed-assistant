namespace ED.Assistant.Data.Seed.ExoBiology;

static class TussockDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedPennata(modelBuilder);
        SeedVentusa(modelBuilder);
        SeedIgnis(modelBuilder);
        SeedCultro(modelBuilder);
        SeedCatena(modelBuilder);
        SeedPennatis(modelBuilder);
        SeedSerrati(modelBuilder);
        SeedAlbata(modelBuilder);
        SeedPropagito(modelBuilder);
        SeedDivisa(modelBuilder);
        SeedCaputus(modelBuilder);
        SeedTriticum(modelBuilder);
        SeedStigmasis(modelBuilder);
        SeedVirgam(modelBuilder);
        SeedCapillum(modelBuilder);
    }

    private static void SeedCapillum(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Capillum, "$Codex_Ent_Tussocks_15_Name;", "Tussock Capillum", 7_025_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.CapillumArgon,
                GenusId = (int)TussockGenus.Capillum,
                MinGravity = 0.22,
                MaxGravity = 0.276,
                MinTemperature = 80.0,
                MaxTemperature = 129.0
            },
            new Rule
            {
                Id = (int)TussockRule.CapillumMethane,
                GenusId = (int)TussockGenus.Capillum,
                MinGravity = 0.033,
                MaxGravity = 0.276,
                MinTemperature = 80.0,
                MaxTemperature = 110.0
            });

        SeedAtmospheres(modelBuilder, TussockRule.CapillumArgon, AtmosphereEnum.Argon);
        SeedAtmospheres(modelBuilder, TussockRule.CapillumMethane, AtmosphereEnum.Methane);

        SeedBodyClasses(modelBuilder, TussockRule.CapillumArgon, BodyClassEnum.RockyIceBody);
        SeedBodyClasses(modelBuilder, TussockRule.CapillumMethane, BodyClassEnum.RockyBody, BodyClassEnum.RockyIceBody);
    }

    private static void SeedVirgam(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Virgam, "$Codex_Ent_Tussocks_14_Name;", "Tussock Virgam", 14_313_700);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.VirgamNone,
                GenusId = (int)TussockGenus.Virgam,
                MinGravity = 0.04,
                MaxGravity = 0.065
            },
            new Rule
            {
                Id = (int)TussockRule.VirgamWater,
                GenusId = (int)TussockGenus.Virgam,
                MinGravity = 0.04,
                MaxGravity = 0.065
            });

        SeedAtmospheres(modelBuilder, TussockRule.VirgamNone, AtmosphereEnum.Water);
        SeedAtmospheres(modelBuilder, TussockRule.VirgamWater, AtmosphereEnum.Water);

        SeedBodyClasses(modelBuilder, TussockRule.VirgamNone, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedBodyClasses(modelBuilder, TussockRule.VirgamWater, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(modelBuilder, TussockRule.VirgamNone, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, TussockRule.VirgamWater, VolcanismEnum.Water);
    }

    private static void SeedStigmasis(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Stigmasis, "$Codex_Ent_Tussocks_13_Name;", "Tussock Stigmasis", 19_010_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Stigmasis,
                GenusId = (int)TussockGenus.Stigmasis,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 132.0,
                MaxTemperature = 180.0,
                MaxPressure = 0.01
            });

        SeedAtmospheres(modelBuilder, TussockRule.Stigmasis, AtmosphereEnum.SulphurDioxide);
        SeedBodyClasses(modelBuilder, TussockRule.Stigmasis, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
    }

    private static void SeedTriticum(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Triticum, "$Codex_Ent_Tussocks_12_Name;", "Tussock Triticum", 7_774_700);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Triticum,
                GenusId = (int)TussockGenus.Triticum,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 191.0,
                MaxTemperature = 197.0,
                MinPressure = 0.058
            });

        SeedAtmospheres(modelBuilder, TussockRule.Triticum, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(modelBuilder, TussockRule.Triticum, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, TussockRule.Triticum, VolcanismEnum.None);
    }

    private static void SeedCaputus(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Caputus, "$Codex_Ent_Tussocks_11_Name;", "Tussock Caputus", 3_472_400);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Caputus,
                GenusId = (int)TussockGenus.Caputus,
                MinGravity = 0.041,
                MaxGravity = 0.27,
                MinTemperature = 181.0,
                MaxTemperature = 190.0,
                MinPressure = 0.0275
            });

        SeedAtmospheres(modelBuilder, TussockRule.Caputus, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(modelBuilder, TussockRule.Caputus, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, TussockRule.Caputus, VolcanismEnum.None);
    }

    private static void SeedDivisa(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Divisa, "$Codex_Ent_Tussocks_10_Name;", "Tussock Divisa", 1_766_600);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Divisa,
                GenusId = (int)TussockGenus.Divisa,
                MinGravity = 0.042,
                MaxGravity = 0.276,
                MinTemperature = 152.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            });

        SeedAtmospheres(modelBuilder, TussockRule.Divisa, AtmosphereEnum.Ammonia);
        SeedBodyClasses(modelBuilder, TussockRule.Divisa, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
    }

    private static void SeedPropagito(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Propagito, "$Codex_Ent_Tussocks_09_Name;", "Tussock Propagito", 1_000_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Propagito,
                GenusId = (int)TussockGenus.Propagito,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 145.0,
                MaxTemperature = 197.0,
                MinPressure = 0.00289
            });

        SeedAtmospheres(modelBuilder, TussockRule.Propagito, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(modelBuilder, TussockRule.Propagito, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, TussockRule.Propagito, VolcanismEnum.None);
    }

    private static void SeedAlbata(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Albata, "$Codex_Ent_Tussocks_08_Name;", "Tussock Albata", 3_252_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Albata,
                GenusId = (int)TussockGenus.Albata,
                MinGravity = 0.042,
                MaxGravity = 0.276,
                MinTemperature = 175.0,
                MaxTemperature = 180.0,
                MinPressure = 0.016
            });

        SeedAtmospheres(modelBuilder, TussockRule.Albata, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(modelBuilder, TussockRule.Albata, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, TussockRule.Albata, VolcanismEnum.None);
    }

    private static void SeedSerrati(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Serrati, "$Codex_Ent_Tussocks_07_Name;", "Tussock Serrati", 4_447_100);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Serrati,
                GenusId = (int)TussockGenus.Serrati,
                MinGravity = 0.042,
                MaxGravity = 0.23,
                MinTemperature = 171.0,
                MaxTemperature = 174.0,
                MinPressure = 0.01,
                MaxPressure = 0.071
            });

        SeedAtmospheres(modelBuilder, TussockRule.Serrati, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(modelBuilder, TussockRule.Serrati, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, TussockRule.Serrati, VolcanismEnum.None);
    }

    private static void SeedPennatis(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Pennatis, "$Codex_Ent_Tussocks_06_Name;", "Tussock Pennatis", 1_000_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Pennatis,
                GenusId = (int)TussockGenus.Pennatis,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 147.0,
                MaxTemperature = 197.0,
                MinPressure = 0.00289
            });

        SeedAtmospheres(modelBuilder, TussockRule.Pennatis, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(modelBuilder, TussockRule.Pennatis, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, TussockRule.Pennatis, VolcanismEnum.None);
    }

    private static void SeedCatena(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Catena, "$Codex_Ent_Tussocks_05_Name;", "Tussock Catena", 1_766_600);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Catena,
                GenusId = (int)TussockGenus.Catena,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 152.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            });

        SeedAtmospheres(modelBuilder, TussockRule.Catena, AtmosphereEnum.Ammonia);
        SeedBodyClasses(modelBuilder, TussockRule.Catena, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
    }

    private static void SeedCultro(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Cultro, "$Codex_Ent_Tussocks_04_Name;", "Tussock Cultro", 1_766_600);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Cultro,
                GenusId = (int)TussockGenus.Cultro,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 152.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            });

        SeedAtmospheres(modelBuilder, TussockRule.Cultro, AtmosphereEnum.Ammonia);
        SeedBodyClasses(modelBuilder, TussockRule.Cultro, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
    }

    private static void SeedIgnis(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Ignis, "$Codex_Ent_Tussocks_03_Name;", "Tussock Ignis", 1_849_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Ignis,
                GenusId = (int)TussockGenus.Ignis,
                MinGravity = 0.04,
                MaxGravity = 0.2,
                MinTemperature = 161.0,
                MaxTemperature = 170.0,
                MinPressure = 0.00289
            });

        SeedAtmospheres(modelBuilder, TussockRule.Ignis, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(modelBuilder, TussockRule.Ignis, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, TussockRule.Ignis, VolcanismEnum.None);
    }

    private static void SeedVentusa(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Ventusa, "$Codex_Ent_Tussocks_02_Name;", "Tussock Ventusa", 3_227_700);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Ventusa,
                GenusId = (int)TussockGenus.Ventusa,
                MinGravity = 0.04,
                MaxGravity = 0.13,
                MinTemperature = 155.0,
                MaxTemperature = 160.0,
                MinPressure = 0.00289
            });

        SeedAtmospheres(modelBuilder, TussockRule.Ventusa, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(modelBuilder, TussockRule.Ventusa, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, TussockRule.Ventusa, VolcanismEnum.None);
    }

    private static void SeedPennata(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, TussockGenus.Pennata, "$Codex_Ent_Tussocks_01_Name;", "Tussock Pennata", 5_853_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TussockRule.Pennata,
                GenusId = (int)TussockGenus.Pennata,
                MinGravity = 0.04,
                MaxGravity = 0.09,
                MinTemperature = 146.0,
                MaxTemperature = 154.0,
                MinPressure = 0.00289
            });

        SeedAtmospheres(modelBuilder, TussockRule.Pennata, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(modelBuilder, TussockRule.Pennata, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, TussockRule.Pennata, VolcanismEnum.None);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        TussockGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Tussocks_Genus_Name;",
                CodexName = codexName,
                Name = name,
                Value = value,
                Distance = 200
            });
    }

    private static void SeedAtmospheres(
        ModelBuilder modelBuilder,
        TussockRule rule,
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
        TussockRule rule,
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
        TussockRule rule,
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