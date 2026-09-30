namespace ED.Assistant.Data.Configurations;

internal class VolcanismConfiguration : IEntityTypeConfiguration<Volcanism>
{
    public void Configure(EntityTypeBuilder<Volcanism> builder)
    {
        builder.ToTable(nameof(Volcanism));
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired();
    }
}