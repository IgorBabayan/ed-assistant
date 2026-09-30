namespace ED.Assistant.Data.Configurations;

internal class AtmosphereConfiguration : IEntityTypeConfiguration<Atmosphere>
{
    public void Configure(EntityTypeBuilder<Atmosphere> builder)
    {
        builder.ToTable(nameof(Atmosphere));
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(a => a.Name).IsRequired();
    }
}