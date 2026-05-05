namespace ED.Assistant.Data.Biology;

public sealed class BioSpecies
{
	public int Id { get; set; }

	public int GenusId { get; set; }

	public string Name { get; set; } = string.Empty;
	public string DisplayName { get; set; } = string.Empty;
	public string? Description { get; set; }

	public int? BaseValue { get; set; }
	public int? MinScanDistanceM { get; set; }

	public BioGenus? Genus { get; set; }

	public List<BioVariant> Variants { get; set; } = [];
	public List<BioSpawnCondition> SpawnConditions { get; set; } = [];
	public List<BioReference> References { get; set; } = [];
}
