namespace ED.Assistant.Data.Biology;

public sealed class BodyType
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;

	public ICollection<BioSpawnRuleBodyType> SpawnRules { get; set; } = [];
}
