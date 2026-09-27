namespace ED.Assistant.Data.Biology;

public sealed class BioSpawnRuleAtmosphereComponent
{
	public int SpawnRuleId { get; set; }
	public BioSpawnRule SpawnRule { get; set; } = null!;
	public int AtmosphereId { get; set; }
	public Atmosphere Atmosphere { get; set; } = null!;
	public double MinPercent { get; set; }
}
