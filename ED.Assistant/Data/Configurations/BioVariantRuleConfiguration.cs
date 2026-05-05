namespace ED.Assistant.Data.Configurations;

sealed class BioVariantRuleConfiguration : IEntityTypeConfiguration<BioVariantRule>
{
	public void Configure(EntityTypeBuilder<BioVariantRule> builder)
	{
		builder.ToTable("bio_variant_rule");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.VariantId)
			.HasColumnName("variant_id")
			.IsRequired();

		builder.Property(x => x.StarClass)
			.HasColumnName("star_class")
			.HasMaxLength(20);

		builder.Property(x => x.MaterialName)
			.HasColumnName("material_name")
			.HasMaxLength(100);

		builder.Property(x => x.RegionName)
			.HasColumnName("region_name")
			.HasMaxLength(150);

		builder.Property(x => x.Notes)
			.HasColumnName("notes");

		builder.HasOne(x => x.Variant)
			.WithMany(x => x.Rules)
			.HasForeignKey(x => x.VariantId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasIndex(x => x.VariantId);
		builder.HasIndex(x => x.StarClass);
		builder.HasIndex(x => x.MaterialName);
	}
}
