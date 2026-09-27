namespace ED.Assistant.Data.Biology;

/// <summary>All conditions within one rule are AND; a species' rules are OR alternatives.</summary>
public sealed class BioSpawnRule
{
	public int Id { get; set; }
	public int SpeciesId { get; set; }
	public BioSpecies Species { get; set; } = null!;

	// Kept only to preserve pre-import/custom reference data during migration.
	// Imported rules use the normalized conditions below, not these legacy strings.
	public string AtmosphereRaw { get; set; } = string.Empty;
	public string VolcanismRaw { get; set; } = string.Empty;

	public string? SourceFile { get; set; }
	public int? SourceIndex { get; set; }
	public double? MinTemperatureK { get; set; }
	public double? MaxTemperatureK { get; set; }
	// EDMC-BioScan uses journal gravity / 9.797759 (not standard Earth g).
	public double? MinGravityG { get; set; }
	public double? MaxGravityG { get; set; }
	// EDMC-BioScan uses journal pressure / 101231.656250 Pa.
	public double? MinPressureAtmospheres { get; set; }
	public double? MaxPressureAtmospheres { get; set; }
	public double? MaxOrbitalPeriodSeconds { get; set; }
	public double? MinArrivalDistanceLs { get; set; }
	// Temperature/gravity and minimum pressure are inclusive; these two maxima are exclusive.
	public bool MaxPressureExclusive { get; set; } = true;
	public bool MaxOrbitalPeriodExclusive { get; set; } = true;
	// "all" includes planetary nebulae; null means no nebula requirement.
	public string? Nebula { get; set; }
	public VolcanismMode VolcanismMode { get; set; }

	public ICollection<BioSpawnRuleBodyType> BodyTypes { get; set; } = [];
	public ICollection<BioSpawnRuleSystemBodyType> SystemBodyTypes { get; set; } = [];
	public ICollection<BioSpawnRuleAtmosphere> Atmospheres { get; set; } = [];
	public ICollection<BioSpawnRuleAtmosphereComponent> AtmosphereComponents { get; set; } = [];
	public ICollection<BioSpawnRuleVolcanism> VolcanismPatterns { get; set; } = [];
	public ICollection<BioSpawnRuleStar> Stars { get; set; } = [];
}
