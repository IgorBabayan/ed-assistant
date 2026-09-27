namespace ED.Assistant.Data.Configurations;

sealed class BioSpawnRuleStarConfiguration : IEntityTypeConfiguration<BioSpawnRuleStar>
{
	public void Configure(EntityTypeBuilder<BioSpawnRuleStar> builder)
	{
		builder.ToTable("BioSpawnRuleStars");
		builder.HasKey(x => x.Id);
		builder.HasOne(x => x.SpawnRule).WithMany(x => x.Stars)
			.HasForeignKey(x => x.SpawnRuleId).OnDelete(DeleteBehavior.Cascade);
		builder.Property(x => x.Scope).HasConversion<string>();
	}
}
