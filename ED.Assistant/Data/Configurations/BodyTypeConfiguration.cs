namespace ED.Assistant.Data.Configurations;

sealed class BodyTypeConfiguration : IEntityTypeConfiguration<BodyType>
{
	public void Configure(EntityTypeBuilder<BodyType> builder)
	{
		builder.ToTable("BodyTypes");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired();

		builder.HasIndex(x => x.Name)
			.IsUnique();
	}
}
