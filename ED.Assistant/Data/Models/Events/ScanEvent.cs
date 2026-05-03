namespace ED.Assistant.Data.Models.Events;

public class ScanEvent : BaseJournalEvent
{
	internal const string EventName = "Scan";

	[JsonPropertyName("BodyID")]
	public int BodyId { get; set; }

	[JsonPropertyName("BodyName")]
	public string BodyName { get; set; } = string.Empty;

	[JsonPropertyName("ScanType")]
	public string ScanType { get; set; } = string.Empty;

	[JsonPropertyName("StarSystem")]
	public string StarSystem { get; set; } = string.Empty;

	[JsonPropertyName("SystemAddress")]
	public long SystemAddress { get; set; }

	[JsonPropertyName("StarType")]
	public string StarType { get; set; } = string.Empty;

	[JsonPropertyName("StellarMass")]
	public double StellarMass { get; set; }

	[JsonPropertyName("Radius")]
	public double Radius { get; set; }

	[JsonPropertyName("AbsoluteMagnitude")]
	public double AbsoluteMagnitude { get; set; }

	[JsonPropertyName("Age_MY")]
	public int Age { get; set; }

	[JsonPropertyName("SurfaceTemperature")]
	public double SurfaceTemperature { get; set; }

	[JsonPropertyName("Luminosity")]
	public string Luminosity { get; set; } = string.Empty;

	[JsonPropertyName("SemiMajorAxis")]
	public double SemiMajorAxis { get; set; }

	[JsonPropertyName("Eccentricity")]
	public double Eccentricity { get; set; }

	[JsonPropertyName("OrbitalInclination")]
	public double OrbitalInclination { get; set; }

	[JsonPropertyName("Periapsis")]
	public double Periapsis { get; set; }

	[JsonPropertyName("OrbitalPeriod")]
	public double OrbitalPeriod { get; set; }

	[JsonPropertyName("AscendingNode")]
	public double AscendingNode { get; set; }

	[JsonPropertyName("MeanAnomaly")]
	public double MeanAnomaly { get; set; }

	[JsonPropertyName("RotationPeriod")]
	public double RotationPeriod { get; set; }

	[JsonPropertyName("AxialTilt")]
	public double AxialTilt { get; set; }

	[JsonPropertyName("WasDiscovered")]
	public bool WasDiscovered { get; set; }

	[JsonPropertyName("WasMapped")]
	public bool WasMapped { get; set; }

	[JsonPropertyName("WasFootfalled")]
	public bool WasFootfalled { get; set; }

	[JsonPropertyName("Parents")]
	public IEnumerable<Parent>? Parents { get; set; } = default;

	[JsonPropertyName("PlanetClass")]
	public string PlanetClass { get; set; } = string.Empty;
}
