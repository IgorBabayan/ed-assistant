namespace EdAssistant.DTO.Scan;

public abstract class CelestialBody
{
    public string BodyName { get; set; } = string.Empty;
    public int BodyId { get; set; }
    public string BodyType { get; set; } = string.Empty;
    public double DistanceFromArrivalLS { get; set; }
    public bool WasDiscovered { get; set; }
    public bool WasMapped { get; set; }

    // For TreeDataGrid hierarchy
    public IList<CelestialBody> Children { get; set; } = new List<CelestialBody>();

    // Properties for TreeDataGrid columns
    public virtual string DisplayName => BodyName;
    public virtual string TypeInfo => BodyType;
    public virtual string DistanceInfo => $"{DistanceFromArrivalLS:N1} LS";
    public virtual string StatusInfo => WasDiscovered ? (WasMapped ? "Mapped" : "Discovered") : "Undiscovered";
    public virtual string LandableInfo => "";
    public virtual string MassInfo => "";

    // For HierarchicalTreeDataGridSource
    public IEnumerable<CelestialBody> SubItems => Children;

    // Optional orbital parameters
    public double? SemiMajorAxis { get; set; }
    public double? Eccentricity { get; set; }
    public double? OrbitalInclination { get; set; }
    public double? Periapsis { get; set; }
    public double? OrbitalPeriod { get; set; }
    public double? AscendingNode { get; set; }
    public double? MeanAnomaly { get; set; }
    public double? RotationPeriod { get; set; }
    public double? AxialTilt { get; set; }
}
