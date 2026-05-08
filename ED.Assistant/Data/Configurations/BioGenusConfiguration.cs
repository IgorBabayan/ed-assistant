namespace ED.Assistant.Data.Configurations;

sealed class BioGenusConfiguration : IEntityTypeConfiguration<BioGenus>
{
	public void Configure(EntityTypeBuilder<BioGenus> builder)
	{
		builder.ToTable("BioGenera");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired();

		builder.Property(x => x.DisplayName)
			.IsRequired();

		builder.HasIndex(x => x.Name)
			.IsUnique();

		builder.HasMany(x => x.Species)
			.WithOne(x => x.Genus)
			.HasForeignKey(x => x.GenusId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
