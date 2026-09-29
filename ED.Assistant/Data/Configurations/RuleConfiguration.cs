namespace ED.Assistant.Data.Configurations;

class RuleConfiguration : IEntityTypeConfiguration<Rule>
{
    public void Configure(EntityTypeBuilder<Rule> builder)
    {
        builder.ToTable(nameof(Rule));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.MinGravity);
        builder.Property(x => x.MaxGravity);

        builder.Property(x => x.MinTemperature);
        builder.Property(x => x.MaxTemperature);

        builder.Property(x => x.MinPressure);
        builder.Property(x => x.MaxPressure);

        builder.Property(x => x.MaxOrbitalPeriod);
        builder.Property(x => x.Guardian);

        builder
            .HasOne(x => x.Genus)
            .WithMany(x => x.Rules)
            .HasForeignKey(x => x.GenusId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(x => x.ParentBodyClasses)
            .WithMany(x => x.Rules)
            .UsingEntity<Dictionary<string, object>>(
                "RuleParentBodyClass",
                r => r.HasOne<ParentBodyClass>().WithMany().HasForeignKey("ParentBodyClassId").OnDelete(DeleteBehavior.Cascade),
                l => l.HasOne<Rule>().WithMany().HasForeignKey("RuleId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasKey("RuleId", "ParentBodyClassId"));

        ConfigureBodyClasses(builder);
        ConfigureSystemBodyClasses(builder);
        ConfigureAtmospheres(builder);
        ConfigureVolcanisms(builder);
    }

    private static void ConfigureSystemBodyClasses(EntityTypeBuilder<Rule> builder)
    {
        builder
            .HasMany(x => x.SystemBodyClasses)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "RuleSystemBodyClass",
                right => right
                    .HasOne<BodyClass>()
                    .WithMany()
                    .HasForeignKey("BodyClassId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<Rule>()
                    .WithMany()
                    .HasForeignKey("RuleId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("RuleSystemBodyClass");

                    join.HasKey(
                        "RuleId",
                        "BodyClassId");
                });
    }

    private static void ConfigureBodyClasses(EntityTypeBuilder<Rule> builder)
    {
        builder
            .HasMany(x => x.BodyClasses)
            .WithMany(x => x.Rules)
            .UsingEntity<Dictionary<string, object>>(
                "RuleBodyClass",
                right => right
                    .HasOne<BodyClass>()
                    .WithMany()
                    .HasForeignKey("BodyClassId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<Rule>()
                    .WithMany()
                    .HasForeignKey("RuleId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("RuleBodyClass");

                    join.HasKey(
                        "RuleId",
                        "BodyClassId");
                });
    }

    private static void ConfigureAtmospheres(EntityTypeBuilder<Rule> builder)
    {
        builder
            .HasMany(x => x.Atmospheres)
            .WithMany(x => x.Rules)
            .UsingEntity<Dictionary<string, object>>(
                "RuleAtmosphere",
                right => right
                    .HasOne<Atmosphere>()
                    .WithMany()
                    .HasForeignKey("AtmosphereId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<Rule>()
                    .WithMany()
                    .HasForeignKey("RuleId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("RuleAtmosphere");

                    join.HasKey(
                        "RuleId",
                        "AtmosphereId");
                });
    }

    private static void ConfigureVolcanisms(EntityTypeBuilder<Rule> builder)
    {
        builder
            .HasMany(x => x.Volcanisms)
            .WithMany(x => x.Rules)
            .UsingEntity<Dictionary<string, object>>(
                "RuleVolcanism",
                right => right
                    .HasOne<Volcanism>()
                    .WithMany()
                    .HasForeignKey("VolcanismId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<Rule>()
                    .WithMany()
                    .HasForeignKey("RuleId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("RuleVolcanism");

                    join.HasKey(
                        "RuleId",
                        "VolcanismId");
                });
    }
}