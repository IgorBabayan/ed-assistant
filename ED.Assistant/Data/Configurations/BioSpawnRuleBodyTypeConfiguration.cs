namespace ED.Assistant.Data.Configurations;

sealed class BioSpawnRuleBodyTypeConfiguration
	: IEntityTypeConfiguration<BioSpawnRuleBodyType>
{
	public void Configure(EntityTypeBuilder<BioSpawnRuleBodyType> builder)
	{
		builder.ToTable("BioSpawnRuleBodyTypes");

		builder.HasKey(x => new { x.SpawnRuleId, x.BodyTypeId, x.Mode });

		builder.Property(x => x.Mode)
			.HasConversion<string>()
			.IsRequired();

		builder.HasOne(x => x.SpawnRule)
			.WithMany(x => x.BodyTypes)
			.HasForeignKey(x => x.SpawnRuleId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.BodyType)
			.WithMany(x => x.SpawnRules)
			.HasForeignKey(x => x.BodyTypeId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
