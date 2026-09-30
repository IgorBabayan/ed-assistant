namespace ED.Assistant.Data.Seed.ExoBiology;

internal static class BrainTreeDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedRoseum(modelBuilder);
        SeedGypseeum(modelBuilder);
        SeedOstrinum(modelBuilder);
        SeedViride(modelBuilder);
        SeedAureum(modelBuilder);
        SeedPuniceum(modelBuilder);
        SeedLindigoticum(modelBuilder);
        SeedLividum(modelBuilder);
    }

    private static void SeedLividum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            BrainTreeGenus.Lividum,
            "$Codex_Ent_SeedEFGH_Name;",
            "Lividum Brain Tree");

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)BrainTreeRule.Lividum,
                GenusId = (int)BrainTreeGenus.Lividum,
                MaxGravity = 0.5,
                MinTemperature = 300.0,
                MaxTemperature = 500.0,
                Guardian = true
            });

        SeedHelpers.BodyClasses(
            modelBuilder,
            BrainTreeRule.Lividum,
            BodyClassEnum.RockyBody);

        SeedHelpers.Volcanisms(
            modelBuilder,
            BrainTreeRule.Lividum,
            VolcanismEnum.Metallic,
            VolcanismEnum.Rocky,
            VolcanismEnum.Silicate,
            VolcanismEnum.Water);
    }

    private static void SeedLindigoticum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            BrainTreeGenus.Lindigoticum,
            "$Codex_Ent_SeedEFGH_03_Name;",
            "Lindigoticum Brain Tree");

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)BrainTreeRule.Lindigoticum,
                GenusId = (int)BrainTreeGenus.Lindigoticum,
                MaxGravity = 2.7,
                MinTemperature = 300.0,
                MaxTemperature = 500.0,
                Guardian = true
            });

        SeedHelpers.BodyClasses(
            modelBuilder,
            BrainTreeRule.Lindigoticum,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(
            modelBuilder,
            BrainTreeRule.Lindigoticum,
            VolcanismEnum.Rocky,
            VolcanismEnum.Silicate,
            VolcanismEnum.Metallic);

        SeedSystemBodyClasses(
            modelBuilder,
            BrainTreeRule.Lindigoticum,
            BodyClassEnum.EarthLikeBody,
            BodyClassEnum.GasGiantWithWaterBasedLife,
            BodyClassEnum.WaterGiant);
    }

    private static void SeedPuniceum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            BrainTreeGenus.Puniceum,
            "$Codex_Ent_SeedEFGH_02_Name;",
            "Puniceum Brain Tree");

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)BrainTreeRule.Puniceum,
                GenusId = (int)BrainTreeGenus.Puniceum,
                Guardian = true
            });

        SeedHelpers.BodyClasses(
            modelBuilder,
            BrainTreeRule.Puniceum,
            BodyClassEnum.MetalRichBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(modelBuilder, BrainTreeRule.Puniceum, VolcanismEnum.Any);

        SeedSystemBodyClasses(
            modelBuilder,
            BrainTreeRule.Puniceum,
            BodyClassEnum.EarthLikeBody,
            BodyClassEnum.GasGiantWithWaterBasedLife,
            BodyClassEnum.WaterGiant);
    }

    private static void SeedAureum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            BrainTreeGenus.Aureum,
            "$Codex_Ent_SeedEFGH_01_Name;",
            "Aureum Brain Tree");

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)BrainTreeRule.Aureum,
                GenusId = (int)BrainTreeGenus.Aureum,
                MaxGravity = 2.9,
                MinTemperature = 300.0,
                MaxTemperature = 500.0,
                Guardian = true
            });

        SeedHelpers.BodyClasses(
            modelBuilder,
            BrainTreeRule.Aureum,
            BodyClassEnum.MetalRichBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(
            modelBuilder,
            BrainTreeRule.Aureum,
            VolcanismEnum.Metallic,
            VolcanismEnum.Rocky,
            VolcanismEnum.Silicate);
    }

    private static void SeedViride(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            BrainTreeGenus.Viride,
            "$Codex_Ent_SeedABCD_03_Name;",
            "Viride Brain Tree");

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)BrainTreeRule.Viride,
                GenusId = (int)BrainTreeGenus.Viride,
                MaxGravity = 0.4,
                MinTemperature = 100.0,
                MaxTemperature = 270.0,
                Guardian = true
            });

        SeedHelpers.BodyClasses(
            modelBuilder,
            BrainTreeRule.Viride,
            BodyClassEnum.RockyIceBody);

        SeedHelpers.Volcanisms(modelBuilder, BrainTreeRule.Viride, VolcanismEnum.Any);

        SeedSystemBodyClasses(
            modelBuilder,
            BrainTreeRule.Viride,
            BodyClassEnum.EarthLikeBody,
            BodyClassEnum.GasGiantWithWaterBasedLife,
            BodyClassEnum.WaterGiant);
    }

    private static void SeedOstrinum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            BrainTreeGenus.Ostrinum,
            "$Codex_Ent_SeedABCD_02_Name;",
            "Ostrinum Brain Tree");

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)BrainTreeRule.Ostrinum,
                GenusId = (int)BrainTreeGenus.Ostrinum,
                Guardian = true
            });

        SeedHelpers.BodyClasses(
            modelBuilder,
            BrainTreeRule.Ostrinum,
            BodyClassEnum.MetalRichBody,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedHelpers.Volcanisms(
            modelBuilder,
            BrainTreeRule.Ostrinum,
            VolcanismEnum.Metallic,
            VolcanismEnum.Rocky,
            VolcanismEnum.Silicate);
    }

    private static void SeedGypseeum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            BrainTreeGenus.Gypseeum,
            "$Codex_Ent_SeedABCD_01_Name;",
            "Gypseeum Brain Tree");

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)BrainTreeRule.Gypseeum,
                GenusId = (int)BrainTreeGenus.Gypseeum,
                MaxGravity = 0.42,
                MinTemperature = 200.0,
                MaxTemperature = 400.0,
                Guardian = true
            });

        SeedHelpers.BodyClasses(
            modelBuilder,
            BrainTreeRule.Gypseeum,
            BodyClassEnum.RockyBody);

        SeedHelpers.Volcanisms(
            modelBuilder,
            BrainTreeRule.Gypseeum,
            VolcanismEnum.Metallic,
            VolcanismEnum.Rocky,
            VolcanismEnum.Silicate,
            VolcanismEnum.Water);

        SeedSystemBodyClasses(
            modelBuilder,
            BrainTreeRule.Gypseeum,
            BodyClassEnum.EarthLikeBody,
            BodyClassEnum.GasGiantWithWaterBasedLife,
            BodyClassEnum.WaterGiant);
    }

    private static void SeedRoseum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            BrainTreeGenus.Roseum,
            "$Codex_Ent_Seed_Name;",
            "Roseum Brain Tree");

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)BrainTreeRule.Roseum,
                GenusId = (int)BrainTreeGenus.Roseum,
                MinTemperature = 200.0,
                MaxTemperature = 500.0,
                Guardian = true
            });

        SeedHelpers.Volcanisms(modelBuilder, BrainTreeRule.Roseum, VolcanismEnum.Any);
    }
    
    private static void SeedGenus(
        ModelBuilder modelBuilder,
        BrainTreeGenus id,
        string codexName,
        string name)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Brancae_Name;",
                CodexName = codexName,
                Name = name,
                Value = 1_593_700,
                Distance = 100
            });
    }

    private static void SeedSystemBodyClasses(
        ModelBuilder modelBuilder,
        BrainTreeRule rule,
        params BodyClassEnum[] bodyClasses)
    {
        modelBuilder.Entity("RuleSystemBodyClass").HasData(
            bodyClasses.Select(x => new
            {
                RuleId = (int)rule,
                BodyClassId = (int)x
            }));
    }
}