namespace ED.Assistant.Data.Biology;

public sealed class SpeciesAtmosphereCondition
{
	public int SpeciesId { get; set; }
	public BioSpecies Species { get; set; } = null!;

	public int? AtmosphereId { get; set; }
	public Atmosphere? Atmosphere { get; set; }

	public ConditionMode Mode { get; set; }
}
