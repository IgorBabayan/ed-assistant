namespace ED.Assistant.Data.Seed.ExoBiology;

static class StratumDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedExcutitus(modelBuilder);
        SeedPaleas(modelBuilder);
        SeedLaminamus(modelBuilder);
        SeedAraneamus(modelBuilder);
        SeedLimaxus(modelBuilder);
        SeedCucumisis(modelBuilder);
        SeedTectonicas(modelBuilder);
        SeedFrigus(modelBuilder);
    }

    private static void SeedFrigus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            StratumGenus.Frigus,
            "$Codex_Ent_Stratum_08_Name;",
            "Stratum Frigus",
            2_637_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)StratumRule.FrigusCarbonDioxide,
                GenusId = (int)StratumGenus.Frigus,
                MinGravity = 0.043,
                MaxGravity = 0.54,
                MinTemperature = 191.0,
                MaxTemperature = 365.0,
                MinPressure = 0.001
            },
            new Rule
            {
                Id = (int)StratumRule.FrigusCarbonDioxideRich,
                GenusId = (int)StratumGenus.Frigus,
                MinGravity = 0.45,
                MaxGravity = 0.56,
                MinTemperature = 200.0,
                MaxTemperature = 250.0,
                MinPressure = 0.01
            },
            new Rule
            {
                Id = (int)StratumRule.FrigusSulphurDioxide,
                GenusId = (int)StratumGenus.Frigus,
                MinGravity = 0.29,
                MaxGravity = 0.52,
                MinTemperature = 191.0,
                MaxTemperature = 369.0
            });

        SeedHelpers.Atmospheres(modelBuilder, StratumRule.FrigusCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.FrigusCarbonDioxideRich, AtmosphereEnum.CarbonDioxideRich);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.FrigusSulphurDioxide, AtmosphereEnum.SulphurDioxide);

        foreach (var rule in new[]
        {
            StratumRule.FrigusCarbonDioxide,
            StratumRule.FrigusCarbonDioxideRich,
            StratumRule.FrigusSulphurDioxide
        })
        {
            SeedHelpers.BodyClasses(modelBuilder, rule, BodyClassEnum.RockyBody);
        }

        SeedHelpers.Volcanisms(modelBuilder, StratumRule.FrigusCarbonDioxide, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, StratumRule.FrigusCarbonDioxideRich, VolcanismEnum.None);
    }

    private static void SeedTectonicas(ModelBuilder modelBuilder)
    {
                SeedGenus(
            modelBuilder,
            StratumGenus.Tectonicas,
            "$Codex_Ent_Stratum_07_Name;",
            "Stratum Tectonicas",
            19_010_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)StratumRule.TectonicasAmmonia,
                GenusId = (int)StratumGenus.Tectonicas,
                MinGravity = 0.045,
                MaxGravity = 0.38,
                MinTemperature = 165.0,
                MaxTemperature = 177.0
            },
            new Rule
            {
                Id = (int)StratumRule.TectonicasArgon,
                GenusId = (int)StratumGenus.Tectonicas,
                MinGravity = 0.485,
                MaxGravity = 0.54,
                MinTemperature = 167.0,
                MaxTemperature = 199.0
            },
            new Rule
            {
                Id = (int)StratumRule.TectonicasCarbonDioxide,
                GenusId = (int)StratumGenus.Tectonicas,
                MinGravity = 0.045,
                MaxGravity = 0.61,
                MinTemperature = 165.0,
                MaxTemperature = 430.0
            },
            new Rule
            {
                Id = (int)StratumRule.TectonicasCarbonDioxideRich,
                GenusId = (int)StratumGenus.Tectonicas,
                MinGravity = 0.035,
                MaxGravity = 0.61,
                MinTemperature = 165.0,
                MaxTemperature = 260.0
            },
            new Rule
            {
                Id = (int)StratumRule.TectonicasOxygen,
                GenusId = (int)StratumGenus.Tectonicas,
                MinGravity = 0.4,
                MaxGravity = 0.52,
                MinTemperature = 165.0,
                MaxTemperature = 246.0
            },
            new Rule
            {
                Id = (int)StratumRule.TectonicasSulphurDioxide,
                GenusId = (int)StratumGenus.Tectonicas,
                MinGravity = 0.29,
                MaxGravity = 0.62,
                MinTemperature = 165.0,
                MaxTemperature = 450.0
            },
            new Rule
            {
                Id = (int)StratumRule.TectonicasWater,
                GenusId = (int)StratumGenus.Tectonicas,
                MinGravity = 0.045,
                MaxGravity = 0.063
            });
        
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.TectonicasAmmonia, AtmosphereEnum.Ammonia);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.TectonicasArgon, AtmosphereEnum.Argon, AtmosphereEnum.ArgonRich);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.TectonicasCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.TectonicasCarbonDioxideRich, AtmosphereEnum.CarbonDioxideRich);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.TectonicasOxygen, AtmosphereEnum.Oxygen);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.TectonicasSulphurDioxide, AtmosphereEnum.SulphurDioxide);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.TectonicasWater, AtmosphereEnum.Water);

        foreach (var rule in new[]
                 {
                     StratumRule.TectonicasAmmonia,
                     StratumRule.TectonicasArgon,
                     StratumRule.TectonicasCarbonDioxide,
                     StratumRule.TectonicasCarbonDioxideRich,
                     StratumRule.TectonicasOxygen,
                     StratumRule.TectonicasSulphurDioxide,
                     StratumRule.TectonicasWater
                 })
        {
            SeedHelpers.BodyClasses(modelBuilder, rule, BodyClassEnum.HighMetalContentBody);
        }

        SeedHelpers.Volcanisms(modelBuilder, StratumRule.TectonicasArgon, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, StratumRule.TectonicasWater, VolcanismEnum.None);
    }

    private static void SeedCucumisis(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            StratumGenus.Cucumisis,
            "$Codex_Ent_Stratum_06_Name;",
            "Stratum Cucumisis",
            16_202_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)StratumRule.CucumisisCarbonDioxide,
                GenusId = (int)StratumGenus.Cucumisis,
                MinGravity = 0.04,
                MaxGravity = 0.6,
                MinTemperature = 191.0,
                MaxTemperature = 371.0
            },
            new Rule
            {
                Id = (int)StratumRule.CucumisisCarbonDioxideRich,
                GenusId = (int)StratumGenus.Cucumisis,
                MinGravity = 0.44,
                MaxGravity = 0.56,
                MinTemperature = 210.0,
                MaxTemperature = 246.0,
                MinPressure = 0.01
            },
            new Rule
            {
                Id = (int)StratumRule.CucumisisOxygen,
                GenusId = (int)StratumGenus.Cucumisis,
                MinGravity = 0.4,
                MaxGravity = 0.6,
                MinTemperature = 200.0,
                MaxTemperature = 250.0,
                MinPressure = 0.01
            },
            new Rule
            {
                Id = (int)StratumRule.CucumisisSulphurDioxide,
                GenusId = (int)StratumGenus.Cucumisis,
                MinGravity = 0.26,
                MaxGravity = 0.55,
                MinTemperature = 191.0,
                MaxTemperature = 373.0
            });

        SeedHelpers.Atmospheres(modelBuilder, StratumRule.CucumisisCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.CucumisisCarbonDioxideRich, AtmosphereEnum.CarbonDioxideRich);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.CucumisisOxygen, AtmosphereEnum.Oxygen);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.CucumisisSulphurDioxide, AtmosphereEnum.SulphurDioxide);

        foreach (var rule in new[]
                 {
                     StratumRule.CucumisisCarbonDioxide,
                     StratumRule.CucumisisCarbonDioxideRich,
                     StratumRule.CucumisisOxygen,
                     StratumRule.CucumisisSulphurDioxide
                 })
        {
            SeedHelpers.BodyClasses(modelBuilder, rule, BodyClassEnum.RockyBody);
        }

        SeedHelpers.Volcanisms(modelBuilder, StratumRule.CucumisisCarbonDioxide, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, StratumRule.CucumisisCarbonDioxideRich, VolcanismEnum.None);
    }

    private static void SeedLimaxus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            StratumGenus.Limaxus,
            "$Codex_Ent_Stratum_05_Name;",
            "Stratum Limaxus",
            1_362_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)StratumRule.LimaxusCarbonDioxide,
                GenusId = (int)StratumGenus.Limaxus,
                MinGravity = 0.03,
                MaxGravity = 0.4,
                MinTemperature = 165.0,
                MaxTemperature = 190.0,
                MinPressure = 0.05
            },
            new Rule
            {
                Id = (int)StratumRule.LimaxusSulphurDioxide,
                GenusId = (int)StratumGenus.Limaxus,
                MinGravity = 0.27,
                MaxGravity = 0.4,
                MinTemperature = 165.0,
                MaxTemperature = 190.0
            });

        SeedHelpers.Atmospheres(modelBuilder, StratumRule.LimaxusCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.LimaxusSulphurDioxide, AtmosphereEnum.SulphurDioxide);

        SeedHelpers.BodyClasses(modelBuilder, StratumRule.LimaxusCarbonDioxide, BodyClassEnum.RockyBody);
        SeedHelpers.BodyClasses(modelBuilder, StratumRule.LimaxusSulphurDioxide, BodyClassEnum.RockyBody);

        SeedHelpers.Volcanisms(modelBuilder, StratumRule.LimaxusCarbonDioxide, VolcanismEnum.None);
    }

    private static void SeedAraneamus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            StratumGenus.Araneamus,
            "$Codex_Ent_Stratum_04_Name;",
            "Stratum Araneamus",
            2_448_900);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)StratumRule.Araneamus,
                GenusId = (int)StratumGenus.Araneamus,
                MinGravity = 0.26,
                MaxGravity = 0.57,
                MinTemperature = 165.0,
                MaxTemperature = 373.0
            });

        SeedHelpers.Atmospheres(modelBuilder, StratumRule.Araneamus, AtmosphereEnum.SulphurDioxide);
        SeedHelpers.BodyClasses(modelBuilder, StratumRule.Araneamus, BodyClassEnum.RockyBody);
    }

    private static void SeedLaminamus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            StratumGenus.Laminamus,
            "$Codex_Ent_Stratum_03_Name;",
            "Stratum Laminamus",
            2_788_300);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)StratumRule.Laminamus,
                GenusId = (int)StratumGenus.Laminamus,
                MinGravity = 0.04,
                MaxGravity = 0.34,
                MinTemperature = 165.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            });

        SeedHelpers.Atmospheres(modelBuilder, StratumRule.Laminamus, AtmosphereEnum.Ammonia);
        SeedHelpers.BodyClasses(modelBuilder, StratumRule.Laminamus, BodyClassEnum.RockyBody);
    }

    private static void SeedPaleas(ModelBuilder modelBuilder)
    {
                SeedGenus(
            modelBuilder,
            StratumGenus.Paleas,
            "$Codex_Ent_Stratum_02_Name;",
            "Stratum Paleas",
            1_362_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)StratumRule.PaleasAmmonia,
                GenusId = (int)StratumGenus.Paleas,
                MinGravity = 0.04,
                MaxGravity = 0.35,
                MinTemperature = 165.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            },
            new Rule
            {
                Id = (int)StratumRule.PaleasCarbonDioxide,
                GenusId = (int)StratumGenus.Paleas,
                MinGravity = 0.04,
                MaxGravity = 0.585,
                MinTemperature = 165.0,
                MaxTemperature = 395.0
            },
            new Rule
            {
                Id = (int)StratumRule.PaleasCarbonDioxideRich,
                GenusId = (int)StratumGenus.Paleas,
                MinGravity = 0.43,
                MaxGravity = 0.585,
                MinTemperature = 185.0,
                MaxTemperature = 260.0,
                MinPressure = 0.015
            },
            new Rule
            {
                Id = (int)StratumRule.PaleasWaterNone,
                GenusId = (int)StratumGenus.Paleas,
                MinGravity = 0.04,
                MaxGravity = 0.056
            },
            new Rule
            {
                Id = (int)StratumRule.PaleasWaterVolcanism,
                GenusId = (int)StratumGenus.Paleas,
                MinGravity = 0.04,
                MaxGravity = 0.056,
                MinPressure = 0.065
            },
            new Rule
            {
                Id = (int)StratumRule.PaleasOxygen,
                GenusId = (int)StratumGenus.Paleas,
                MinGravity = 0.39,
                MaxGravity = 0.59,
                MinTemperature = 165.0,
                MaxTemperature = 250.0,
                MinPressure = 0.022
            });
        
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.PaleasAmmonia, AtmosphereEnum.Ammonia);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.PaleasCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.PaleasCarbonDioxideRich, AtmosphereEnum.CarbonDioxideRich);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.PaleasWaterNone, AtmosphereEnum.Water);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.PaleasWaterVolcanism, AtmosphereEnum.Water);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.PaleasOxygen, AtmosphereEnum.Oxygen);

        foreach (var rule in new[]
                 {
                     StratumRule.PaleasAmmonia,
                     StratumRule.PaleasCarbonDioxide,
                     StratumRule.PaleasCarbonDioxideRich,
                     StratumRule.PaleasWaterNone,
                     StratumRule.PaleasWaterVolcanism,
                     StratumRule.PaleasOxygen
                 })
        {
            SeedHelpers.BodyClasses(modelBuilder, rule, BodyClassEnum.RockyBody);
        }

        SeedHelpers.Volcanisms(modelBuilder, StratumRule.PaleasCarbonDioxide, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, StratumRule.PaleasCarbonDioxideRich, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, StratumRule.PaleasWaterNone, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, StratumRule.PaleasWaterVolcanism, VolcanismEnum.Water);
    }

    private static void SeedExcutitus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            StratumGenus.Excutitus,
            "$Codex_Ent_Stratum_01_Name;",
            "Stratum Excutitus",
            2_448_900);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)StratumRule.ExcutitusCarbonDioxide,
                GenusId = (int)StratumGenus.Excutitus,
                MinGravity = 0.04,
                MaxGravity = 0.48,
                MinTemperature = 165.0,
                MaxTemperature = 190.0,
                MinPressure = 0.0035
            },
            new Rule
            {
                Id = (int)StratumRule.ExcutitusSulphurDioxide,
                GenusId = (int)StratumGenus.Excutitus,
                MinGravity = 0.27,
                MaxGravity = 0.4,
                MinTemperature = 165.0,
                MaxTemperature = 190.0
            });

        SeedHelpers.Atmospheres(modelBuilder, StratumRule.ExcutitusCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, StratumRule.ExcutitusSulphurDioxide, AtmosphereEnum.SulphurDioxide);

        SeedHelpers.BodyClasses(modelBuilder, StratumRule.ExcutitusCarbonDioxide, BodyClassEnum.RockyBody);
        SeedHelpers.BodyClasses(modelBuilder, StratumRule.ExcutitusSulphurDioxide, BodyClassEnum.RockyBody);

        SeedHelpers.Volcanisms(modelBuilder, StratumRule.ExcutitusCarbonDioxide, VolcanismEnum.None);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        StratumGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Stratum_Genus_Name;",
                CodexName = codexName,
                Name = name,
                Value = value,
                Distance = 500
            });
    }
}