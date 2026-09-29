namespace ED.Assistant.Data.Evaluator;

public sealed class Evaluator
{
    public int Id { get; set; }
    public DateTime DateCreation { get; set; }
    public bool HasFirstFootStep { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal Total { get; set; }

    public int GenusId { get; set; }
    public Genus Genus { get; set; } = null!;
}