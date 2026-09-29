namespace ED.Assistant.Data.Configurations;

class EvaluatorConfiguration : IEntityTypeConfiguration<Evaluator.Evaluator>
{
    public void Configure(EntityTypeBuilder<Evaluator.Evaluator> builder)
    {
        builder.ToTable(nameof(Evaluator.Evaluator));
        builder.HasKey(ev => ev.Id);
        builder.Property(ev => ev.DateCreation).IsRequired();
        builder.Property(ev => ev.HasFirstFootStep).IsRequired();
        builder.Property(ev => ev.Total).IsRequired();
        builder.Property(ev => ev.IsActive).IsRequired();
        
        builder.HasQueryFilter(x => x.IsActive);
        
        builder.Property(ev => ev.DateSold);
        builder.HasIndex(ev => new { ev.GenusId, ev.DateCreation });
    }
}