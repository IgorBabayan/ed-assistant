namespace ED.Assistant.Data.Seed;

internal sealed class BioCatalog
{
	public int SchemaVersion { get; set; }
	public string SourceCommit { get; set; } = string.Empty;
	public List<CatalogLookup> BodyTypes { get; set; } = [];
	public List<CatalogLookup> Atmospheres { get; set; } = [];
	public List<CatalogSpecies> Species { get; set; } = [];
}

internal sealed class CatalogLookup
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
}

internal sealed class CatalogSpecies
{
	public string JournalName { get; set; } = string.Empty;
	public string GenusJournalName { get; set; } = string.Empty;
	public string GenusName { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public int BaseValue { get; set; }
	public int? MinScanDistanceM { get; set; }
	public string VariantDeterminant { get; set; } = string.Empty;
	public string SourceFile { get; set; } = string.Empty;
	public List<CatalogRule> Rules { get; set; } = [];
}

internal sealed class CatalogRule
{
	public int SourceIndex { get; set; }
	public double? MinTemperatureK { get; set; }
	public double? MaxTemperatureK { get; set; }
	public double? MinGravityG { get; set; }
	public double? MaxGravityG { get; set; }
	public double? MinPressureAtmospheres { get; set; }
	public double? MaxPressureAtmospheres { get; set; }
	public double? MaxOrbitalPeriodSeconds { get; set; }
	public double? MinArrivalDistanceLs { get; set; }
	public string? Nebula { get; set; }
	public VolcanismMode VolcanismMode { get; set; }
	public List<int> BodyTypeIds { get; set; } = [];
	public List<int> SystemBodyTypeIds { get; set; } = [];
	public List<int> AtmosphereIds { get; set; } = [];
	public List<CatalogComponent> AtmosphereComponents { get; set; } = [];
	public List<CatalogVolcanism> VolcanismPatterns { get; set; } = [];
	public List<CatalogStar> Stars { get; set; } = [];
}

internal sealed class CatalogComponent
{
	public int AtmosphereId { get; set; }
	public double MinPercent { get; set; }
}

internal sealed class CatalogVolcanism
{
	public string Pattern { get; set; } = string.Empty;
	public VolcanismMatch Match { get; set; }
}

internal sealed class CatalogStar
{
	public StarScope Scope { get; set; }
	public string StarType { get; set; } = string.Empty;
	public string? Luminosity { get; set; }
}
