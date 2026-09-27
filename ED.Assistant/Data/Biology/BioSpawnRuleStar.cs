namespace ED.Assistant.Data.Biology;

public sealed class BioSpawnRuleStar
{
	public int SpawnRuleId { get; set; }
	public BioSpawnRule SpawnRule { get; set; } = null!;
	public int Id { get; set; }
	public StarScope Scope { get; set; }
	public string StarType { get; set; } = string.Empty;
	public string? Luminosity { get; set; }
}
