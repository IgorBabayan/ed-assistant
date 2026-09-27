namespace ED.Assistant.Data.Biology;

public sealed class AtmosphereComponentRule
{
    public int RuleId { get; set; }
    public Rule Rule { get; set; } = null!;

    public int AtmosphereId { get; set; }
    public Atmosphere Atmosphere { get; set; } = null!;

    public double MinPercentage { get; set; }
}