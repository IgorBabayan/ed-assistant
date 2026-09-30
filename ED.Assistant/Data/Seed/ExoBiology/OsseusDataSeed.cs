namespace ED.Assistant.Data.Seed.ExoBiology;

internal static class OsseusDataSeed
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

        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.Pellebantus, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.BodyClasses(
            modelBuilder,
            OsseusRule.Pellebantus,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
        SeedHelpers.Volcanisms(modelBuilder, OsseusRule.Pellebantus, VolcanismEnum.None);
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

        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.Cornibus, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.BodyClasses(
            modelBuilder,
            OsseusRule.Cornibus,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
        SeedHelpers.Volcanisms(modelBuilder, OsseusRule.Cornibus, VolcanismEnum.None);
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
        
        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.PumiceArgonNone, AtmosphereEnum.Argon);
        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.PumiceArgonWaterGeysers, AtmosphereEnum.Argon);
        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.PumiceArgonRich, AtmosphereEnum.ArgonRich);
        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.PumiceMethane, AtmosphereEnum.Methane);
        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.PumiceNitrogen, AtmosphereEnum.Nitrogen);

        SeedHelpers.BodyClasses(
            modelBuilder,
            OsseusRule.PumiceArgonNone,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, OsseusRule.PumiceArgonWaterGeysers, BodyClassEnum.RockyIceBody);
        SeedHelpers.BodyClasses(modelBuilder, OsseusRule.PumiceArgonRich, BodyClassEnum.RockyIceBody);
        SeedHelpers.BodyClasses(
            modelBuilder,
            OsseusRule.PumiceMethane,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(
            modelBuilder,
            OsseusRule.PumiceNitrogen,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(modelBuilder, OsseusRule.PumiceArgonNone, VolcanismEnum.None);
        SeedHelpers.Volcanisms(
            modelBuilder,
            OsseusRule.PumiceArgonWaterGeysers,
            VolcanismEnum.Water,
            VolcanismEnum.Geysers);
        SeedHelpers.Volcanisms(modelBuilder, OsseusRule.PumiceArgonRich, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, OsseusRule.PumiceNitrogen, VolcanismEnum.None);

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

        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.Spiralis, AtmosphereEnum.Ammonia);
        SeedHelpers.BodyClasses(
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
        
        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.DiscusAmmonia, AtmosphereEnum.Ammonia);
        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.DiscusArgon, AtmosphereEnum.Argon);
        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.DiscusCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.DiscusMethane, AtmosphereEnum.Methane);
        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.DiscusWater, AtmosphereEnum.Water);

        SeedHelpers.BodyClasses(
            modelBuilder,
            OsseusRule.DiscusAmmonia,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, OsseusRule.DiscusArgon, BodyClassEnum.RockyIceBody);
        SeedHelpers.BodyClasses(modelBuilder, OsseusRule.DiscusCarbonDioxide, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, OsseusRule.DiscusMethane, BodyClassEnum.RockyBody);
        SeedHelpers.BodyClasses(
            modelBuilder,
            OsseusRule.DiscusWater,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(modelBuilder, OsseusRule.DiscusAmmonia, VolcanismEnum.Any);
        SeedHelpers.Volcanisms(modelBuilder, OsseusRule.DiscusArgon, VolcanismEnum.Any);
        SeedHelpers.Volcanisms(modelBuilder, OsseusRule.DiscusCarbonDioxide, VolcanismEnum.Any);
        SeedHelpers.Volcanisms(modelBuilder, OsseusRule.DiscusMethane, VolcanismEnum.Any);
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

        SeedHelpers.Atmospheres(modelBuilder, OsseusRule.Fractus, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.BodyClasses(
            modelBuilder,
            OsseusRule.Fractus,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);
        SeedHelpers.Volcanisms(modelBuilder, OsseusRule.Fractus, VolcanismEnum.None);
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
}