namespace ED.Assistant.Data.Biology;

public sealed class BioSpawnRuleAtmosphere
{
	public int SpawnRuleId { get; set; }
	public BioSpawnRule SpawnRule { get; set; } = null!;
	public int AtmosphereId { get; set; }
	public Atmosphere Atmosphere { get; set; } = null!;
	public ConditionMode Mode { get; set; }
}
