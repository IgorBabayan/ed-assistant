namespace ED.Assistant.Data.Configurations;

sealed class BioSpawnRuleConfiguration : IEntityTypeConfiguration<BioSpawnRule>
{
	public void Configure(EntityTypeBuilder<BioSpawnRule> builder)
	{
		builder.ToTable("BioSpawnRules");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.AtmosphereRaw).IsRequired();
		builder.Property(x => x.VolcanismRaw).IsRequired();

		builder.HasOne(x => x.Species)
			.WithOne(x => x.SpawnRule)
			.HasForeignKey<BioSpawnRule>(x => x.SpeciesId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasIndex(x => x.SpeciesId)
			.IsUnique();
	}
}
