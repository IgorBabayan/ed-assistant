namespace ED.Assistant.Data.Biology;

public sealed class BioSpawnRuleBodyType
{
	public int SpawnRuleId { get; set; }
	public BioSpawnRule SpawnRule { get; set; } = null!;

	public int BodyTypeId { get; set; }
	public BodyType BodyType { get; set; } = null!;

	public ConditionMode Mode { get; set; }
}
