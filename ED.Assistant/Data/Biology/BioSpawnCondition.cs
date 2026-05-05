namespace ED.Assistant.Data.Biology;

public sealed class BioSpawnCondition
{
	public int Id { get; set; }

	public int? SpeciesId { get; set; }
	public int? VariantId { get; set; }

	public string? Atmosphere { get; set; }
	public string? PlanetClass { get; set; }
	public string? VolcanicActivity { get; set; }

	public double? MinTemperatureK { get; set; }
	public double? MaxTemperatureK { get; set; }

	public double? MinGravityG { get; set; }
	public double? MaxGravityG { get; set; }

	public double? MinPressureAtm { get; set; }
	public double? MaxPressureAtm { get; set; }

	public double? MinDistanceFromStarLs { get; set; }
	public double? MaxDistanceFromStarLs { get; set; }

	public string? Notes { get; set; }

	public BioSpecies? Species { get; set; }
	public BioVariant? Variant { get; set; }
}
