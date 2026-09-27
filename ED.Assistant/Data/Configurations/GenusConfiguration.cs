namespace ED.Assistant.Data.Configurations;

class GenusConfiguration : IEntityTypeConfiguration<Genus>
{
    public void Configure(EntityTypeBuilder<Genus> builder)
    {
        builder.ToTable(nameof(Genus));
        
        builder.HasKey(b => b.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(b => b.Name).IsRequired();
        builder.Property(b => b.Value).IsRequired();
        builder.Property(b => b.CodexType).IsRequired();
        builder.Property(b => b.CodexName).IsRequired();
        
        builder
            .HasMany(x => x.Rules)
            .WithOne(x => x.Genus)
            .HasForeignKey(x => x.GenusId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}