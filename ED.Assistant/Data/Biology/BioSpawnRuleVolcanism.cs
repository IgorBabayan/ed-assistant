namespace ED.Assistant.Data.Biology;

public sealed class BioSpawnRuleVolcanism
{
	public int SpawnRuleId { get; set; }
	public BioSpawnRule SpawnRule { get; set; } = null!;
	public int Id { get; set; }
	public string Pattern { get; set; } = string.Empty;
	public VolcanismMatch Match { get; set; }
}
