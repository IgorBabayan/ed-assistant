namespace ED.Assistant.Data.Configurations;

sealed class BioSpawnRuleVolcanismConfiguration : IEntityTypeConfiguration<BioSpawnRuleVolcanism>
{
	public void Configure(EntityTypeBuilder<BioSpawnRuleVolcanism> builder)
	{
		builder.ToTable("BioSpawnRuleVolcanisms");
		builder.HasKey(x => x.Id);
		builder.HasOne(x => x.SpawnRule).WithMany(x => x.VolcanismPatterns)
			.HasForeignKey(x => x.SpawnRuleId).OnDelete(DeleteBehavior.Cascade);
		builder.Property(x => x.Match).HasConversion<string>();
	}
}
