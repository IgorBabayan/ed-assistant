namespace ED.Assistant.Data.Biology;

public sealed class ParentBodyClass
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Rule> Rules { get; set; } = [];
}