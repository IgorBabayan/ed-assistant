namespace ED.Assistant.Data.Seed.ExoBiology;

internal static class BacteriumDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedAurasus(modelBuilder);
        SeedNebulus(modelBuilder);
        SeedScopulum(modelBuilder);
        SeedAcies(modelBuilder);
        SeedVesicula(modelBuilder);
        SeedAlcyoneum(modelBuilder);
        SeedTela(modelBuilder);
        SeedInformem(modelBuilder);
        SeedVolu(modelBuilder);
        SeedBullaris(modelBuilder);
        SeedOmentum(modelBuilder);
        SeedCerbrus(modelBuilder);
        SeedVerrata(modelBuilder);
    }

    private static void SeedVerrata(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Verrata, "$Codex_Ent_Bacterial_13_Name;", "Bacterium Verrata", 3_897_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule { Id = (int)BacteriumRule.VerrataAmmonia, GenusId = (int)BacteriumGenus.Verrata, MinGravity = 0.03, MaxGravity = 0.09, MinTemperature = 160.0, MaxTemperature = 180.0, MaxPressure = 0.0135 },
            new Rule { Id = (int)BacteriumRule.VerrataArgon, GenusId = (int)BacteriumGenus.Verrata, MinGravity = 0.165, MaxGravity = 0.33, MinTemperature = 57.5, MaxTemperature = 145.0 },
            new Rule { Id = (int)BacteriumRule.VerrataArgonRich, GenusId = (int)BacteriumGenus.Verrata, MinGravity = 0.04, MaxGravity = 0.08, MinTemperature = 80.0, MaxTemperature = 90.0, MaxPressure = 0.01 },
            new Rule { Id = (int)BacteriumRule.VerrataCarbonDioxide, GenusId = (int)BacteriumGenus.Verrata, MinGravity = 0.25, MaxGravity = 0.32, MinTemperature = 167.0, MaxTemperature = 240.0 },
            new Rule { Id = (int)BacteriumRule.VerrataHelium, GenusId = (int)BacteriumGenus.Verrata, MinGravity = 0.49, MaxGravity = 0.53, MinTemperature = 20.0, MaxTemperature = 21.0, MinPressure = 0.065 },
            new Rule { Id = (int)BacteriumRule.VerrataNeon, GenusId = (int)BacteriumGenus.Verrata, MinGravity = 0.29, MaxGravity = 0.61, MinTemperature = 20.0, MaxTemperature = 51.0, MaxPressure = 0.075 },
            new Rule { Id = (int)BacteriumRule.VerrataNeonRich, GenusId = (int)BacteriumGenus.Verrata, MinGravity = 0.43, MaxGravity = 0.61, MinTemperature = 20.0, MaxTemperature = 65.0, MinPressure = 0.005 },
            new Rule { Id = (int)BacteriumRule.VerrataNitrogen, GenusId = (int)BacteriumGenus.Verrata, MinGravity = 0.205, MaxGravity = 0.241, MinTemperature = 60.0, MaxTemperature = 80.0 },
            new Rule { Id = (int)BacteriumRule.VerrataOxygen, GenusId = (int)BacteriumGenus.Verrata, MinGravity = 0.24, MaxGravity = 0.35, MinTemperature = 154.0, MaxTemperature = 220.0, MinPressure = 0.01 },
            new Rule { Id = (int)BacteriumRule.VerrataWater, GenusId = (int)BacteriumGenus.Verrata, MinGravity = 0.04, MaxGravity = 0.054 }
        );

        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.VerrataAmmonia, AtmosphereEnum.Ammonia);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.VerrataArgon, AtmosphereEnum.Argon);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.VerrataArgonRich, AtmosphereEnum.ArgonRich);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.VerrataCarbonDioxide, AtmosphereEnum.CarbonDioxide, AtmosphereEnum.CarbonDioxideRich);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.VerrataHelium, AtmosphereEnum.Helium);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.VerrataNeon, AtmosphereEnum.Neon);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.VerrataNeonRich, AtmosphereEnum.NeonRich);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.VerrataNitrogen, AtmosphereEnum.Nitrogen);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.VerrataOxygen, AtmosphereEnum.Oxygen);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.VerrataWater, AtmosphereEnum.Water);

        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.VerrataAmmonia, BodyClassEnum.RockyBody, BodyClassEnum.RockyIceBody, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.VerrataArgon, BodyClassEnum.RockyIceBody, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.VerrataArgonRich, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.VerrataCarbonDioxide, BodyClassEnum.RockyIceBody, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.VerrataHelium, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.VerrataNeon, BodyClassEnum.RockyIceBody, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.VerrataNeonRich, BodyClassEnum.RockyIceBody, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.VerrataNitrogen, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.VerrataOxygen, BodyClassEnum.RockyIceBody, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.VerrataWater, BodyClassEnum.RockyBody);

        foreach (var rule in new[]
        {
            BacteriumRule.VerrataAmmonia,
            BacteriumRule.VerrataArgon,
            BacteriumRule.VerrataArgonRich,
            BacteriumRule.VerrataCarbonDioxide,
            BacteriumRule.VerrataHelium,
            BacteriumRule.VerrataNeon,
            BacteriumRule.VerrataNeonRich,
            BacteriumRule.VerrataNitrogen,
            BacteriumRule.VerrataOxygen,
            BacteriumRule.VerrataWater
        })
        {
            SeedHelpers.Volcanisms(modelBuilder, rule, VolcanismEnum.Water);
        }
    }

    private static void SeedCerbrus(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Cerbrus, "$Codex_Ent_Bacterial_12_Name;", "Bacterium Cerbrus", 1_689_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule { Id = (int)BacteriumRule.CerbrusSulphurDioxide, GenusId = (int)BacteriumGenus.Cerbrus, MinGravity = 0.042, MaxGravity = 0.605, MinTemperature = 132.0, MaxTemperature = 500.0 },
            new Rule { Id = (int)BacteriumRule.CerbrusWaterNone, GenusId = (int)BacteriumGenus.Cerbrus, MinGravity = 0.04, MaxGravity = 0.064 },
            new Rule { Id = (int)BacteriumRule.CerbrusWaterVolcanism, GenusId = (int)BacteriumGenus.Cerbrus, MinGravity = 0.04, MaxGravity = 0.064 },
            new Rule { Id = (int)BacteriumRule.CerbrusWaterRich, GenusId = (int)BacteriumGenus.Cerbrus, MinGravity = 0.4, MaxGravity = 0.5, MinTemperature = 190.0, MaxTemperature = 330.0 }
        );

        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.CerbrusSulphurDioxide, AtmosphereEnum.SulphurDioxide);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.CerbrusWaterNone, AtmosphereEnum.Water);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.CerbrusWaterVolcanism, AtmosphereEnum.Water);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.CerbrusWaterRich, AtmosphereEnum.WaterRich);

        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.CerbrusSulphurDioxide, BodyClassEnum.RockyBody, BodyClassEnum.RockyIceBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.CerbrusWaterNone, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.CerbrusWaterVolcanism, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.CerbrusWaterRich, BodyClassEnum.RockyIceBody);

        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.CerbrusWaterNone, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.CerbrusWaterVolcanism, VolcanismEnum.Water);
        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.CerbrusWaterRich, VolcanismEnum.None);
    }

    private static void SeedOmentum(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Omentum, "$Codex_Ent_Bacterial_11_Name;", "Bacterium Omentum", 4_638_900);

        modelBuilder.Entity<Rule>().HasData(
            new Rule { Id = (int)BacteriumRule.OmentumArgon, GenusId = (int)BacteriumGenus.Omentum, MinGravity = 0.045, MaxGravity = 0.45, MinTemperature = 50.0 },
            new Rule { Id = (int)BacteriumRule.OmentumArgonRich, GenusId = (int)BacteriumGenus.Omentum, MinGravity = 0.23, MaxGravity = 0.45, MinTemperature = 80.0, MaxTemperature = 90.0, MinPressure = 0.01 },
            new Rule { Id = (int)BacteriumRule.OmentumHelium, GenusId = (int)BacteriumGenus.Omentum, MinGravity = 0.4, MaxGravity = 0.51, MinTemperature = 20.0, MaxTemperature = 21.0, MinPressure = 0.065 },
            new Rule { Id = (int)BacteriumRule.OmentumMethane, GenusId = (int)BacteriumGenus.Omentum, MinGravity = 0.0265, MaxGravity = 0.0455, MinTemperature = 84.0, MaxTemperature = 108.0, MinPressure = 0.035 },
            new Rule { Id = (int)BacteriumRule.OmentumNeon, GenusId = (int)BacteriumGenus.Omentum, MinGravity = 0.31, MaxGravity = 0.6, MinTemperature = 20.0, MaxTemperature = 61.0, MaxPressure = 0.0065 },
            new Rule { Id = (int)BacteriumRule.OmentumNeonRich, GenusId = (int)BacteriumGenus.Omentum, MinGravity = 0.27, MaxGravity = 0.61, MinTemperature = 20.0, MaxTemperature = 93.0, MinPressure = 0.0027 },
            new Rule { Id = (int)BacteriumRule.OmentumNitrogen, GenusId = (int)BacteriumGenus.Omentum, MinGravity = 0.2, MaxGravity = 0.26, MinTemperature = 60.0, MaxTemperature = 80.0 },
            new Rule { Id = (int)BacteriumRule.OmentumWaterRich, GenusId = (int)BacteriumGenus.Omentum, MinGravity = 0.38, MaxGravity = 0.45, MinTemperature = 190.0, MaxTemperature = 330.0, MinPressure = 0.07 }
        );

        var rules = new[]
        {
            BacteriumRule.OmentumArgon,
            BacteriumRule.OmentumArgonRich,
            BacteriumRule.OmentumHelium,
            BacteriumRule.OmentumMethane,
            BacteriumRule.OmentumNeon,
            BacteriumRule.OmentumNeonRich,
            BacteriumRule.OmentumNitrogen,
            BacteriumRule.OmentumWaterRich
        };

        var atmospheres = new[]
        {
            AtmosphereEnum.Argon,
            AtmosphereEnum.ArgonRich,
            AtmosphereEnum.Helium,
            AtmosphereEnum.Methane,
            AtmosphereEnum.Neon,
            AtmosphereEnum.NeonRich,
            AtmosphereEnum.Nitrogen,
            AtmosphereEnum.WaterRich
        };

        for (var i = 0; i < rules.Length; i++)
        {
            SeedHelpers.Atmospheres(modelBuilder, rules[i], atmospheres[i]);
            SeedHelpers.BodyClasses(modelBuilder, rules[i], BodyClassEnum.IcyBody);
            SeedHelpers.Volcanisms(modelBuilder, rules[i], VolcanismEnum.Nitrogen, VolcanismEnum.Ammonia);
        }
    }

    private static void SeedBullaris(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Bullaris, "$Codex_Ent_Bacterial_10_Name;", "Bacterium Bullaris", 1_152_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule { Id = (int)BacteriumRule.BullarisMethane, GenusId = (int)BacteriumGenus.Bullaris, MinGravity = 0.0245, MaxGravity = 0.35, MinTemperature = 67.0, MaxTemperature = 109.0 },
            new Rule { Id = (int)BacteriumRule.BullarisMethaneRich, GenusId = (int)BacteriumGenus.Bullaris, MinGravity = 0.44, MaxGravity = 0.6, MinTemperature = 74.0, MaxTemperature = 141.0, MinPressure = 0.01, MaxPressure = 0.05 }
        );

        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.BullarisMethane, AtmosphereEnum.Methane);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.BullarisMethaneRich, AtmosphereEnum.MethaneRich);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.BullarisMethaneRich, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.BullarisMethaneRich, VolcanismEnum.None);
    }

    private static void SeedVolu(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Volu, "$Codex_Ent_Bacterial_09_Name;", "Bacterium Volu", 7_774_700);

        modelBuilder.Entity<Rule>().HasData(new Rule
        {
            Id = (int)BacteriumRule.Volu,
            GenusId = (int)BacteriumGenus.Volu,
            MinGravity = 0.239,
            MaxGravity = 0.61,
            MinTemperature = 143.5,
            MaxTemperature = 246.0,
            MinPressure = 0.013
        });

        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.Volu, AtmosphereEnum.Oxygen);
    }

    private static void SeedInformem(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Informem, "$Codex_Ent_Bacterial_08_Name;", "Bacterium Informem", 8_418_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule { Id = (int)BacteriumRule.InformemRocky, GenusId = (int)BacteriumGenus.Informem, MinGravity = 0.05, MaxGravity = 0.6, MinTemperature = 42.5, MaxTemperature = 151.0 },
            new Rule { Id = (int)BacteriumRule.InformemIcy, GenusId = (int)BacteriumGenus.Informem, MinGravity = 0.17, MaxGravity = 0.63, MinTemperature = 50.0, MaxTemperature = 90.0 }
        );

        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.InformemRocky, AtmosphereEnum.Nitrogen);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.InformemIcy, AtmosphereEnum.Nitrogen);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.InformemRocky, BodyClassEnum.RockyBody, BodyClassEnum.RockyIceBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.InformemIcy, BodyClassEnum.IcyBody);
        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.InformemRocky, VolcanismEnum.None);
    }

    private static void SeedTela(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Tela, "$Codex_Ent_Bacterial_07_Name;", "Bacterium Tela", 1_949_000);

        modelBuilder.Entity<Rule>().HasData(
            new Rule { Id = (int)BacteriumRule.TelaArgon, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.045, MaxGravity = 0.45, MinTemperature = 50.0 },
            new Rule { Id = (int)BacteriumRule.TelaArgonRich, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.24, MaxGravity = 0.45, MinTemperature = 50.0, MaxTemperature = 150.0, MaxPressure = 0.05 },
            new Rule { Id = (int)BacteriumRule.TelaAmmonia, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.025, MaxGravity = 0.23, MinTemperature = 165.0, MaxTemperature = 177.0, MinPressure = 0.0025, MaxPressure = 0.02 },
            new Rule { Id = (int)BacteriumRule.TelaCarbonDioxideNone, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.45, MaxGravity = 0.61, MinTemperature = 300.0, MinPressure = 0.006 },
            new Rule { Id = (int)BacteriumRule.TelaCarbonDioxideAny, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.025, MaxGravity = 0.61, MinTemperature = 167.0, MinPressure = 0.006 },
            new Rule { Id = (int)BacteriumRule.TelaHelium, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.025, MaxGravity = 0.61, MinTemperature = 20.0, MaxTemperature = 21.0, MinPressure = 0.067 },
            new Rule { Id = (int)BacteriumRule.TelaMethane, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.026, MaxGravity = 0.126, MinTemperature = 80.0, MaxTemperature = 109.0, MinPressure = 0.012 },
            new Rule { Id = (int)BacteriumRule.TelaNeon, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.27, MaxGravity = 0.61, MinTemperature = 20.0, MaxTemperature = 95.0, MaxPressure = 0.008 },
            new Rule { Id = (int)BacteriumRule.TelaNeonRich, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.27, MaxGravity = 0.61, MinTemperature = 20.0, MaxTemperature = 95.0, MinPressure = 0.003 },
            new Rule { Id = (int)BacteriumRule.TelaNitrogen, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.21, MaxGravity = 0.35, MinTemperature = 55.0, MaxTemperature = 80.0 },
            new Rule { Id = (int)BacteriumRule.TelaOxygen, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.23, MaxGravity = 0.5, MinTemperature = 150.0, MaxTemperature = 240.0, MinPressure = 0.01 },
            new Rule { Id = (int)BacteriumRule.TelaSulphurDioxideAny, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.18, MaxGravity = 0.61, MinTemperature = 148.0, MaxTemperature = 550.0 },
            new Rule { Id = (int)BacteriumRule.TelaSulphurDioxideNone, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.18, MaxGravity = 0.61, MinTemperature = 300.0, MaxTemperature = 550.0 },
            new Rule { Id = (int)BacteriumRule.TelaSulphurDioxideHotThin, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.5, MaxGravity = 0.55, MinTemperature = 500.0, MaxTemperature = 650.0 },
            new Rule { Id = (int)BacteriumRule.TelaWater, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.04, MaxGravity = 0.063 },
            new Rule { Id = (int)BacteriumRule.TelaWaterRich, GenusId = (int)BacteriumGenus.Tela, MinGravity = 0.315, MaxGravity = 0.44, MinTemperature = 190.0, MaxTemperature = 330.0, MinPressure = 0.01 }
        );

        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaArgon, AtmosphereEnum.Argon);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaArgonRich, AtmosphereEnum.ArgonRich);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaAmmonia, AtmosphereEnum.Ammonia);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaCarbonDioxideNone, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaCarbonDioxideAny, AtmosphereEnum.CarbonDioxide, AtmosphereEnum.CarbonDioxideRich);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaHelium, AtmosphereEnum.Helium);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaMethane, AtmosphereEnum.Methane);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaNeon, AtmosphereEnum.Neon);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaNeonRich, AtmosphereEnum.NeonRich);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaNitrogen, AtmosphereEnum.Nitrogen);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaOxygen, AtmosphereEnum.Oxygen);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaSulphurDioxideAny, AtmosphereEnum.SulphurDioxide);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaSulphurDioxideNone, AtmosphereEnum.SulphurDioxide);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaSulphurDioxideHotThin, AtmosphereEnum.SulphurDioxide);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaWater, AtmosphereEnum.Water);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.TelaWaterRich, AtmosphereEnum.WaterRich);

        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.TelaArgon, BodyClassEnum.IcyBody, BodyClassEnum.RockyIceBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.TelaHelium, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.TelaMethane, BodyClassEnum.IcyBody, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.TelaNeon, BodyClassEnum.IcyBody, BodyClassEnum.RockyIceBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.TelaNeonRich, BodyClassEnum.IcyBody, BodyClassEnum.RockyIceBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.TelaSulphurDioxideHotThin, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.TelaWater, BodyClassEnum.RockyBody, BodyClassEnum.HighMetalContentBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.TelaWaterRich, BodyClassEnum.IcyBody, BodyClassEnum.RockyIceBody);

        foreach (var rule in new[]
        {
            BacteriumRule.TelaArgon,
            BacteriumRule.TelaArgonRich,
            BacteriumRule.TelaAmmonia,
            BacteriumRule.TelaCarbonDioxideAny,
            BacteriumRule.TelaHelium,
            BacteriumRule.TelaMethane,
            BacteriumRule.TelaNeon,
            BacteriumRule.TelaNeonRich,
            BacteriumRule.TelaNitrogen,
            BacteriumRule.TelaOxygen,
            BacteriumRule.TelaSulphurDioxideAny,
            BacteriumRule.TelaSulphurDioxideHotThin,
            BacteriumRule.TelaWaterRich
        })
        {
            SeedHelpers.Volcanisms(modelBuilder, rule, VolcanismEnum.Any);
        }

        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.TelaCarbonDioxideNone, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.TelaSulphurDioxideNone, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.TelaWater, VolcanismEnum.None);
    }

    private static void SeedAlcyoneum(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Alcyoneum, "$Codex_Ent_Bacterial_06_Name;", "Bacterium Alcyoneum", 1_658_500);

        modelBuilder.Entity<Rule>().HasData(new Rule
        {
            Id = (int)BacteriumRule.Alcyoneum,
            GenusId = (int)BacteriumGenus.Alcyoneum,
            MinGravity = 0.04,
            MaxGravity = 0.376,
            MinTemperature = 152.0,
            MaxTemperature = 177.0,
            MaxPressure = 0.0135
        });

        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.Alcyoneum, AtmosphereEnum.Ammonia);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.Alcyoneum,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);
    }

    private static void SeedVesicula(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Vesicula, "$Codex_Ent_Bacterial_05_Name;", "Bacterium Vesicula", 1_000_000);

        modelBuilder.Entity<Rule>().HasData(new Rule
        {
            Id = (int)BacteriumRule.Vesicula,
            GenusId = (int)BacteriumGenus.Vesicula,
            MinGravity = 0.027,
            MaxGravity = 0.51,
            MinTemperature = 50.0,
            MaxTemperature = 245.0
        });

        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.Vesicula, AtmosphereEnum.Argon);
    }

    private static void SeedAcies(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Acies, "$Codex_Ent_Bacterial_04_Name;", "Bacterium Acies",
            1_000_000);

        modelBuilder.Entity<Rule>().HasData(new Rule
        {
            Id = (int)BacteriumRule.Acies,
            GenusId = (int)BacteriumGenus.Acies,
            MinGravity = 0.255,
            MaxGravity = 0.61,
            MinTemperature = 20.0,
            MaxTemperature = 61.0,
            MaxPressure = 0.01
        });

        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.Acies, AtmosphereEnum.Neon);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.Acies, BodyClassEnum.IcyBody, BodyClassEnum.RockyIceBody);
    }

    private static void SeedScopulum(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Scopulum, "$Codex_Ent_Bacterial_03_Name;", "Bacterium Scopulum", 4_934_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule { Id = (int)BacteriumRule.ScopulumArgon, GenusId = (int)BacteriumGenus.Scopulum, MinGravity = 0.15, MaxGravity = 0.26, MinTemperature = 56, MaxTemperature = 150 },
            new Rule { Id = (int)BacteriumRule.ScopulumHelium, GenusId = (int)BacteriumGenus.Scopulum, MinGravity = 0.48, MaxGravity = 0.51, MinTemperature = 20, MaxTemperature = 21, MinPressure = 0.075 },
            new Rule { Id = (int)BacteriumRule.ScopulumMethane, GenusId = (int)BacteriumGenus.Scopulum, MinGravity = 0.025, MaxGravity = 0.047, MinTemperature = 84, MaxTemperature = 110, MinPressure = 0.03 },
            new Rule { Id = (int)BacteriumRule.ScopulumNeon, GenusId = (int)BacteriumGenus.Scopulum, MinGravity = 0.025, MaxGravity = 0.61, MinTemperature = 20, MaxTemperature = 65, MaxPressure = 0.008 },
            new Rule { Id = (int)BacteriumRule.ScopulumNeonRich, GenusId = (int)BacteriumGenus.Scopulum, MinGravity = 0.025, MaxGravity = 0.61, MinTemperature = 20, MaxTemperature = 65, MinPressure = 0.005 },
            new Rule { Id = (int)BacteriumRule.ScopulumNitrogen, GenusId = (int)BacteriumGenus.Scopulum, MinGravity = 0.2, MaxGravity = 0.3, MinTemperature = 60, MaxTemperature = 70 },
            new Rule { Id = (int)BacteriumRule.ScopulumOxygen, GenusId = (int)BacteriumGenus.Scopulum, MinGravity = 0.27, MaxGravity = 0.40, MinTemperature = 150, MaxTemperature = 220, MinPressure = 0.01 }
        );

        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.ScopulumArgon, AtmosphereEnum.Argon);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.ScopulumHelium, AtmosphereEnum.Helium);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.ScopulumMethane, AtmosphereEnum.Methane);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.ScopulumNeon, AtmosphereEnum.Neon);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.ScopulumNeonRich, AtmosphereEnum.NeonRich);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.ScopulumNitrogen, AtmosphereEnum.Nitrogen);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.ScopulumOxygen, AtmosphereEnum.Oxygen);

        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.ScopulumArgon, BodyClassEnum.IcyBody, BodyClassEnum.RockyIceBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.ScopulumHelium, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.ScopulumMethane, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.ScopulumNeon, BodyClassEnum.IcyBody, BodyClassEnum.RockyIceBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.ScopulumNeonRich, BodyClassEnum.IcyBody, BodyClassEnum.RockyIceBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.ScopulumNitrogen, BodyClassEnum.IcyBody, BodyClassEnum.RockyIceBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.ScopulumOxygen, BodyClassEnum.IcyBody, BodyClassEnum.RockyIceBody);

        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.ScopulumArgon, VolcanismEnum.CarbonDioxide, VolcanismEnum.Methane);
        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.ScopulumHelium, VolcanismEnum.Methane);
        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.ScopulumMethane, VolcanismEnum.Methane);
        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.ScopulumNeon, VolcanismEnum.CarbonDioxide, VolcanismEnum.Methane);
        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.ScopulumNeonRich, VolcanismEnum.CarbonDioxide, VolcanismEnum.Methane);
        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.ScopulumNitrogen, VolcanismEnum.CarbonDioxide, VolcanismEnum.Methane);
        SeedHelpers.Volcanisms(modelBuilder, BacteriumRule.ScopulumOxygen, VolcanismEnum.CarbonDioxide, VolcanismEnum.Methane);
    }

    private static void SeedNebulus(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Nebulus, "$Codex_Ent_Bacterial_02_Name;", "Bacterium Nebulus", 5_289_900);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)BacteriumRule.NebulusIcy,
                GenusId = (int)BacteriumGenus.Nebulus,
                MinGravity = 0.4,
                MaxGravity = 0.55,
                MinTemperature = 20.0,
                MaxTemperature = 21.0,
                MinPressure = 0.067
            },
            new Rule
            {
                Id = (int)BacteriumRule.NebulusRockyIce,
                GenusId = (int)BacteriumGenus.Nebulus,
                MinGravity = 0.4,
                MaxGravity = 0.7,
                MinTemperature = 20.0,
                MaxTemperature = 21.0,
                MinPressure = 0.067
            });

        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.NebulusIcy, AtmosphereEnum.Helium);
        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.NebulusRockyIce, AtmosphereEnum.Helium);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.NebulusIcy, BodyClassEnum.IcyBody);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.NebulusRockyIce, BodyClassEnum.RockyIceBody);
    }

    private static void SeedAurasus(ModelBuilder modelBuilder)
    {
        SeedGenus(modelBuilder, BacteriumGenus.Aurasus, "$Codex_Ent_Bacterial_01_Name;", "Bacterium Aurasus", 1_000_000);

        modelBuilder.Entity<Rule>().HasData(new Rule
        {
            Id = (int)BacteriumRule.Aurasus,
            GenusId = (int)BacteriumGenus.Aurasus,
            MinGravity = 0.039,
            MaxGravity = 0.608,
            MinTemperature = 145.0,
            MaxTemperature = 400.0
        });

        SeedHelpers.Atmospheres(modelBuilder, BacteriumRule.Aurasus, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.BodyClasses(modelBuilder, BacteriumRule.Aurasus,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody,
            BodyClassEnum.RockyIceBody);
    }

    private static void SeedGenus(ModelBuilder modelBuilder, BacteriumGenus id, string codexName, string name, decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(new Genus
        {
            Id = (int)id,
            CodexType = "$Codex_Ent_Bacterial_Genus_Name;",
            CodexName = codexName,
            Name = name,
            Value = value,
            Distance = 500
        });
    }
}