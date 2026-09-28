namespace ED.Assistant.Data.Seed.ExoBiology;

static class AnemoneDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedLuteolum(modelBuilder);
        SeedCroceum(modelBuilder);
        SeedPuniceum(modelBuilder);
        SeedRoseum(modelBuilder);
        SeedRubeumBioluminescent(modelBuilder);
        SeedPrasinumBioluminescent(modelBuilder);
        SeedRoseumBioluminescent(modelBuilder);
        SeedBlatteumBioluminescent(modelBuilder);
    }

    private static void SeedBlatteumBioluminescent(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AnemoneGenus.BlatteumBioluminescent,
                CodexType = "$Codex_Ent_Sphere_Name;",
                CodexName = "$Codex_Ent_SphereEFGH_Name;",
                Name = "Blatteum Bioluminescent Anemone",
                Value = 1_499_900,
                Distance = 100
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AnemoneRule.BlatteumBioluminescent,
                GenusId = (int)AnemoneGenus.BlatteumBioluminescent,
                MinTemperature = 220.0
            });

        SeedBodyClasses(
            modelBuilder,
            AnemoneRule.BlatteumBioluminescent,
            BodyClassEnum.MetalRichBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(
            modelBuilder,
            AnemoneRule.BlatteumBioluminescent,
            VolcanismEnum.Any);

        SeedStars(
            modelBuilder,
            AnemoneRule.BlatteumBioluminescent,
            (StarClassEnum.B, "IV"),
            (StarClassEnum.B, "V"));
    }

    private static void SeedRoseumBioluminescent(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AnemoneGenus.RoseumBioluminescent,
                CodexType = "$Codex_Ent_Sphere_Name;",
                CodexName = "$Codex_Ent_SphereEFGH_03_Name;",
                Name = "Roseum Bioluminescent Anemone",
                Value = 1_499_900,
                Distance = 100
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AnemoneRule.RoseumBioluminescent,
                GenusId = (int)AnemoneGenus.RoseumBioluminescent,
                MinGravity = 0.036,
                MaxGravity = 4.61,
                MinTemperature = 400.0
            });

        SeedBodyClasses(
            modelBuilder,
            AnemoneRule.RoseumBioluminescent,
            BodyClassEnum.MetalRichBody,
            BodyClassEnum.HighMetalContentBody);

        SeedVolcanisms(
            modelBuilder,
            AnemoneRule.RoseumBioluminescent,
            VolcanismEnum.Any);

        SeedStars(
            modelBuilder,
            AnemoneRule.RoseumBioluminescent,
            (StarClassEnum.B, "I"),
            (StarClassEnum.B, "II"),
            (StarClassEnum.B, "III"));
    }

    private static void SeedPrasinumBioluminescent(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AnemoneGenus.PrasinumBioluminescent,
                CodexType = "$Codex_Ent_Sphere_Name;",
                CodexName = "$Codex_Ent_SphereEFGH_02_Name;",
                Name = "Prasinum Bioluminescent Anemone",
                Value = 1_499_900,
                Distance = 100
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AnemoneRule.PrasinumBioluminescent,
                GenusId = (int)AnemoneGenus.PrasinumBioluminescent,
                MinGravity = 0.036,
                MinTemperature = 110.0,
                MaxTemperature = 3050.0
            });

        SeedBodyClasses(
            modelBuilder,
            AnemoneRule.PrasinumBioluminescent,
            BodyClassEnum.MetalRichBody,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedStars(
            modelBuilder,
            AnemoneRule.PrasinumBioluminescent,
            (StarClassEnum.O, null));
    }

    private static void SeedRubeumBioluminescent(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AnemoneGenus.RubeumBioluminescent,
                CodexType = "$Codex_Ent_Sphere_Name;",
                CodexName = "$Codex_Ent_SphereEFGH_01_Name;",
                Name = "Rubeum Bioluminescent Anemone",
                Value = 1_499_900,
                Distance = 100
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AnemoneRule.RubeumBioluminescent,
                GenusId = (int)AnemoneGenus.RubeumBioluminescent,
                MinGravity = 0.036,
                MaxGravity = 4.61,
                MinTemperature = 160.0,
                MaxTemperature = 1800.0
            });

        SeedBodyClasses(modelBuilder, AnemoneRule.RubeumBioluminescent, BodyClassEnum.MetalRichBody,
            BodyClassEnum.HighMetalContentBody);
        SeedVolcanisms(modelBuilder, AnemoneRule.RubeumBioluminescent, VolcanismEnum.Any);
        SeedStars(modelBuilder, AnemoneRule.RubeumBioluminescent, (StarClassEnum.B, "VI"),
            (StarClassEnum.A, "I"), (StarClassEnum.A, "II"),
            (StarClassEnum.A, "III"), (StarClassEnum.N, null));
    }

    private static void SeedRoseum(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AnemoneGenus.Roseum,
                CodexType = "$Codex_Ent_Sphere_Name;",
                CodexName = "$Codex_Ent_SphereABCD_03_Name;",
                Name = "Roseum Anemone",
                Value = 1_499_900,
                Distance = 100
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AnemoneRule.Roseum,
                GenusId = (int)AnemoneGenus.Roseum,
                MinGravity = 0.045,
                MaxGravity = 0.37,
                MinTemperature = 200.0,
                MaxTemperature = 440.0
            });

        SeedBodyClasses(modelBuilder, AnemoneRule.Roseum, BodyClassEnum.RockyBody);
        SeedVolcanisms(modelBuilder, AnemoneRule.Roseum, VolcanismEnum.Silicate,
            VolcanismEnum.Rocky, VolcanismEnum.Metallic);
        SeedStars(modelBuilder, AnemoneRule.Roseum,
            (StarClassEnum.B, "I"), (StarClassEnum.B, "II"),
            (StarClassEnum.B, "III"), (StarClassEnum.B, "IV"));
    }

    private static void SeedPuniceum(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AnemoneGenus.Puniceum,
                CodexType = "$Codex_Ent_Sphere_Name;",
                CodexName = "$Codex_Ent_SphereABCD_02_Name;",
                Name = "Puniceum Anemone",
                Value = 1_499_900,
                Distance = 100
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AnemoneRule.PuniceumNoVolcanism,
                GenusId = (int)AnemoneGenus.Puniceum,
                MinGravity = 0.17,
                MaxGravity = 2.52,
                MinTemperature = 65.0,
                MaxTemperature = 800.0
            },
            new Rule
            {
                Id = (int)AnemoneRule.PuniceumCarbonDioxideGeysers,
                GenusId = (int)AnemoneGenus.Puniceum,
                MinGravity = 0.17,
                MaxGravity = 2.52,
                MinTemperature = 65.0,
                MaxTemperature = 800.0
            });

        SeedBodyClasses(modelBuilder, AnemoneRule.PuniceumNoVolcanism, BodyClassEnum.IcyBody,
            BodyClassEnum.RockyIceBody);
        SeedBodyClasses(modelBuilder, AnemoneRule.PuniceumCarbonDioxideGeysers, BodyClassEnum.IcyBody,
            BodyClassEnum.RockyIceBody);
        SeedVolcanisms(modelBuilder, AnemoneRule.PuniceumNoVolcanism, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, AnemoneRule.PuniceumCarbonDioxideGeysers,
            VolcanismEnum.CarbonDioxideGeysers);
        SeedStars(modelBuilder, AnemoneRule.PuniceumNoVolcanism, (StarClassEnum.O, null));
        SeedStars(modelBuilder, AnemoneRule.PuniceumCarbonDioxideGeysers, (StarClassEnum.O, null));
    }

    private static void SeedCroceum(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AnemoneGenus.Croceum,
                CodexType = "$Codex_Ent_Sphere_Name;",
                CodexName = "$Codex_Ent_SphereABCD_01_Name;",
                Name = "Croceum Anemone",
                Value = 1_499_900,
                Distance = 100
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AnemoneRule.Croceum,
                GenusId = (int)AnemoneGenus.Croceum,
                MinGravity = 0.047,
                MaxGravity = 0.37,
                MinTemperature = 200.0,
                MaxTemperature = 440.0
            });

        SeedBodyClasses(modelBuilder, AnemoneRule.Croceum, BodyClassEnum.RockyBody);
        SeedVolcanisms(modelBuilder, AnemoneRule.Croceum, VolcanismEnum.Silicate,
            VolcanismEnum.Rocky, VolcanismEnum.Metallic);
        SeedStars(modelBuilder, AnemoneRule.Croceum, (StarClassEnum.B, "V"),
            (StarClassEnum.B, "VI"), (StarClassEnum.A, "III"));
    }

    private static void SeedLuteolum(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)AnemoneGenus.Luteolum,
                CodexType = "$Codex_Ent_Sphere_Name;",
                CodexName = "$Codex_Ent_Sphere_Name;",
                Name = "Luteolum Anemone",
                Value = 1_499_900,
                Distance = 100
            }
        );
        
        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)AnemoneRule.Luteolum,
                GenusId = (int)AnemoneGenus.Luteolum,
                MinGravity = 0.044,
                MaxGravity = 1.28,
                MinTemperature = 200.0,
                MaxTemperature = 440.0
            }
        );
        
        SeedBodyClasses(modelBuilder, AnemoneRule.Luteolum, BodyClassEnum.RockyBody);
        SeedVolcanisms(modelBuilder, AnemoneRule.Luteolum, VolcanismEnum.Metallic,
            VolcanismEnum.Silicate, VolcanismEnum.Rocky, VolcanismEnum.Water);
        SeedStars(modelBuilder,AnemoneRule.Luteolum,
            (StarClassEnum.B, "IV"), (StarClassEnum.B, "V"));
    }
    
    private static void SeedBodyClasses(ModelBuilder modelBuilder, AnemoneRule rule, params BodyClassEnum[] bodyClasses)
    {
        modelBuilder.Entity("RuleBodyClass").HasData(
            bodyClasses.Select(bodyClass => new
            {
                RuleId = (int)rule,
                BodyClassId = (int)bodyClass
            }));
    }
    
    private static void SeedVolcanisms(ModelBuilder modelBuilder, AnemoneRule rule, params VolcanismEnum[] volcanisms)
    {
        modelBuilder.Entity("RuleVolcanism").HasData(
            volcanisms.Select(volcanism => new
            {
                RuleId = (int)rule,
                VolcanismId = (int)volcanism
            }));
    }
    
    private static void SeedStars(ModelBuilder modelBuilder, AnemoneRule rule,
        params (StarClassEnum StarClass, string? LuminosityClass)[] stars)
    {
        var index = 1;
        foreach (var star in stars)
        {
            modelBuilder.Entity<RuleStar>().HasData(
                new RuleStar
                {
                    Id = ((int)rule * 100) + index++,
                    RuleId = (int)rule,
                    StarClassId = (int)star.StarClass,
                    LuminosityClass = star.LuminosityClass,
                    Type = RuleStarType.Star
                }
            );
        }
    }
}
