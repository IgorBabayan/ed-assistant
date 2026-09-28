namespace ED.Assistant.Data.Seed.ExoBiology;

static class OsseusDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedFractus(modelBuilder);
        SeedDiscus(modelBuilder);
        SeedSpiralis(modelBuilder);
        SeedPumice(modelBuilder);
        SeedCornibus(modelBuilder);
        SeedPellebantus(modelBuilder);
    }

    private static void SeedPellebantus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            OsseusGenus.Pellebantus,
            "$Codex_Ent_Osseus_06_Name;",
            "Osseus Pellebantus",
            9_739_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)OsseusRule.Pellebantus,
                GenusId = (int)OsseusGenus.Pellebantus,
                MinGravity = 0.0405,
                MaxGravity = 0.276,
                MinTemperature = 191.0,
                MinPressure = 0.057
            });

        SeedAtmospheres(modelBuilder, OsseusRule.Pellebantus, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(
            modelBuilder,
            OsseusRule.Pellebantus,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, OsseusRule.Pellebantus, VolcanismEnum.None);
    }

    private static void SeedCornibus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            OsseusGenus.Cornibus,
            "$Codex_Ent_Osseus_05_Name;",
            "Osseus Cornibus",
            1_483_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)OsseusRule.Cornibus,
                GenusId = (int)OsseusGenus.Cornibus,
                MinGravity = 0.0405,
                MaxGravity = 0.276,
                MinTemperature = 180.0,
                MinPressure = 0.025
            });

        SeedAtmospheres(modelBuilder, OsseusRule.Cornibus, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(
            modelBuilder,
            OsseusRule.Cornibus,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, OsseusRule.Cornibus, VolcanismEnum.None);
    }

    private static void SeedPumice(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            OsseusGenus.Pumice,
            "$Codex_Ent_Osseus_04_Name;",
            "Osseus Pumice",
            3_156_300);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)OsseusRule.PumiceArgonNone,
                GenusId = (int)OsseusGenus.Pumice,
                MinGravity = 0.059,
                MaxGravity = 0.276,
                MinTemperature = 50.0,
                MaxTemperature = 135.0
            },
            new Rule
            {
                Id = (int)OsseusRule.PumiceArgonWaterGeysers,
                GenusId = (int)OsseusGenus.Pumice,
                MinGravity = 0.059,
                MaxGravity = 0.276,
                MinTemperature = 50.0,
                MaxTemperature = 135.0
            },
            new Rule
            {
                Id = (int)OsseusRule.PumiceArgonRich,
                GenusId = (int)OsseusGenus.Pumice,
                MinGravity = 0.035,
                MaxGravity = 0.276,
                MinTemperature = 60.0,
                MaxTemperature = 80.5,
                MinPressure = 0.03
            },
            new Rule
            {
                Id = (int)OsseusRule.PumiceMethane,
                GenusId = (int)OsseusGenus.Pumice,
                MinGravity = 0.033,
                MaxGravity = 0.276,
                MinTemperature = 67.0,
                MaxTemperature = 109.0
            },
            new Rule
            {
                Id = (int)OsseusRule.PumiceNitrogen,
                GenusId = (int)OsseusGenus.Pumice,
                MinGravity = 0.05,
                MaxGravity = 0.276,
                MinTemperature = 42.0,
                MaxTemperature = 70.1
            });
        
        SeedAtmospheres(modelBuilder, OsseusRule.PumiceArgonNone, AtmosphereEnum.Argon);
        SeedAtmospheres(modelBuilder, OsseusRule.PumiceArgonWaterGeysers, AtmosphereEnum.Argon);
        SeedAtmospheres(modelBuilder, OsseusRule.PumiceArgonRich, AtmosphereEnum.ArgonRich);
        SeedAtmospheres(modelBuilder, OsseusRule.PumiceMethane, AtmosphereEnum.Methane);
        SeedAtmospheres(modelBuilder, OsseusRule.PumiceNitrogen, AtmosphereEnum.Nitrogen);

        SeedBodyClasses(
            modelBuilder,
            OsseusRule.PumiceArgonNone,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);
        SeedBodyClasses(modelBuilder, OsseusRule.PumiceArgonWaterGeysers, BodyClassEnum.RockyIceBody);
        SeedBodyClasses(modelBuilder, OsseusRule.PumiceArgonRich, BodyClassEnum.RockyIceBody);
        SeedBodyClasses(
            modelBuilder,
            OsseusRule.PumiceMethane,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);
        SeedBodyClasses(
            modelBuilder,
            OsseusRule.PumiceNitrogen,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(modelBuilder, OsseusRule.PumiceArgonNone, VolcanismEnum.None);
        SeedVolcanisms(
            modelBuilder,
            OsseusRule.PumiceArgonWaterGeysers,
            VolcanismEnum.Water,
            VolcanismEnum.Geysers);
        SeedVolcanisms(modelBuilder, OsseusRule.PumiceArgonRich, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, OsseusRule.PumiceNitrogen, VolcanismEnum.None);

    }

    private static void SeedSpiralis(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            OsseusGenus.Spiralis,
            "$Codex_Ent_Osseus_03_Name;",
            "Osseus Spiralis",
            2_404_700);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)OsseusRule.Spiralis,
                GenusId = (int)OsseusGenus.Spiralis,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 160.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            });

        SeedAtmospheres(modelBuilder, OsseusRule.Spiralis, AtmosphereEnum.Ammonia);
        SeedBodyClasses(
            modelBuilder,
            OsseusRule.Spiralis,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);
    }

    private static void SeedDiscus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            OsseusGenus.Discus,
            "$Codex_Ent_Osseus_02_Name;",
            "Osseus Discus",
            12_934_900);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)OsseusRule.DiscusAmmonia,
                GenusId = (int)OsseusGenus.Discus,
                MinGravity = 0.04,
                MaxGravity = 0.088,
                MinTemperature = 161.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            },
            new Rule
            {
                Id = (int)OsseusRule.DiscusArgon,
                GenusId = (int)OsseusGenus.Discus,
                MinGravity = 0.2,
                MaxGravity = 0.276,
                MinTemperature = 65.0,
                MaxTemperature = 120.0
            },
            new Rule
            {
                Id = (int)OsseusRule.DiscusCarbonDioxide,
                GenusId = (int)OsseusGenus.Discus,
                MinGravity = 0.026,
                MaxGravity = 0.276,
                MinTemperature = 500.0
            },
            new Rule
            {
                Id = (int)OsseusRule.DiscusMethane,
                GenusId = (int)OsseusGenus.Discus,
                MinGravity = 0.04,
                MaxGravity = 0.127,
                MinTemperature = 80.0,
                MaxTemperature = 110.0,
                MinPressure = 0.012
            },
            new Rule
            {
                Id = (int)OsseusRule.DiscusWater,
                GenusId = (int)OsseusGenus.Discus,
                MinGravity = 0.04,
                MaxGravity = 0.055
            });
        
        SeedAtmospheres(modelBuilder, OsseusRule.DiscusAmmonia, AtmosphereEnum.Ammonia);
        SeedAtmospheres(modelBuilder, OsseusRule.DiscusArgon, AtmosphereEnum.Argon);
        SeedAtmospheres(modelBuilder, OsseusRule.DiscusCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedAtmospheres(modelBuilder, OsseusRule.DiscusMethane, AtmosphereEnum.Methane);
        SeedAtmospheres(modelBuilder, OsseusRule.DiscusWater, AtmosphereEnum.Water);

        SeedBodyClasses(
            modelBuilder,
            OsseusRule.DiscusAmmonia,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);
        SeedBodyClasses(modelBuilder, OsseusRule.DiscusArgon, BodyClassEnum.RockyIceBody);
        SeedBodyClasses(modelBuilder, OsseusRule.DiscusCarbonDioxide, BodyClassEnum.HighMetalContentBody);
        SeedBodyClasses(modelBuilder, OsseusRule.DiscusMethane, BodyClassEnum.RockyBody);
        SeedBodyClasses(
            modelBuilder,
            OsseusRule.DiscusWater,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(modelBuilder, OsseusRule.DiscusAmmonia, VolcanismEnum.Any);
        SeedVolcanisms(modelBuilder, OsseusRule.DiscusArgon, VolcanismEnum.Any);
        SeedVolcanisms(modelBuilder, OsseusRule.DiscusCarbonDioxide, VolcanismEnum.Any);
        SeedVolcanisms(modelBuilder, OsseusRule.DiscusMethane, VolcanismEnum.Any);
    }

    private static void SeedFractus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            OsseusGenus.Fractus,
            "$Codex_Ent_Osseus_01_Name;",
            "Osseus Fractus",
            4_027_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)OsseusRule.Fractus,
                GenusId = (int)OsseusGenus.Fractus,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 180.0,
                MinPressure = 0.025
            });

        SeedAtmospheres(modelBuilder, OsseusRule.Fractus, AtmosphereEnum.CarbonDioxide);
        SeedBodyClasses(
            modelBuilder,
            OsseusRule.Fractus,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, OsseusRule.Fractus, VolcanismEnum.None);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        OsseusGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Osseus_Genus_Name;",
                CodexName = codexName,
                Name = name,
                Value = value,
                Distance = 800
            });
    }

    private static void SeedAtmospheres(
        ModelBuilder modelBuilder,
        OsseusRule rule,
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
        OsseusRule rule,
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
        OsseusRule rule,
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