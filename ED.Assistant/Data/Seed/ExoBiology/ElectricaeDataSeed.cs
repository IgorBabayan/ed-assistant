namespace ED.Assistant.Data.Seed.ExoBiology;

static class ElectricaeDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedPluma(modelBuilder);
        SeedRadialem(modelBuilder);
    }

    private static void SeedRadialem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)ElectricaeGenus.Radialem,
                CodexType = "$Codex_Ent_Electricae_Genus_Name;",
                CodexName = "$Codex_Ent_Electricae_02_Name;",
                Name = "Electricae Radialem",
                Value = 6_284_600,
                Distance = 1000
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ElectricaeRule.RadialemArgon,
                GenusId = (int)ElectricaeGenus.Radialem,
                MinGravity = 0.025,
                MaxGravity = 0.276,
                MinTemperature = 50.0,
                MaxTemperature = 150.0,
                Nebula = NebulaRuleType.All
            },
            new Rule
            {
                Id = (int)ElectricaeRule.RadialemNeon,
                GenusId = (int)ElectricaeGenus.Radialem,
                MinGravity = 0.026,
                MaxGravity = 0.276,
                MinTemperature = 20.0,
                MaxTemperature = 70.0,
                MaxPressure = 0.005,
                Nebula = NebulaRuleType.All
            });

        SeedAtmospheres(
            modelBuilder,
            ElectricaeRule.RadialemArgon,
            AtmosphereEnum.Argon,
            AtmosphereEnum.ArgonRich);

        SeedAtmospheres(
            modelBuilder,
            ElectricaeRule.RadialemNeon,
            AtmosphereEnum.Neon,
            AtmosphereEnum.NeonRich);

        SeedBodyClasses(modelBuilder, ElectricaeRule.RadialemArgon, BodyClassEnum.IcyBody);
        SeedBodyClasses(modelBuilder, ElectricaeRule.RadialemNeon, BodyClassEnum.IcyBody);
    }

    private static void SeedPluma(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)ElectricaeGenus.Pluma,
                CodexType = "$Codex_Ent_Electricae_Genus_Name;",
                CodexName = "$Codex_Ent_Electricae_01_Name;",
                Name = "Electricae Pluma",
                Value = 6_284_600,
                Distance = 1000
            });

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ElectricaeRule.PlumaArgon,
                GenusId = (int)ElectricaeGenus.Pluma,
                MinGravity = 0.025,
                MaxGravity = 0.276,
                MinTemperature = 50.0,
                MaxTemperature = 150.0
            },
            new Rule
            {
                Id = (int)ElectricaeRule.PlumaNeon,
                GenusId = (int)ElectricaeGenus.Pluma,
                MinGravity = 0.26,
                MaxGravity = 0.276,
                MinTemperature = 20.0,
                MaxTemperature = 70.0,
                MaxPressure = 0.005
            });

        SeedAtmospheres(
            modelBuilder,
            ElectricaeRule.PlumaArgon,
            AtmosphereEnum.Argon,
            AtmosphereEnum.ArgonRich);

        SeedAtmospheres(
            modelBuilder,
            ElectricaeRule.PlumaNeon,
            AtmosphereEnum.Neon,
            AtmosphereEnum.NeonRich);

        SeedBodyClasses(modelBuilder, ElectricaeRule.PlumaArgon, BodyClassEnum.IcyBody);
        SeedBodyClasses(modelBuilder, ElectricaeRule.PlumaNeon, BodyClassEnum.IcyBody);

        SeedParentStars(
            modelBuilder,
            ElectricaeRule.PlumaArgon,
            StarClassEnum.A,
            StarClassEnum.N,
            StarClassEnum.D,
            StarClassEnum.H,
            StarClassEnum.AeBe);

        SeedParentStars(
            modelBuilder,
            ElectricaeRule.PlumaNeon,
            StarClassEnum.A,
            StarClassEnum.N,
            StarClassEnum.D,
            StarClassEnum.H,
            StarClassEnum.AeBe);
    }

    private static void SeedAtmospheres(
        ModelBuilder modelBuilder,
        ElectricaeRule rule,
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
        ElectricaeRule rule,
        params BodyClassEnum[] bodyClasses)
    {
        modelBuilder.Entity("RuleBodyClass").HasData(
            bodyClasses.Select(x => new
            {
                RuleId = (int)rule,
                BodyClassId = (int)x
            }));
    }

    private static void SeedParentStars(
        ModelBuilder modelBuilder,
        ElectricaeRule rule,
        params StarClassEnum[] starClasses)
    {
        var index = 1;

        foreach (var starClass in starClasses)
        {
            modelBuilder.Entity<RuleStar>().HasData(
                new RuleStar
                {
                    Id = ((int)rule * 100) + index++,
                    RuleId = (int)rule,
                    StarClassId = (int)starClass,
                    LuminosityClass = null,
                    Type = RuleStarType.ParentStar
                });
        }
    }
}