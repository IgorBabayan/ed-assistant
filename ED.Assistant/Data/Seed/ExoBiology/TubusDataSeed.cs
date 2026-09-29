namespace ED.Assistant.Data.Seed.ExoBiology;

static class TubusDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedConifer(modelBuilder);
        SeedSororibus(modelBuilder);
        SeedCavas(modelBuilder);
        SeedRosarium(modelBuilder);
        SeedCompagibus(modelBuilder);
    }

    private static void SeedCompagibus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubusGenus.Compagibus,
            "$Codex_Ent_Tubus_05_Name;",
            "Tubus Compagibus",
            7_774_700);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubusRule.Compagibus,
                GenusId = (int)TubusGenus.Compagibus,
                MinGravity = 0.04,
                MaxGravity = 0.153,
                MinTemperature = 160.0,
                MaxTemperature = 197.0,
                MinPressure = 0.003
            });

        SeedHelpers.Atmospheres(
            modelBuilder,
            TubusRule.Compagibus,
            AtmosphereEnum.CarbonDioxide);

        SeedHelpers.BodyClasses(
            modelBuilder,
            TubusRule.Compagibus,
            BodyClassEnum.RockyBody);

        SeedHelpers.Volcanisms(
            modelBuilder,
            TubusRule.Compagibus,
            VolcanismEnum.None);
    }

    private static void SeedRosarium(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubusGenus.Rosarium,
            "$Codex_Ent_Tubus_04_Name;",
            "Tubus Rosarium",
            2_637_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubusRule.Rosarium,
                GenusId = (int)TubusGenus.Rosarium,
                MinGravity = 0.04,
                MaxGravity = 0.153,
                MinTemperature = 160.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            });

        SeedHelpers.Atmospheres(
            modelBuilder,
            TubusRule.Rosarium,
            AtmosphereEnum.Ammonia);

        SeedHelpers.BodyClasses(
            modelBuilder,
            TubusRule.Rosarium,
            BodyClassEnum.RockyBody);
    }

    private static void SeedCavas(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubusGenus.Cavas,
            "$Codex_Ent_Tubus_03_Name;",
            "Tubus Cavas",
            11_873_200);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubusRule.Cavas,
                GenusId = (int)TubusGenus.Cavas,
                MinGravity = 0.04,
                MaxGravity = 0.152,
                MinTemperature = 160.0,
                MaxTemperature = 197.0,
                MinPressure = 0.003
            });

        SeedHelpers.Atmospheres(
            modelBuilder,
            TubusRule.Cavas,
            AtmosphereEnum.CarbonDioxide);

        SeedHelpers.BodyClasses(
            modelBuilder,
            TubusRule.Cavas,
            BodyClassEnum.RockyBody);

        SeedHelpers.Volcanisms(
            modelBuilder,
            TubusRule.Cavas,
            VolcanismEnum.None);
    }

    private static void SeedSororibus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubusGenus.Sororibus,
            "$Codex_Ent_Tubus_02_Name;",
            "Tubus Sororibus",
            5_727_600);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubusRule.SororibusAmmonia,
                GenusId = (int)TubusGenus.Sororibus,
                MinGravity = 0.045,
                MaxGravity = 0.152,
                MinTemperature = 160.0,
                MaxTemperature = 177.0,
                MaxPressure = 0.0135
            },
            new Rule
            {
                Id = (int)TubusRule.SororibusCarbonDioxide,
                GenusId = (int)TubusGenus.Sororibus,
                MinGravity = 0.045,
                MaxGravity = 0.152,
                MinTemperature = 160.0,
                MaxTemperature = 195.0
            });

        SeedHelpers.Atmospheres(
            modelBuilder,
            TubusRule.SororibusAmmonia,
            AtmosphereEnum.Ammonia);

        SeedHelpers.Atmospheres(
            modelBuilder,
            TubusRule.SororibusCarbonDioxide,
            AtmosphereEnum.CarbonDioxide);

        SeedHelpers.BodyClasses(
            modelBuilder,
            TubusRule.SororibusAmmonia,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.BodyClasses(
            modelBuilder,
            TubusRule.SororibusCarbonDioxide,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(
            modelBuilder,
            TubusRule.SororibusCarbonDioxide,
            VolcanismEnum.None);
    }

    private static void SeedConifer(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubusGenus.Conifer,
            "$Codex_Ent_Tubus_01_Name;",
            "Tubus Conifer",
            2_415_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubusRule.Conifer,
                GenusId = (int)TubusGenus.Conifer,
                MinGravity = 0.041,
                MaxGravity = 0.153,
                MinTemperature = 160.0,
                MaxTemperature = 197.0,
                MinPressure = 0.003
            });

        SeedHelpers.Atmospheres(
            modelBuilder,
            TubusRule.Conifer,
            AtmosphereEnum.CarbonDioxide);

        SeedHelpers.BodyClasses(
            modelBuilder,
            TubusRule.Conifer,
            BodyClassEnum.RockyBody);

        SeedHelpers.Volcanisms(
            modelBuilder,
            TubusRule.Conifer,
            VolcanismEnum.None);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        TubusGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Tubus_Genus_Name;",
                CodexName = codexName,
                Name = name,
                Value = value,
                Distance = 800
            });
    }
}