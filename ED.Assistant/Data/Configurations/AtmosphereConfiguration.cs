namespace ED.Assistant.Data.Configurations;

sealed class AtmosphereConfiguration : IEntityTypeConfiguration<Atmosphere>
{
	public void Configure(EntityTypeBuilder<Atmosphere> builder)
	{
		builder.ToTable("Atmospheres");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired();

		builder.HasIndex(x => x.Name)
			.IsUnique();
	}
}
