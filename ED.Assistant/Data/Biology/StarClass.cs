namespace ED.Assistant.Data.Biology;

internal enum StarClassEnum
{
    A = 1,
    B,
    O,
    N,
    D,
    H,
    AeBe,
    F,
    G,
    K,
    MS,
    S,
    M
}

public sealed class StarClass
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<RuleStar> RuleStars { get; set; } = [];
}