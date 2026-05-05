namespace ED.Assistant.Data.Configurations;

sealed class BioVariantConfiguration : IEntityTypeConfiguration<BioVariant>
{
	public void Configure(EntityTypeBuilder<BioVariant> builder)
	{
		builder.ToTable("bio_variant");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.SpeciesId)
			.HasColumnName("species_id")
			.IsRequired();

		builder.Property(x => x.Name)
			.HasColumnName("name")
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(x => x.DisplayName)
			.HasColumnName("display_name")
			.HasMaxLength(150)
			.IsRequired();

		builder.Property(x => x.ColorName)
			.HasColumnName("color_name")
			.HasMaxLength(100);

		builder.Property(x => x.ColorHex)
			.HasColumnName("color_hex")
			.HasMaxLength(20);

		builder.Property(x => x.ImageUrl)
			.HasColumnName("image_url")
			.HasMaxLength(500);

		builder.HasOne(x => x.Species)
			.WithMany(x => x.Variants)
			.HasForeignKey(x => x.SpeciesId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasIndex(x => x.SpeciesId);

		builder.HasIndex(x => new { x.SpeciesId, x.Name })
			.IsUnique();
	}
}
