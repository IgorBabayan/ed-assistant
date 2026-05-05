namespace ED.Assistant.Data.Configurations;

sealed class BioSourceConfiguration : IEntityTypeConfiguration<BioSource>
{
	public void Configure(EntityTypeBuilder<BioSource> builder)
	{
		builder.ToTable("bio_source");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.HasColumnName("name")
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(x => x.Url)
			.HasColumnName("url")
			.HasMaxLength(500);

		builder.Property(x => x.Notes)
			.HasColumnName("notes");

		builder.HasIndex(x => x.Name)
			.IsUnique();
	}
}
