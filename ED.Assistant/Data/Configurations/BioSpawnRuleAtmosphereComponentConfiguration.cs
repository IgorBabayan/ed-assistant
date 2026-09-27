namespace ED.Assistant.Data.Configurations;

sealed class BioSpawnRuleAtmosphereComponentConfiguration : IEntityTypeConfiguration<BioSpawnRuleAtmosphereComponent>
{
	public void Configure(EntityTypeBuilder<BioSpawnRuleAtmosphereComponent> builder)
	{
		builder.ToTable("BioSpawnRuleAtmosphereComponents");
		builder.HasKey(x => new { x.SpawnRuleId, x.AtmosphereId });
		builder.HasOne(x => x.SpawnRule).WithMany(x => x.AtmosphereComponents)
			.HasForeignKey(x => x.SpawnRuleId).OnDelete(DeleteBehavior.Cascade);
		builder.HasOne(x => x.Atmosphere).WithMany()
			.HasForeignKey(x => x.AtmosphereId).OnDelete(DeleteBehavior.Restrict);
	}
}
