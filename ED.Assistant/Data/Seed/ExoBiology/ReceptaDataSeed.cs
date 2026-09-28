namespace ED.Assistant.Data.Seed.ExoBiology;

static class ReceptaDataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedUmbrux(modelBuilder);
        SeedDeltahedronix(modelBuilder);
        SeedConditivus(modelBuilder);
    }

    private static void SeedConditivus(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            ReceptaGenus.Conditivus,
            "$Codex_Ent_Recepta_03_Name;",
            "Recepta Conditivus",
            14_313_700);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ReceptaRule.ConditivusCarbonDioxide,
                GenusId = (int)ReceptaGenus.Conditivus,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 150.0,
                MaxTemperature = 195.0
            },
            new Rule
            {
                Id = (int)ReceptaRule.ConditivusOxygenNone,
                GenusId = (int)ReceptaGenus.Conditivus,
                MinGravity = 0.23,
                MaxGravity = 0.276,
                MinTemperature = 154.0,
                MaxTemperature = 175.0,
                MinPressure = 0.01
            },
            new Rule
            {
                Id = (int)ReceptaRule.ConditivusOxygenWater,
                GenusId = (int)ReceptaGenus.Conditivus,
                MinGravity = 0.23,
                MaxGravity = 0.276,
                MinTemperature = 154.0,
                MaxTemperature = 175.0,
                MinPressure = 0.01
            },
            new Rule
            {
                Id = (int)ReceptaRule.ConditivusSulphurDioxide,
                GenusId = (int)ReceptaGenus.Conditivus,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 132.0,
                MaxTemperature = 275.0
            });

        SeedAtmospheres(
            modelBuilder,
            ReceptaRule.ConditivusCarbonDioxide,
            AtmosphereEnum.CarbonDioxide,
            AtmosphereEnum.CarbonDioxideRich);
        
        SeedAtmospheres(modelBuilder, ReceptaRule.ConditivusOxygenNone, AtmosphereEnum.Oxygen);
        SeedAtmospheres(modelBuilder, ReceptaRule.ConditivusOxygenWater, AtmosphereEnum.Oxygen);
        SeedAtmospheres(modelBuilder, ReceptaRule.ConditivusSulphurDioxide, AtmosphereEnum.SulphurDioxide);

        SeedBodyClasses(
            modelBuilder,
            ReceptaRule.ConditivusCarbonDioxide,
            BodyClassEnum.IcyBody,
            BodyClassEnum.RockyBody,
            BodyClassEnum.HighMetalContentBody);

        SeedBodyClasses(modelBuilder, ReceptaRule.ConditivusOxygenNone, BodyClassEnum.IcyBody);
        SeedBodyClasses(modelBuilder, ReceptaRule.ConditivusOxygenWater, BodyClassEnum.IcyBody);

        SeedVolcanisms(modelBuilder, ReceptaRule.ConditivusCarbonDioxide, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, ReceptaRule.ConditivusOxygenNone, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, ReceptaRule.ConditivusOxygenWater, VolcanismEnum.Water);

        SeedSulphurDioxideComponent(modelBuilder, ReceptaRule.ConditivusCarbonDioxide);
        SeedSulphurDioxideComponent(modelBuilder, ReceptaRule.ConditivusOxygenNone);
        SeedSulphurDioxideComponent(modelBuilder, ReceptaRule.ConditivusOxygenWater);
        SeedSulphurDioxideComponent(modelBuilder, ReceptaRule.ConditivusSulphurDioxide);
    }

    private static void SeedDeltahedronix(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            ReceptaGenus.Deltahedronix,
            "$Codex_Ent_Recepta_02_Name;",
            "Recepta Deltahedronix",
            16_202_800);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ReceptaRule.DeltahedronixCarbonDioxideNone,
                GenusId = (int)ReceptaGenus.Deltahedronix,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 150.0,
                MaxTemperature = 195.0
            },
            new Rule
            {
                Id = (int)ReceptaRule.DeltahedronixCarbonDioxideWater,
                GenusId = (int)ReceptaGenus.Deltahedronix,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 150.0,
                MaxTemperature = 195.0
            },
            new Rule
            {
                Id = (int)ReceptaRule.DeltahedronixSulphurDioxide,
                GenusId = (int)ReceptaGenus.Deltahedronix,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 132.0,
                MaxTemperature = 272.0
            });

        SeedAtmospheres(
            modelBuilder,
            ReceptaRule.DeltahedronixCarbonDioxideNone,
            AtmosphereEnum.CarbonDioxide);

        SeedAtmospheres(
            modelBuilder,
            ReceptaRule.DeltahedronixCarbonDioxideWater,
            AtmosphereEnum.CarbonDioxide);

        SeedAtmospheres(
            modelBuilder,
            ReceptaRule.DeltahedronixSulphurDioxide,
            AtmosphereEnum.SulphurDioxide);

        SeedBodyClasses(
            modelBuilder,
            ReceptaRule.DeltahedronixCarbonDioxideWater,
            BodyClassEnum.IcyBody,
            BodyClassEnum.RockyIceBody);

        SeedVolcanisms(
            modelBuilder,
            ReceptaRule.DeltahedronixCarbonDioxideNone,
            VolcanismEnum.None);

        SeedVolcanisms(
            modelBuilder,
            ReceptaRule.DeltahedronixCarbonDioxideWater,
            VolcanismEnum.Water);

        SeedSulphurDioxideComponent(modelBuilder, ReceptaRule.DeltahedronixCarbonDioxideNone);
        SeedSulphurDioxideComponent(modelBuilder, ReceptaRule.DeltahedronixCarbonDioxideWater);
        SeedSulphurDioxideComponent(modelBuilder, ReceptaRule.DeltahedronixSulphurDioxide);
    }

    private static void SeedUmbrux(ModelBuilder modelBuilder)
    {
        SeedGenus(
            modelBuilder,
            ReceptaGenus.Umbrux,
            "$Codex_Ent_Recepta_01_Name;",
            "Recepta Umbrux",
            12_934_900);

        modelBuilder.Entity<Rule>().HasData(
            new Rule
            {
                Id = (int)ReceptaRule.UmbruxCarbonDioxide,
                GenusId = (int)ReceptaGenus.Umbrux,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 151.0,
                MaxTemperature = 200.0
            },
            new Rule
            {
                Id = (int)ReceptaRule.UmbruxOxygenNone,
                GenusId = (int)ReceptaGenus.Umbrux,
                MinGravity = 0.23,
                MaxGravity = 0.276,
                MinTemperature = 154.0,
                MaxTemperature = 175.0,
                MinPressure = 0.01
            },
            new Rule
            {
                Id = (int)ReceptaRule.UmbruxOxygenWater,
                GenusId = (int)ReceptaGenus.Umbrux,
                MinGravity = 0.23,
                MaxGravity = 0.276,
                MinTemperature = 154.0,
                MaxTemperature = 175.0,
                MinPressure = 0.01
            },
            new Rule
            {
                Id = (int)ReceptaRule.UmbruxSulphurDioxide,
                GenusId = (int)ReceptaGenus.Umbrux,
                MinGravity = 0.04,
                MaxGravity = 0.276,
                MinTemperature = 132.0,
                MaxTemperature = 273.0
            });
        
        SeedAtmospheres(modelBuilder, ReceptaRule.UmbruxCarbonDioxide, AtmosphereEnum.CarbonDioxide);
        SeedAtmospheres(modelBuilder, ReceptaRule.UmbruxOxygenNone, AtmosphereEnum.Oxygen);
        SeedAtmospheres(modelBuilder, ReceptaRule.UmbruxOxygenWater, AtmosphereEnum.Oxygen);
        SeedAtmospheres(modelBuilder, ReceptaRule.UmbruxSulphurDioxide, AtmosphereEnum.SulphurDioxide);

        SeedBodyClasses(modelBuilder, ReceptaRule.UmbruxOxygenNone, BodyClassEnum.IcyBody);
        SeedBodyClasses(modelBuilder, ReceptaRule.UmbruxOxygenWater, BodyClassEnum.IcyBody);

        SeedVolcanisms(modelBuilder, ReceptaRule.UmbruxOxygenNone, VolcanismEnum.None);
        SeedVolcanisms(modelBuilder, ReceptaRule.UmbruxOxygenWater, VolcanismEnum.Water);

        SeedSulphurDioxideComponent(modelBuilder, ReceptaRule.UmbruxCarbonDioxide);
        SeedSulphurDioxideComponent(modelBuilder, ReceptaRule.UmbruxOxygenNone);
        SeedSulphurDioxideComponent(modelBuilder, ReceptaRule.UmbruxOxygenWater);
        SeedSulphurDioxideComponent(modelBuilder, ReceptaRule.UmbruxSulphurDioxide);
    }

    private static void SeedGenus(
        ModelBuilder modelBuilder,
        ReceptaGenus id,
        string codexName,
        string name,
        decimal value)
    {
        modelBuilder.Entity<Genus>().HasData(
            new Genus
            {
                Id = (int)id,
                CodexType = "$Codex_Ent_Recepta_Genus_Name;",
                CodexName = codexName,
                Name = name,
                Value = value,
                Distance = 150
            });
    }

    private static void SeedAtmospheres(
        ModelBuilder modelBuilder,
        ReceptaRule rule,
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
        ReceptaRule rule,
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
        ReceptaRule rule,
        params VolcanismEnum[] volcanisms)
    {
        modelBuilder.Entity("RuleVolcanism").HasData(
            volcanisms.Select(x => new
            {
                RuleId = (int)rule,
                VolcanismId = (int)x
            }));
    }

    private static void SeedSulphurDioxideComponent(
        ModelBuilder modelBuilder,
        ReceptaRule rule)
    {
        modelBuilder.Entity<AtmosphereComponentRule>().HasData(
            new AtmosphereComponentRule
            {
                RuleId = (int)rule,
                AtmosphereId = (int)AtmosphereEnum.SulphurDioxide,
                MinPercentage = 1.05
            });
    }
}