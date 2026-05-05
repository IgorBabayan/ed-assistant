namespace ED.Assistant.Data.Configurations;

sealed class BioSpeciesConfiguration : IEntityTypeConfiguration<BioSpecies>
{
	public void Configure(EntityTypeBuilder<BioSpecies> builder)
	{
		builder.ToTable("bio_species");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.GenusId)
			.HasColumnName("genus_id")
			.IsRequired();

		builder.Property(x => x.Name)
			.HasColumnName("name")
			.HasMaxLength(150)
			.IsRequired();

		builder.Property(x => x.DisplayName)
			.HasColumnName("display_name")
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(x => x.Description)
			.HasColumnName("description");

		builder.Property(x => x.BaseValue)
			.HasColumnName("base_value");

		builder.Property(x => x.MinScanDistanceM)
			.HasColumnName("min_scan_distance_m");

		builder.HasOne(x => x.Genus)
			.WithMany(x => x.Species)
			.HasForeignKey(x => x.GenusId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasIndex(x => x.GenusId);

		builder.HasIndex(x => new { x.GenusId, x.Name })
			.IsUnique();
	}
}
