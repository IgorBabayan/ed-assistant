namespace ED.Assistant.Data.Configurations;

sealed class BioCatalogVersionConfiguration : IEntityTypeConfiguration<BioCatalogVersion>
{
	public void Configure(EntityTypeBuilder<BioCatalogVersion> builder)
	{
		builder.ToTable("BioCatalogVersions");
		builder.HasKey(x => x.Id);
	}
}
