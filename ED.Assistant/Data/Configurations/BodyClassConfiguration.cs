namespace ED.Assistant.Data.Configurations;

class BodyClassConfiguration : IEntityTypeConfiguration<BodyClass>
{
    public void Configure(EntityTypeBuilder<BodyClass> builder)
    {
        builder.ToTable(nameof(BodyClass));
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(b => b.Name).IsRequired();
    }
}