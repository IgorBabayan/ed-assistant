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
        
                SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.GelataArgon, AtmosphereEnum.Argon);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.GelataAmmoniaRocky, AtmosphereEnum.Ammonia);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.GelataAmmoniaHighMetal, AtmosphereEnum.Ammonia);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.GelataCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.GelataMethane, AtmosphereEnum.Methane);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.GelataWater, AtmosphereEnum.Water);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.GelataArgon,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.GelataAmmoniaRocky,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.GelataAmmoniaHighMetal,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.GelataCarbonDioxide,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.GelataMethane,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.GelataWater,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(modelBuilder, FungoidaRule.GelataArgon, VolcanismEnum.MajorSilicate);
        SeedHelpers.Volcanisms(modelBuilder, FungoidaRule.GelataAmmoniaRocky, VolcanismEnum.MajorSilicate);
        SeedHelpers.Volcanisms(modelBuilder, FungoidaRule.GelataAmmoniaHighMetal, VolcanismEnum.MajorRocky);
        SeedHelpers.Volcanisms(modelBuilder, FungoidaRule.GelataCarbonDioxide, VolcanismEnum.None);
        SeedHelpers.Volcanisms(
            modelBuilder,
            FungoidaRule.GelataMethane,
            VolcanismEnum.MajorSilicate,
            VolcanismEnum.MajorMetallic);
        SeedHelpers.Volcanisms(modelBuilder, FungoidaRule.GelataWater, VolcanismEnum.None);
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

        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.BullarumArgon, AtmosphereEnum.Argon);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.BullarumNitrogen, AtmosphereEnum.Nitrogen);

        foreach (var rule in new[]
                 {
                     FungoidaRule.BullarumArgon,
                     FungoidaRule.BullarumNitrogen
                 })
        {
            SeedHelpers.BodyClasses(
                modelBuilder,
                rule,
                BodyClassEnum.RockyBody,
                BodyClassEnum.RockyIceBody,
                BodyClassEnum.HighMetalContentBody);

            SeedHelpers.Volcanisms(modelBuilder, rule, VolcanismEnum.None);
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

        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.StabitisAmmonia, AtmosphereEnum.Ammonia);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.StabitisArgon, AtmosphereEnum.Argon);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.StabitisArgonRich, AtmosphereEnum.ArgonRich);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.StabitisCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.StabitisMethane, AtmosphereEnum.Methane);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.StabitisWater, AtmosphereEnum.Water);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.StabitisAmmonia,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.StabitisArgon,
            BodyClassEnum.RockyIceBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.StabitisArgonRich,
            BodyClassEnum.IcyBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.StabitisCarbonDioxide,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.StabitisMethane,
            BodyClassEnum.RockyBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.StabitisWater,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(modelBuilder, FungoidaRule.StabitisAmmonia, VolcanismEnum.Silicate);
        SeedHelpers.Volcanisms(modelBuilder, FungoidaRule.StabitisArgon, VolcanismEnum.Silicate, VolcanismEnum.Rocky);
        SeedHelpers.Volcanisms(modelBuilder, FungoidaRule.StabitisCarbonDioxide, VolcanismEnum.None);
        SeedHelpers.Volcanisms(modelBuilder, FungoidaRule.StabitisMethane, VolcanismEnum.MajorSilicate);
        SeedHelpers.Volcanisms(modelBuilder, FungoidaRule.StabitisWater, VolcanismEnum.None);
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

        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.SetisisAmmonia, AtmosphereEnum.Ammonia);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.SetisisMethaneRockyIce, AtmosphereEnum.Methane);
        SeedHelpers.Atmospheres(modelBuilder, FungoidaRule.SetisisMethaneRocky, AtmosphereEnum.Methane);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.SetisisAmmonia,
            BodyClassEnum.RockyBody,
            BodyClassEnum.RockyIceBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.SetisisMethaneRockyIce,
            BodyClassEnum.RockyIceBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            FungoidaRule.SetisisMethaneRocky,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(modelBuilder, FungoidaRule.SetisisMethaneRockyIce, VolcanismEnum.None);
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
                Value = value,
                Distance = 300
            });
    }
}