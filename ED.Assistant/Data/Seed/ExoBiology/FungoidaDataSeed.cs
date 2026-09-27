namespace ED.Assistant.Data.Seed.ExoBiology;

static class FungoidaDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedSetisis(modelBuilder);
        SeedStabitis(modelBuilder);
        SeedBullarum(modelBuilder);
        SeedGelata(modelBuilder);
    }

    private static void SeedGelata(ModelBuilder modelBuilder)
    {
                SeedGenus(
            modelBuilder,
            FungoidaGenus.Gelata,
            "$Codex_Ent_Fungoids_04_Name;",
            "Fungoida Gelata",
            3_330_300);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FungoidaRule.GelataArgon,
                GenusId = (int)FungoidaGenus.Gelata,
                MinGravity = 0.041,
                MaxGravity = 0.276,
                MinTemperature = 160.0,
                MaxTemperature = 180.0,
                MaxPressure = 0.0135
            },
            new Rule
            {
                Id = (int)FungoidaRule.GelataAmmoniaRocky,
                GenusId = (int)FungoidaGenus.Gelata,
                MinGravity = 0.042,
                MaxGravity = 0.071,
                MinTemperature = 160.0,
                MaxTemperature = 180.0,
                MaxPressure = 0.0135
            },
            new Rule
            {
                Id = (int)FungoidaRule.GelataAmmoniaHighMetal,
                GenusId = (int)FungoidaGenus.Gelata,
                MinGravity = 0.042,
                MaxGravity = 0.071,
                MinTemperature = 160.0,
                MaxTemperature = 180.0,
                MaxPressure = 0.0135
            },
            new Rule
            {
                Id = (int)FungoidaRule.GelataCarbonDioxide,
                GenusId = (int)FungoidaGenus.Gelata,
                MinGravity = 0.041,
                MaxGravity = 0.276,
                MinTemperature = 180.0,
                MinPressure = 0.025
            },
            new Rule
            {
                Id = (int)FungoidaRule.GelataMethane,
                GenusId = (int)FungoidaGenus.Gelata,
                MinGravity = 0.044,
                MaxGravity = 0.125,
                MinTemperature = 80.0,
                MaxTemperature = 110.0,
                MinPressure = 0.01
            },
            new Rule
            {
                Id = (int)FungoidaRule.GelataWater,
                GenusId = (int)FungoidaGenus.Gelata,
                MinGravity = 0.039,
                MaxGravity = 0.063
            });
        
                SeedAtmospheres(modelBuilder, FungoidaRule.GelataArgon, AtmosphereEnum.Argon);
        SeedAtmospheres(modelBuilder, FungoidaRule.GelataAmmoniaRocky, AtmosphereEnum.Ammonia);
        SeedAtmospheres(modelBuilder, FungoidaRule.GelataAmmoniaHighMetal, AtmosphereEnum.Ammonia);
        SeedAtmospheres(modelBuilder, FungoidaRule.GelataCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedAtmospheres(modelBuilder, FungoidaRule.GelataMethane, AtmosphereEnum.Methane);
        SeedAtmospheres(modelBuilder, FungoidaRule.GelataWater, AtmosphereEnum.Water);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.GelataArgon,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.GelataAmmoniaRocky,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.GelataAmmoniaHighMetal,
            BodyClassEnum.HighMetalContentBody);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.GelataCarbonDioxide,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.GelataMethane,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.GelataWater,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(modelBuilder, FungoidaRule.GelataArgon, VolcanismEnum.MajorSilicate);
        SeedVolcanisms(modelBuilder, FungoidaRule.GelataAmmoniaRocky, VolcanismEnum.MajorSilicate);
        SeedVolcanisms(modelBuilder, FungoidaRule.GelataAmmoniaHighMetal, VolcanismEnum.MajorRocky);
        SeedVolcanisms(modelBuilder, FungoidaRule.GelataCarbonDioxide, VolcanismEnum.None);
        SeedVolcanisms(
            modelBuilder,
            FungoidaRule.GelataMethane,
            VolcanismEnum.MajorSilicate,
            VolcanismEnum.MajorMetallic);
        SeedVolcanisms(modelBuilder, FungoidaRule.GelataWater, VolcanismEnum.None);
    }

    private static void SeedBullarum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            FungoidaGenus.Bullarum,
            "$Codex_Ent_Fungoids_03_Name;",
            "Fungoida Bullarum",
            3_703_200);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FungoidaRule.BullarumArgon,
                GenusId = (int)FungoidaGenus.Bullarum,
                MinGravity = 0.058,
                MaxGravity = 0.276,
                MinTemperature = 50.0,
                MaxTemperature = 129.0
            },
            new Rule
            {
                Id = (int)FungoidaRule.BullarumNitrogen,
                GenusId = (int)FungoidaGenus.Bullarum,
                MinGravity = 0.155,
                MaxGravity = 0.276,
                MinTemperature = 50.0,
                MaxTemperature = 70.0
            });

        SeedAtmospheres(modelBuilder, FungoidaRule.BullarumArgon, AtmosphereEnum.Argon);
        SeedAtmospheres(modelBuilder, FungoidaRule.BullarumNitrogen, AtmosphereEnum.Nitrogen);

        foreach (var rule in new[]
                 {
                     FungoidaRule.BullarumArgon,
                     FungoidaRule.BullarumNitrogen
                 })
        {
            SeedBodyClasses(
                modelBuilder,
                rule,
                BodyClassEnum.RockyBody,
                BodyClassEnum.RockyIceBody,
                BodyClassEnum.HighMetalContentBody);

            SeedVolcanisms(modelBuilder, rule, VolcanismEnum.None);
        }
    }

    private static void SeedStabitis(ModelBuilder modelBuilder)
    {
                SeedGenus(
            modelBuilder,
            FungoidaGenus.Stabitis,
            "$Codex_Ent_Fungoids_02_Name;",
            "Fungoida Stabitis",
            2_680_300);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FungoidaRule.StabitisAmmonia,
                GenusId = (int)FungoidaGenus.Stabitis,
                MinGravity = 0.04,
                MaxGravity = 0.045,
                MinTemperature = 172.0,
                MaxTemperature = 177.0
            },
            new Rule
            {
                Id = (int)FungoidaRule.StabitisArgon,
                GenusId = (int)FungoidaGenus.Stabitis,
                MinGravity = 0.20,
                MaxGravity = 0.23,
                MinTemperature = 60.0,
                MaxTemperature = 90.0
            },
            new Rule
            {
                Id = (int)FungoidaRule.StabitisArgonRich,
                GenusId = (int)FungoidaGenus.Stabitis,
                MinGravity = 0.3,
                MaxGravity = 0.5,
                MinTemperature = 60.0,
                MaxTemperature = 90.0
            },
            new Rule
            {
                Id = (int)FungoidaRule.StabitisCarbonDioxide,
                GenusId = (int)FungoidaGenus.Stabitis,
                MinGravity = 0.0405,
                MaxGravity = 0.27,
                MinTemperature = 180.0,
                MinPressure = 0.025
            },
            new Rule
            {
                Id = (int)FungoidaRule.StabitisMethane,
                GenusId = (int)FungoidaGenus.Stabitis,
                MinGravity = 0.043,
                MaxGravity = 0.126,
                MinTemperature = 78.5,
                MaxTemperature = 109.0,
                MinPressure = 0.012
            },
            new Rule
            {
                Id = (int)FungoidaRule.StabitisWater,
                GenusId = (int)FungoidaGenus.Stabitis,
                MinGravity = 0.039,
                MaxGravity = 0.064
            });

        SeedAtmospheres(modelBuilder, FungoidaRule.StabitisAmmonia, AtmosphereEnum.Ammonia);
        SeedAtmospheres(modelBuilder, FungoidaRule.StabitisArgon, AtmosphereEnum.Argon);
        SeedAtmospheres(modelBuilder, FungoidaRule.StabitisArgonRich, AtmosphereEnum.ArgonRich);
        SeedAtmospheres(modelBuilder, FungoidaRule.StabitisCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedAtmospheres(modelBuilder, FungoidaRule.StabitisMethane, AtmosphereEnum.Methane);
        SeedAtmospheres(modelBuilder, FungoidaRule.StabitisWater, AtmosphereEnum.Water);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.StabitisAmmonia,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.StabitisArgon,
            BodyClassEnum.RockyIceBody);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.StabitisArgonRich,
            BodyClassEnum.IcyBody);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.StabitisCarbonDioxide,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.StabitisMethane,
            BodyClassEnum.RockyBody);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.StabitisWater,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(modelBuilder, FungoidaRule.StabitisAmmonia, VolcanismEnum.Silicate);
        SeedVolcanisms(modelBuilder, FungoidaRule.StabitisArgon, VolcanismEnum.Silicate, VolcanismEnum.Rocky);
        SeedVolcanisms(modelBuilder, FungoidaRule.StabitisCarbonDioxide, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, FungoidaRule.StabitisMethane, VolcanismEnum.MajorSilicate);
        SeedVolcanisms(modelBuilder, FungoidaRule.StabitisWater, VolcanismEnum.None);
    }

    private static void SeedSetisis(ModelBuilder modelBuilder)
    {
                SeedGenus(
            modelBuilder,
            FungoidaGenus.Setisis,
            "$Codex_Ent_Fungoids_01_Name;",
            "Fungoida Setisis",
            1_670_100);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)FungoidaRule.SetisisAmmonia,
                GenusId = (int)FungoidaGenus.Setisis,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 152.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            },
            new Rule
            {
                Id = (int)FungoidaRule.SetisisMethaneRockyIce,
                GenusId = (int)FungoidaGenus.Setisis,
                MinGravity = 0.033,
                MaxGravity = 0.276,
                MinTemperature = 68.0,
                MaxTemperature = 109.0
            },
            new Rule
            {
                Id = (int)FungoidaRule.SetisisMethaneRocky,
                GenusId = (int)FungoidaGenus.Setisis,
                MinGravity = 0.033,
                MaxGravity = 0.276,
                MinTemperature = 67.0,
                MaxTemperature = 109.0
            });

        SeedAtmospheres(modelBuilder, FungoidaRule.SetisisAmmonia, AtmosphereEnum.Ammonia);
        SeedAtmospheres(modelBuilder, FungoidaRule.SetisisMethaneRockyIce, AtmosphereEnum.Methane);
        SeedAtmospheres(modelBuilder, FungoidaRule.SetisisMethaneRocky, AtmosphereEnum.Methane);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.SetisisAmmonia,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.SetisisMethaneRockyIce,
            BodyClassEnum.RockyIceBody);

        SeedBodyClasses(
            modelBuilder,
            FungoidaRule.SetisisMethaneRocky,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(modelBuilder, FungoidaRule.SetisisMethaneRockyIce, VolcanismEnum.None);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        FungoidaGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Fungoids_Genus_Name;",
                CodexName = codexName,
                Name = name,
                Value = value
            });
    }

    private static void SeedAtmospheres(
        ModelBuilder modelBuilder,
        FungoidaRule rule,
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
        FungoidaRule rule,
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
        FungoidaRule rule,
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