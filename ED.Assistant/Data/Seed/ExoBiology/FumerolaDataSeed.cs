namespace ED.Assistant.Data.Seed.ExoBiology;

static class FumerolaDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedCarbosis(modelBuilder);
        SeedExtremus(modelBuilder);
        SeedNitris(modelBuilder);
        SeedAquatis(modelBuilder);
    }

    private static void SeedAquatis(ModelBuilder modelBuilder)
    {
                SeedGenus(
            modelBuilder,
            FumerolaGenus.Aquatis,
            "$Codex_Ent_Fumerolas_04_Name;",
            "Fumerola Aquatis",
            6_284_600);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FumerolaRule.AquatisAmmonia,
                GenusId = (int)FumerolaGenus.Aquatis,
                MinGravity = 0.028,
                MaxGravity = 0.276,
                MinTemperature = 161.0,
                MaxTemperature = 177.0,
                MinPressure = 0.002,
                MaxPressure = 0.02
            },
            new Rule
            {
                Id = (int)FumerolaRule.AquatisArgonGroup,
                GenusId = (int)FumerolaGenus.Aquatis,
                MinGravity = 0.166,
                MaxGravity = 0.276,
                MinTemperature = 57.0,
                MaxTemperature = 150.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.AquatisCarbonDioxide,
                GenusId = (int)FumerolaGenus.Aquatis,
                MinGravity = 0.25,
                MaxGravity = 0.276,
                MinTemperature = 160.0,
                MaxTemperature = 180.0,
                MinPressure = 0.01,
                MaxPressure = 0.03
            },
            new Rule
            {
                Id = (int)FumerolaRule.AquatisMethane,
                GenusId = (int)FumerolaGenus.Aquatis,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 80.0,
                MaxTemperature = 100.0,
                MinPressure = 0.01
            },
            new Rule
            {
                Id = (int)FumerolaRule.AquatisNeon,
                GenusId = (int)FumerolaGenus.Aquatis,
                MinGravity = 0.26,
                MaxGravity = 0.276,
                MinTemperature = 20.0,
                MaxTemperature = 60.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.AquatisNitrogen,
                GenusId = (int)FumerolaGenus.Aquatis,
                MinGravity = 0.195,
                MaxGravity = 0.245,
                MinTemperature = 56.0,
                MaxTemperature = 80.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.AquatisOxygen,
                GenusId = (int)FumerolaGenus.Aquatis,
                MinGravity = 0.23,
                MaxGravity = 0.276,
                MinTemperature = 153.0,
                MaxTemperature = 190.0,
                MinPressure = 0.01
            },
            new Rule
            {
                Id = (int)FumerolaRule.AquatisSulphurDioxide,
                GenusId = (int)FumerolaGenus.Aquatis,
                MinGravity = 0.18,
                MaxGravity = 0.276,
                MinTemperature = 150.0,
                MaxTemperature = 270.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.AquatisWater,
                GenusId = (int)FumerolaGenus.Aquatis,
                MinGravity = 0.04,
                MaxGravity = 0.06
            });
        
                SeedAtmospheres(modelBuilder, FumerolaRule.AquatisAmmonia, AtmosphereEnum.Ammonia);
        SeedAtmospheres(modelBuilder, FumerolaRule.AquatisArgonGroup, AtmosphereEnum.Argon, AtmosphereEnum.ArgonRich);
        SeedAtmospheres(modelBuilder, FumerolaRule.AquatisCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedAtmospheres(modelBuilder, FumerolaRule.AquatisMethane, AtmosphereEnum.Methane);
        SeedAtmospheres(modelBuilder, FumerolaRule.AquatisNeon, AtmosphereEnum.Neon);
        SeedAtmospheres(modelBuilder, FumerolaRule.AquatisNitrogen, AtmosphereEnum.Nitrogen);
        SeedAtmospheres(modelBuilder, FumerolaRule.AquatisOxygen, AtmosphereEnum.Oxygen);
        SeedAtmospheres(modelBuilder, FumerolaRule.AquatisSulphurDioxide, AtmosphereEnum.SulphurDioxide);
        SeedAtmospheres(modelBuilder, FumerolaRule.AquatisWater, AtmosphereEnum.Water);

        SeedBodyClasses(
            modelBuilder,
            FumerolaRule.AquatisAmmonia,
            BodyClassEnum.IcyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.RockyBody);

        SeedBodyClasses(
            modelBuilder,
            FumerolaRule.AquatisArgonGroup,
            BodyClassEnum.IcyBody,
            BodyClassEnum.RockyIceBody);

        SeedBodyClasses(modelBuilder, FumerolaRule.AquatisCarbonDioxide, BodyClassEnum.IcyBody);
        SeedBodyClasses(modelBuilder, FumerolaRule.AquatisMethane, BodyClassEnum.RockyBody);
        SeedBodyClasses(modelBuilder, FumerolaRule.AquatisNeon, BodyClassEnum.IcyBody);
        SeedBodyClasses(modelBuilder, FumerolaRule.AquatisNitrogen, BodyClassEnum.IcyBody);
        SeedBodyClasses(modelBuilder, FumerolaRule.AquatisOxygen, BodyClassEnum.IcyBody);

        SeedBodyClasses(
            modelBuilder,
            FumerolaRule.AquatisSulphurDioxide,
            BodyClassEnum.IcyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.RockyBody);

        SeedBodyClasses(modelBuilder, FumerolaRule.AquatisWater, BodyClassEnum.RockyBody);

        foreach (var rule in new[]
        {
            FumerolaRule.AquatisAmmonia,
            FumerolaRule.AquatisArgonGroup,
            FumerolaRule.AquatisCarbonDioxide,
            FumerolaRule.AquatisMethane,
            FumerolaRule.AquatisNeon,
            FumerolaRule.AquatisNitrogen,
            FumerolaRule.AquatisOxygen,
            FumerolaRule.AquatisSulphurDioxide,
            FumerolaRule.AquatisWater
        })
        {
            SeedVolcanisms(modelBuilder, rule, VolcanismEnum.Water);
        }
    }

    private static void SeedNitris(ModelBuilder modelBuilder)
    {
                SeedGenus(
            modelBuilder,
            FumerolaGenus.Nitris,
            "$Codex_Ent_Fumerolas_03_Name;",
            "Fumerola Nitris",
            7_500_900);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FumerolaRule.NitrisNeon,
                GenusId = (int)FumerolaGenus.Nitris,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 30.0,
                MaxTemperature = 129.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.NitrisArgonGroup,
                GenusId = (int)FumerolaGenus.Nitris,
                MinGravity = 0.044,
                MaxGravity = 0.276,
                MinTemperature = 50.0,
                MaxTemperature = 141.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.NitrisMethane,
                GenusId = (int)FumerolaGenus.Nitris,
                MinGravity = 0.025,
                MaxGravity = 0.1,
                MinTemperature = 83.0,
                MaxTemperature = 109.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.NitrisNitrogen,
                GenusId = (int)FumerolaGenus.Nitris,
                MinGravity = 0.21,
                MaxGravity = 0.276,
                MinTemperature = 60.0,
                MaxTemperature = 81.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.NitrisOxygen,
                GenusId = (int)FumerolaGenus.Nitris,
                MaxGravity = 0.276,
                MinTemperature = 150.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.NitrisSulphurDioxide,
                GenusId = (int)FumerolaGenus.Nitris,
                MinGravity = 0.21,
                MaxGravity = 0.276,
                MinTemperature = 160.0,
                MaxTemperature = 250.0
            });

        SeedAtmospheres(modelBuilder, FumerolaRule.NitrisNeon, AtmosphereEnum.Neon);
        SeedAtmospheres(
            modelBuilder,
            FumerolaRule.NitrisArgonGroup,
            AtmosphereEnum.Argon,
            AtmosphereEnum.ArgonRich,
            AtmosphereEnum.NeonRich);
        SeedAtmospheres(modelBuilder, FumerolaRule.NitrisMethane, AtmosphereEnum.Methane);
        SeedAtmospheres(modelBuilder, FumerolaRule.NitrisNitrogen, AtmosphereEnum.Nitrogen);
        SeedAtmospheres(modelBuilder, FumerolaRule.NitrisOxygen, AtmosphereEnum.Oxygen);
        SeedAtmospheres(modelBuilder, FumerolaRule.NitrisSulphurDioxide, AtmosphereEnum.SulphurDioxide);

        foreach (var rule in new[]
        {
            FumerolaRule.NitrisNeon,
            FumerolaRule.NitrisArgonGroup,
            FumerolaRule.NitrisMethane,
            FumerolaRule.NitrisNitrogen,
            FumerolaRule.NitrisOxygen,
            FumerolaRule.NitrisSulphurDioxide
        })
        {
            SeedBodyClasses(modelBuilder, rule, BodyClassEnum.IcyBody);
        }

        SeedVolcanisms(modelBuilder, FumerolaRule.NitrisNeon, VolcanismEnum.Nitrogen, VolcanismEnum.Ammonia);
        SeedVolcanisms(modelBuilder, FumerolaRule.NitrisArgonGroup, VolcanismEnum.Nitrogen, VolcanismEnum.Ammonia);
        SeedVolcanisms(modelBuilder, FumerolaRule.NitrisMethane, VolcanismEnum.Nitrogen);
        SeedVolcanisms(modelBuilder, FumerolaRule.NitrisNitrogen, VolcanismEnum.Nitrogen, VolcanismEnum.Ammonia);
        SeedVolcanisms(modelBuilder, FumerolaRule.NitrisOxygen, VolcanismEnum.Nitrogen, VolcanismEnum.Ammonia);
        SeedVolcanisms(modelBuilder, FumerolaRule.NitrisSulphurDioxide, VolcanismEnum.Nitrogen, VolcanismEnum.Ammonia);
    }

    private static void SeedExtremus(ModelBuilder modelBuilder)
    {
                SeedGenus(
            modelBuilder,
            FumerolaGenus.Extremus,
            "$Codex_Ent_Fumerolas_02_Name;",
            "Fumerola Extremus",
            16_202_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FumerolaRule.ExtremusAmmonia,
                GenusId = (int)FumerolaGenus.Extremus,
                MinGravity = 0.04,
                MaxGravity = 0.09,
                MinTemperature = 161.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            },
            new Rule
            {
                Id = (int)FumerolaRule.ExtremusArgon,
                GenusId = (int)FumerolaGenus.Extremus,
                MinGravity = 0.07,
                MaxGravity = 0.276,
                MinTemperature = 50.0,
                MaxTemperature = 121.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.ExtremusMethane,
                GenusId = (int)FumerolaGenus.Extremus,
                MinGravity = 0.025,
                MaxGravity = 0.127,
                MinTemperature = 77.0,
                MaxTemperature = 109.0,
                MinPressure = 0.01
            },
            new Rule
            {
                Id = (int)FumerolaRule.ExtremusSulphurDioxide,
                GenusId = (int)FumerolaGenus.Extremus,
                MinGravity = 0.07,
                MaxGravity = 0.276,
                MinTemperature = 54.0,
                MaxTemperature = 210.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.ExtremusCarbonDioxide,
                GenusId = (int)FumerolaGenus.Extremus,
                MinGravity = 0.05,
                MaxGravity = 0.276,
                MinTemperature = 500.0
            });

        SeedAtmospheres(modelBuilder, FumerolaRule.ExtremusAmmonia, AtmosphereEnum.Ammonia);
        SeedAtmospheres(modelBuilder, FumerolaRule.ExtremusArgon, AtmosphereEnum.Argon);
        SeedAtmospheres(modelBuilder, FumerolaRule.ExtremusMethane, AtmosphereEnum.Methane);
        SeedAtmospheres(modelBuilder, FumerolaRule.ExtremusSulphurDioxide, AtmosphereEnum.SulphurDioxide);
        SeedAtmospheres(modelBuilder, FumerolaRule.ExtremusCarbonDioxide, AtmosphereEnum.CarbonDioxide);

        SeedBodyClasses(
            modelBuilder,
            FumerolaRule.ExtremusAmmonia,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);

        SeedBodyClasses(
            modelBuilder,
            FumerolaRule.ExtremusArgon,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);

        SeedBodyClasses(
            modelBuilder,
            FumerolaRule.ExtremusMethane,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);

        SeedBodyClasses(
            modelBuilder,
            FumerolaRule.ExtremusSulphurDioxide,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody);

        SeedBodyClasses(
            modelBuilder,
            FumerolaRule.ExtremusCarbonDioxide,
            BodyClassEnum.HighMetalContentBody);

        foreach (var rule in new[]
        {
            FumerolaRule.ExtremusAmmonia,
            FumerolaRule.ExtremusArgon,
            FumerolaRule.ExtremusMethane,
            FumerolaRule.ExtremusSulphurDioxide,
            FumerolaRule.ExtremusCarbonDioxide
        })
        {
            SeedVolcanisms(
                modelBuilder,
                rule,
                VolcanismEnum.Silicate,
                VolcanismEnum.Metallic,
                VolcanismEnum.Rocky);
        }
    }

    private static void SeedCarbosis(ModelBuilder modelBuilder)
    {
                SeedGenus(
            modelBuilder,
            FumerolaGenus.Carbosis,
            "$Codex_Ent_Fumerolas_01_Name;",
            "Fumerola Carbosis",
            6_284_600);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FumerolaRule.CarbosisArgon,
                GenusId = (int)FumerolaGenus.Carbosis,
                MinGravity = 0.168,
                MaxGravity = 0.276,
                MinTemperature = 57.0,
                MaxTemperature = 150.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.CarbosisMethane,
                GenusId = (int)FumerolaGenus.Carbosis,
                MinGravity = 0.025,
                MaxGravity = 0.047,
                MinTemperature = 84.0,
                MaxTemperature = 110.0,
                MinPressure = 0.03
            },
            new Rule
            {
                Id = (int)FumerolaRule.CarbosisNeon,
                GenusId = (int)FumerolaGenus.Carbosis,
                MinGravity = 0.26,
                MaxGravity = 0.276,
                MinTemperature = 40.0,
                MaxTemperature = 60.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.CarbosisNitrogen,
                GenusId = (int)FumerolaGenus.Carbosis,
                MinGravity = 0.2,
                MaxGravity = 0.276,
                MinTemperature = 57.0,
                MaxTemperature = 70.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.CarbosisOxygen,
                GenusId = (int)FumerolaGenus.Carbosis,
                MinGravity = 0.26,
                MaxGravity = 0.276,
                MinTemperature = 160.0,
                MaxTemperature = 180.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.CarbosisSulphurDioxide,
                GenusId = (int)FumerolaGenus.Carbosis,
                MinGravity = 0.185,
                MaxGravity = 0.276,
                MinTemperature = 149.0,
                MaxTemperature = 272.0
            },
            new Rule
            {
                Id = (int)FumerolaRule.CarbosisOtherAtmospheres,
                GenusId = (int)FumerolaGenus.Carbosis,
                MaxGravity = 0.276
            });

        SeedAtmospheres(modelBuilder, FumerolaRule.CarbosisArgon, AtmosphereEnum.Argon);
        SeedAtmospheres(modelBuilder, FumerolaRule.CarbosisMethane, AtmosphereEnum.Methane);
        SeedAtmospheres(modelBuilder, FumerolaRule.CarbosisNeon, AtmosphereEnum.Neon);
        SeedAtmospheres(modelBuilder, FumerolaRule.CarbosisNitrogen, AtmosphereEnum.Nitrogen);
        SeedAtmospheres(modelBuilder, FumerolaRule.CarbosisOxygen, AtmosphereEnum.Oxygen);
        SeedAtmospheres(modelBuilder, FumerolaRule.CarbosisSulphurDioxide, AtmosphereEnum.SulphurDioxide);
        SeedAtmospheres(
            modelBuilder,
            FumerolaRule.CarbosisOtherAtmospheres,
            AtmosphereEnum.Ammonia,
            AtmosphereEnum.ArgonRich,
            AtmosphereEnum.CarbonDioxideRich);

        SeedBodyClasses(modelBuilder, FumerolaRule.CarbosisArgon, BodyClassEnum.IcyBody, BodyClassEnum.RockyIceBody);
        SeedBodyClasses(modelBuilder, FumerolaRule.CarbosisMethane, BodyClassEnum.IcyBody);
        SeedBodyClasses(modelBuilder, FumerolaRule.CarbosisNeon, BodyClassEnum.IcyBody);
        SeedBodyClasses(modelBuilder, FumerolaRule.CarbosisNitrogen, BodyClassEnum.IcyBody);
        SeedBodyClasses(modelBuilder, FumerolaRule.CarbosisOxygen, BodyClassEnum.IcyBody);
        SeedBodyClasses(modelBuilder, FumerolaRule.CarbosisSulphurDioxide, BodyClassEnum.IcyBody, BodyClassEnum.RockyIceBody);
        SeedBodyClasses(modelBuilder, FumerolaRule.CarbosisOtherAtmospheres, BodyClassEnum.IcyBody);

        SeedVolcanisms(modelBuilder, FumerolaRule.CarbosisArgon, VolcanismEnum.Carbon, VolcanismEnum.Methane);
        SeedVolcanisms(modelBuilder, FumerolaRule.CarbosisMethane, VolcanismEnum.MethaneMagma);
        SeedVolcanisms(modelBuilder, FumerolaRule.CarbosisNeon, VolcanismEnum.Carbon, VolcanismEnum.Methane);
        SeedVolcanisms(modelBuilder, FumerolaRule.CarbosisNitrogen, VolcanismEnum.Carbon, VolcanismEnum.Methane);
        SeedVolcanisms(modelBuilder, FumerolaRule.CarbosisOxygen, VolcanismEnum.Carbon);
        SeedVolcanisms(modelBuilder, FumerolaRule.CarbosisSulphurDioxide, VolcanismEnum.Carbon, VolcanismEnum.Methane);
        SeedVolcanisms(modelBuilder, FumerolaRule.CarbosisOtherAtmospheres, VolcanismEnum.Carbon);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        FumerolaGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Fumerolas_Genus_Name;",
                CodexName = codexName,
                Name = name,
                Value = value
            });
    }

    private static void SeedAtmospheres(
        ModelBuilder modelBuilder,
        FumerolaRule rule,
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
        FumerolaRule rule,
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
        FumerolaRule rule,
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