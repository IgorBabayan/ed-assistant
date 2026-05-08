namespace ED.Assistant.Data.Biology;

public sealed class Atmosphere
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;

	public ICollection<SpeciesAtmosphereCondition> SpeciesConditions { get; set; } = [];
}
