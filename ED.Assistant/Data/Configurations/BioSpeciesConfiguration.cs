namespace ED.Assistant.Data.Configurations;

sealed class BioSpeciesConfiguration : IEntityTypeConfiguration<BioSpecies>
{
	public void Configure(EntityTypeBuilder<BioSpecies> builder)
	{
		builder.ToTable("BioSpecies");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name).IsRequired();
		builder.Property(x => x.DisplayName).IsRequired();
		builder.Property(x => x.BaseValue).IsRequired();
		builder.Property(x => x.MinScanDistanceM).IsRequired();
		builder.Property(x => x.VariantDeterminantId).IsRequired();

		builder.HasIndex(x => x.VariantDeterminantId);

		builder.HasIndex(x => x.GenusId);

		builder.HasIndex(x => new { x.GenusId, x.Name })
			.IsUnique();
	}
}
