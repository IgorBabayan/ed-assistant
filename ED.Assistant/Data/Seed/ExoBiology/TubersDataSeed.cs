namespace ED.Assistant.Data.Seed.ExoBiology;

static class TubersDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedRoseum(modelBuilder);
        SeedPrasinum(modelBuilder);
        SeedAlbidum(modelBuilder);
        SeedCaeruleum(modelBuilder);
        SeedLindigoticum(modelBuilder);
        SeedViolaceum(modelBuilder);
        SeedViride(modelBuilder);
        SeedBlatteum(modelBuilder);
    }

    private static void SeedBlatteum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubersGenus.Blatteum,
            "$Codex_Ent_TubeEFGH_Name;",
            "Blatteum Sinuous Tubers",
            1_514_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubersRule.Blatteum,
                GenusId = (int)TubersGenus.Blatteum,
                MinTemperature = 200.0,
                MaxTemperature = 500.0
            });

        SeedBodyClasses(
            modelBuilder,
            TubersRule.Blatteum,
            BodyClassEnum.MetalRichBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(
            modelBuilder,
            TubersRule.Blatteum,
            VolcanismEnum.MetallicMagmaVolcanism,
            VolcanismEnum.RockyMagmaVolcanism,
            VolcanismEnum.MajorSilicateVapour);
    }

    private static void SeedViride(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubersGenus.Viride,
            "$Codex_Ent_TubeEFGH_03_Name;",
            "Viride Sinuous Tubers",
            1_514_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubersRule.VirideHighMetalContent,
                GenusId = (int)TubersGenus.Viride,
                MinTemperature = 200.0,
                MaxTemperature = 500.0
            },
            new Rule
            {
                Id = (int)TubersRule.VirideRocky,
                GenusId = (int)TubersGenus.Viride,
                MinTemperature = 200.0,
                MaxTemperature = 500.0,
                MaxOrbitalPeriod = 86_400
            });

        SeedBodyClasses(
            modelBuilder,
            TubersRule.VirideHighMetalContent,
            BodyClassEnum.HighMetalContentBody);

        SeedBodyClasses(
            modelBuilder,
            TubersRule.VirideRocky,
            BodyClassEnum.RockyBody);

        foreach (var rule in new[]
                 {
                     TubersRule.VirideHighMetalContent,
                     TubersRule.VirideRocky
                 })
        {
            SeedVolcanisms(
                modelBuilder,
                rule,
                VolcanismEnum.MajorRockyMagma,
                VolcanismEnum.MajorSilicateVapour);
        }
    }

    private static void SeedViolaceum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubersGenus.Violaceum,
            "$Codex_Ent_TubeEFGH_02_Name;",
            "Violaceum Sinuous Tubers",
            1_514_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubersRule.Violaceum,
                GenusId = (int)TubersGenus.Violaceum,
                MinTemperature = 200.0,
                MaxTemperature = 500.0
            });

        SeedBodyClasses(
            modelBuilder,
            TubersRule.Violaceum,
            BodyClassEnum.MetalRichBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(
            modelBuilder,
            TubersRule.Violaceum,
            VolcanismEnum.MajorRockyMagma,
            VolcanismEnum.MajorSilicateVapour);
    }

    private static void SeedLindigoticum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubersGenus.Lindigoticum,
            "$Codex_Ent_TubeEFGH_01_Name;",
            "Lindigoticum Sinuous Tubers",
            1_514_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubersRule.Lindigoticum,
                GenusId = (int)TubersGenus.Lindigoticum,
                MinTemperature = 200.0,
                MaxTemperature = 500.0,
                MaxOrbitalPeriod = 86_400
            });

        SeedBodyClasses(
            modelBuilder,
            TubersRule.Lindigoticum,
            BodyClassEnum.RockyBody);

        SeedVolcanisms(
            modelBuilder,
            TubersRule.Lindigoticum,
            VolcanismEnum.MajorSilicateVapour);
    }

    private static void SeedCaeruleum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubersGenus.Caeruleum,
            "$Codex_Ent_TubeABCD_03_Name;",
            "Caeruleum Sinuous Tubers",
            1_514_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubersRule.CaeruleumTuberCondition,
                GenusId = (int)TubersGenus.Caeruleum,
                MinTemperature = 200.0,
                MaxTemperature = 500.0,
                MaxOrbitalPeriod = 86_400
            },
            new Rule
            {
                Id = (int)TubersRule.CaeruleumRegionCondition,
                GenusId = (int)TubersGenus.Caeruleum,
                MinTemperature = 200.0,
                MaxTemperature = 500.0
            });

        SeedBodyClasses(
            modelBuilder,
            TubersRule.CaeruleumTuberCondition,
            BodyClassEnum.RockyBody);

        SeedBodyClasses(
            modelBuilder,
            TubersRule.CaeruleumRegionCondition,
            BodyClassEnum.RockyBody);

        SeedVolcanisms(
            modelBuilder,
            TubersRule.CaeruleumTuberCondition,
            VolcanismEnum.MajorSilicateVapour);

        SeedVolcanisms(
            modelBuilder,
            TubersRule.CaeruleumRegionCondition,
            VolcanismEnum.MajorSilicateVapour);
    }

    private static void SeedAlbidum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubersGenus.Albidum,
            "$Codex_Ent_TubeABCD_02_Name;",
            "Albidum Sinuous Tubers",
            1_514_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubersRule.Albidum,
                GenusId = (int)TubersGenus.Albidum,
                MinTemperature = 200.0,
                MaxTemperature = 500.0,
                MaxOrbitalPeriod = 86_400
            });

        SeedBodyClasses(
            modelBuilder,
            TubersRule.Albidum,
            BodyClassEnum.RockyBody);

        SeedVolcanisms(
            modelBuilder,
            TubersRule.Albidum,
            VolcanismEnum.MajorSilicateVapour,
            VolcanismEnum.MajorMetallicMagma);
    }

    private static void SeedPrasinum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubersGenus.Prasinum,
            "$Codex_Ent_TubeABCD_01_Name;",
            "Prasinum Sinuous Tubers",
            1_514_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubersRule.PrasinumAny,
                GenusId = (int)TubersGenus.Prasinum,
                MinTemperature = 200.0,
                MaxTemperature = 500.0
            },
            new Rule
            {
                Id = (int)TubersRule.PrasinumTuberCondition,
                GenusId = (int)TubersGenus.Prasinum,
                MinTemperature = 200.0,
                MaxTemperature = 500.0
            },
            new Rule
            {
                Id = (int)TubersRule.PrasinumRegionCondition,
                GenusId = (int)TubersGenus.Prasinum,
                MinTemperature = 200.0,
                MaxTemperature = 500.0
            });
        
        SeedBodyClasses(
            modelBuilder,
            TubersRule.PrasinumAny,
            BodyClassEnum.MetalRichBody,
            BodyClassEnum.HighMetalContentBody,
            BodyClassEnum.RockyBody);

        SeedBodyClasses(
            modelBuilder,
            TubersRule.PrasinumTuberCondition,
            BodyClassEnum.MetalRichBody,
            BodyClassEnum.HighMetalContentBody);

        SeedBodyClasses(
            modelBuilder,
            TubersRule.PrasinumRegionCondition,
            BodyClassEnum.MetalRichBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(
            modelBuilder,
            TubersRule.PrasinumAny,
            VolcanismEnum.Any);

        SeedVolcanisms(
            modelBuilder,
            TubersRule.PrasinumTuberCondition,
            VolcanismEnum.MajorRockyMagma,
            VolcanismEnum.MajorSilicateVapour);

        SeedVolcanisms(
            modelBuilder,
            TubersRule.PrasinumRegionCondition,
            VolcanismEnum.MajorRockyMagma,
            VolcanismEnum.MajorSilicateVapour);
    }

    private static void SeedRoseum(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            TubersGenus.Roseum,
            "$Codex_Ent_Tube_Name;",
            "Roseum Sinuous Tubers",
            1_514_500);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)TubersRule.Roseum,
                GenusId = (int)TubersGenus.Roseum,
                MinTemperature = 200.0,
                MaxTemperature = 500.0
            });

        SeedBodyClasses(
            modelBuilder,
            TubersRule.Roseum,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(
            modelBuilder,
            TubersRule.Roseum,
            VolcanismEnum.RockyMagma);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        TubersGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Tube_Name;",
                CodexName = codexName,
                Name = name,
                Value = value
            });
    }

    private static void SeedBodyClasses(
        ModelBuilder modelBuilder,
        TubersRule rule,
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
        TubersRule rule,
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