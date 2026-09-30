namespace ED.Assistant.Data.Evaluator;

public sealed class Evaluator
{
    public int Id { get; set; }
    public DateTime DateCreation { get; set; }
    public DateTime? DateSold { get; set; }
    public bool HasFirstFootStep { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal Total { get; set; }
    
    public long? SystemAddress { get; set; }
    public int? BodyId { get; set; }

    public int GenusId { get; set; }
    public Genus Genus { get; set; } = null!;
}