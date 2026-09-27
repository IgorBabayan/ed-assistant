namespace ED.Assistant.Data.Biology;

enum StarClassEnum
{
    A,
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
    S
}

public sealed class StarClass
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<RuleStar> RuleStars { get; set; } = [];
}