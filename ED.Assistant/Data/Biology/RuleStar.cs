namespace ED.Assistant.Data.Biology;

public enum RuleStarType
{
    Star = 1,
    ParentStar
}

public sealed class RuleStar
{
    public int Id { get; set; }
    
    public int RuleId { get; set; }
    public Rule Rule { get; set; } = null!;

    public int StarClassId { get; set; }
    public StarClass StarClass { get; set; } = null!;

    public string? LuminosityClass { get; set; }

    public RuleStarType Type { get; set; }
}