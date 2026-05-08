namespace ED.Assistant.Data.Configurations;

sealed class SpeciesAtmosphereConditionConfiguration : IEntityTypeConfiguration<SpeciesAtmosphereCondition>
{
	public void Configure(EntityTypeBuilder<SpeciesAtmosphereCondition> builder)
	{
		builder.ToTable("SpeciesAtmosphereConditions");

		builder.HasKey(x => new { x.SpeciesId, x.AtmosphereId, x.Mode });

		builder.Property(x => x.Mode)
			.HasConversion<string>()
			.IsRequired();

		builder.HasOne(x => x.Species)
			.WithMany(x => x.AtmosphereConditions)
			.HasForeignKey(x => x.SpeciesId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Atmosphere)
			.WithMany(x => x.SpeciesConditions)
			.HasForeignKey(x => x.AtmosphereId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
