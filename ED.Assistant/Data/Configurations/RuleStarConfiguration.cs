namespace ED.Assistant.Data.Configurations;

internal class RuleStarConfiguration : IEntityTypeConfiguration<RuleStar>
{
    public void Configure(EntityTypeBuilder<RuleStar> builder)
    {
        builder.ToTable(nameof(RuleStar));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.LuminosityClass)
            .HasMaxLength(16);

        builder
            .HasOne(x => x.Rule)
            .WithMany(x => x.Stars)
            .HasForeignKey(x => x.RuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.StarClass)
            .WithMany(x => x.RuleStars)
            .HasForeignKey(x => x.StarClassId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}