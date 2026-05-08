namespace ED.Assistant.Data.Configurations;

sealed class VariantDeterminantConfiguration : IEntityTypeConfiguration<VariantDeterminant>
{
	public void Configure(EntityTypeBuilder<VariantDeterminant> builder)
	{
		builder.ToTable("VariantDeterminants");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired();

		builder.HasIndex(x => x.Name)
			.IsUnique();

		builder.HasMany(x => x.Species)
			.WithOne(x => x.VariantDeterminant)
			.HasForeignKey(x => x.VariantDeterminantId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
