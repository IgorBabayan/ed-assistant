namespace ED.Assistant.Data.Configurations;

internal class AtmosphereComponentRuleConfiguration : IEntityTypeConfiguration<AtmosphereComponentRule>
{
    public void Configure(EntityTypeBuilder<AtmosphereComponentRule> builder)
    {
        builder.ToTable(nameof(AtmosphereComponentRule));

        builder.HasKey(x => new
        {
            x.RuleId,
            x.AtmosphereId
        });

        builder.Property(x => x.MinPercentage)
            .IsRequired();

        builder
            .HasOne(x => x.Rule)
            .WithMany(x => x.AtmosphereComponents)
            .HasForeignKey(x => x.RuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Atmosphere)
            .WithMany()
            .HasForeignKey(x => x.AtmosphereId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}