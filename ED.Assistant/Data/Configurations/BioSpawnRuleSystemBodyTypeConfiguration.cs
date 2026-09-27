namespace ED.Assistant.Data.Configurations;

sealed class BioSpawnRuleSystemBodyTypeConfiguration : IEntityTypeConfiguration<BioSpawnRuleSystemBodyType>
{
	public void Configure(EntityTypeBuilder<BioSpawnRuleSystemBodyType> builder)
	{
		builder.ToTable("BioSpawnRuleSystemBodyTypes");
		builder.HasKey(x => new { x.SpawnRuleId, x.BodyTypeId });
		builder.HasOne(x => x.SpawnRule).WithMany(x => x.SystemBodyTypes)
			.HasForeignKey(x => x.SpawnRuleId).OnDelete(DeleteBehavior.Cascade);
		builder.HasOne(x => x.BodyType).WithMany()
			.HasForeignKey(x => x.BodyTypeId).OnDelete(DeleteBehavior.Restrict);
	}
}
