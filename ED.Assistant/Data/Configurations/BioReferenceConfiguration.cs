namespace ED.Assistant.Data.Configurations;

sealed class BioReferenceConfiguration : IEntityTypeConfiguration<BioReference>
{
	public void Configure(EntityTypeBuilder<BioReference> builder)
	{
		builder.ToTable("bio_reference");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.GenusId)
			.HasColumnName("genus_id");

		builder.Property(x => x.SpeciesId)
			.HasColumnName("species_id");

		builder.Property(x => x.VariantId)
			.HasColumnName("variant_id");

		builder.Property(x => x.SourceId)
			.HasColumnName("source_id")
			.IsRequired();

		builder.Property(x => x.SourceUrl)
			.HasColumnName("source_url")
			.HasMaxLength(500)
			.IsRequired();

		builder.HasOne(x => x.Genus)
			.WithMany(x => x.References)
			.HasForeignKey(x => x.GenusId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Species)
			.WithMany(x => x.References)
			.HasForeignKey(x => x.SpeciesId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Variant)
			.WithMany(x => x.References)
			.HasForeignKey(x => x.VariantId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Source)
			.WithMany(x => x.References)
			.HasForeignKey(x => x.SourceId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasIndex(x => x.GenusId);
		builder.HasIndex(x => x.SpeciesId);
		builder.HasIndex(x => x.VariantId);
		builder.HasIndex(x => x.SourceId);
	}
}
