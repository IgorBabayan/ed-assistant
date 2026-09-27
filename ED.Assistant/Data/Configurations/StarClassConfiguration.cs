namespace ED.Assistant.Data.Configurations;

class StarClassConfiguration : IEntityTypeConfiguration<StarClass>
{
    public void Configure(EntityTypeBuilder<StarClass> builder)
    {
        builder.ToTable(nameof(StarClass));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(32);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}