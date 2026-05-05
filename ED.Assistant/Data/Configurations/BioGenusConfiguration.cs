namespace ED.Assistant.Data.Configurations;

sealed class BioGenusConfiguration : IEntityTypeConfiguration<BioGenus>
{
	public void Configure(EntityTypeBuilder<BioGenus> builder)
	{
		builder.ToTable("bio_genus");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.HasColumnName("name")
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(x => x.DisplayName)
			.HasColumnName("display_name")
			.HasMaxLength(150)
			.IsRequired();

		builder.Property(x => x.Description)
			.HasColumnName("description");

		builder.HasIndex(x => x.Name).IsUnique();
	}
}
