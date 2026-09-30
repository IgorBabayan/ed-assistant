namespace ED.Assistant.Data.Configurations;

internal class ParentBodyClassConfiguration : IEntityTypeConfiguration<ParentBodyClass>
{
    public void Configure(EntityTypeBuilder<ParentBodyClass> builder)
    {
        builder.ToTable(nameof(ParentBodyClass));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}