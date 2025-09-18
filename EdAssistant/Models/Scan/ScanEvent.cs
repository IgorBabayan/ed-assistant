namespace EdAssistant.Models.Scan;

public class ScanEvent : JournalEvent
{
    public override JournalEventTypeEnum EventTypeEnum => JournalEventTypeEnum.Scan;

    [JsonPropertyName("ScanType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ScanTypeEnum ScanTypeEnum { get; set; }

    [JsonPropertyName("BodyName")]
    public string BodyName { get; set; } = string.Empty;

    [JsonPropertyName("BodyId")]
    public int BodyId { get; set; }

    [JsonPropertyName("BodyType")]
    public string? BodyType { get; set; }

    [JsonPropertyName("Parents")]
    public List<ParentInfo>? Parents { get; set; }

    [JsonPropertyName("StarSystem")]
    public string StarSystem { get; set; } = string.Empty;

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("DistanceFromArrivalLS")]
    public double DistanceFromArrivalLS { get; set; }

    [JsonPropertyName("StarType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StarTypeEnum? StarType { get; set; }

    [JsonPropertyName("Subclass")]
    public int? Subclass { get; set; }

    [JsonPropertyName("StellarMass")]
    public double? StellarMass { get; set; }

    [JsonPropertyName("Radius")]
    public double? Radius { get; set; }

    [JsonPropertyName("AbsoluteMagnitude")]
    public double? AbsoluteMagnitude { get; set; }

    [JsonPropertyName("Age_MY")]
    public int? AgeMY { get; set; }

    [JsonPropertyName("SurfaceTemperature")]
    public double? SurfaceTemperature { get; set; }

    [JsonPropertyName("Luminosity")]
    public string? Luminosity { get; set; }

    [JsonPropertyName("TidalLock")]
    public bool? TidalLock { get; set; }

    [JsonPropertyName("TerraformState")]
    public string? TerraformState { get; set; }

    [JsonPropertyName("PlanetClass")]
    public string? PlanetClass { get; set; }

    [JsonPropertyName("Atmosphere")]
    public string? Atmosphere { get; set; }

    [JsonPropertyName("AtmosphereType")]
    public string? AtmosphereType { get; set; }

    [JsonPropertyName("AtmosphereComposition")]
    public List<AtmosphereComposition>? AtmosphereComposition { get; set; }

    [JsonPropertyName("Volcanism")]
    public string? Volcanism { get; set; }

    [JsonPropertyName("MassEM")]
    public double? MassEM { get; set; }

    [JsonPropertyName("SurfaceGravity")]
    public double? SurfaceGravity { get; set; }

    [JsonPropertyName("SurfacePressure")]
    public double? SurfacePressure { get; set; }

    [JsonPropertyName("Landable")]
    public bool? Landable { get; set; }

    [JsonPropertyName("Materials")]
    public List<Material>? Materials { get; set; }

    [JsonPropertyName("Composition")]
    public Composition? Composition { get; set; }

    [JsonPropertyName("SemiMajorAxis")]
    public double? SemiMajorAxis { get; set; }

    [JsonPropertyName("Eccentricity")]
    public double? Eccentricity { get; set; }

    [JsonPropertyName("OrbitalInclination")]
    public double? OrbitalInclination { get; set; }

    [JsonPropertyName("Periapsis")]
    public double? Periapsis { get; set; }

    [JsonPropertyName("OrbitalPeriod")]
    public double? OrbitalPeriod { get; set; }

    [JsonPropertyName("AscendingNode")]
    public double? AscendingNode { get; set; }

    [JsonPropertyName("MeanAnomaly")]
    public double? MeanAnomaly { get; set; }

    [JsonPropertyName("RotationPeriod")]
    public double? RotationPeriod { get; set; }

    [JsonPropertyName("AxialTilt")]
    public double? AxialTilt { get; set; }

    [JsonPropertyName("Rings")]
    public List<RingInfo>? Rings { get; set; }

    [JsonPropertyName("ReserveLevel")]
    public string? ReserveLevel { get; set; }

    [JsonPropertyName("WasDiscovered")]
    public bool WasDiscovered { get; set; }

    [JsonPropertyName("WasMapped")]
    public bool WasMapped { get; set; }
}
