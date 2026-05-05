namespace ED.Assistant.Data.Biology;

public sealed class BioVariant
{
	public int Id { get; set; }

	public int SpeciesId { get; set; }

	public string Name { get; set; } = string.Empty;
	public string DisplayName { get; set; } = string.Empty;

	public string? ColorName { get; set; }
	public string? ColorHex { get; set; }
	public string? ImageUrl { get; set; }

	public BioSpecies? Species { get; set; }

	public List<BioVariantRule> Rules { get; set; } = [];
	public List<BioSpawnCondition> SpawnConditions { get; set; } = [];
	public List<BioReference> References { get; set; } = [];
}
