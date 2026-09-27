namespace ED.Assistant.Data.Configurations;

sealed class BioSpawnRuleAtmosphereConfiguration : IEntityTypeConfiguration<BioSpawnRuleAtmosphere>
{
	public void Configure(EntityTypeBuilder<BioSpawnRuleAtmosphere> builder)
	{
		builder.ToTable("BioSpawnRuleAtmospheres");
		builder.HasKey(x => new { x.SpawnRuleId, x.AtmosphereId, x.Mode });
		builder.HasOne(x => x.SpawnRule).WithMany(x => x.Atmospheres)
			.HasForeignKey(x => x.SpawnRuleId).OnDelete(DeleteBehavior.Cascade);
		builder.HasOne(x => x.Atmosphere).WithMany(x => x.SpawnRules)
			.HasForeignKey(x => x.AtmosphereId).OnDelete(DeleteBehavior.Restrict);
		builder.Property(x => x.Mode).HasConversion<string>();
	}
}
