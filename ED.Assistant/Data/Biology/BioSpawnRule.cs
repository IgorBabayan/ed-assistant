namespace ED.Assistant.Data.Biology;

public sealed class BioSpawnRule
{
	public int Id { get; set; }

	public int SpeciesId { get; set; }
	public BioSpecies Species { get; set; } = null!;

	public string AtmosphereRaw { get; set; } = string.Empty;
	public string VolcanismRaw { get; set; } = string.Empty;

	public ICollection<BioSpawnRuleBodyType> BodyTypes { get; set; } = [];
}