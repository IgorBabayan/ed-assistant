namespace EdAssistant.DTO.Scan;

public abstract partial class CelestialBody : ObservableObject
{
    [ObservableProperty]
    private IBrush? foregroundBrush;
    
    public string BodyName { get; set; } = string.Empty;
    public virtual int BodyId { get; set; }
    public string BodyType { get; set; } = string.Empty;
    public double DistanceFromArrivalLS { get; set; }
    public bool WasDiscovered { get; set; }
    public bool WasMapped { get; set; }
    public IList<CelestialBody> Children { get; set; } = new List<CelestialBody>();
    public virtual string DisplayName => BodyName;
    public virtual string TypeInfo => BodyType;
    public virtual string DistanceInfo => $"{DistanceFromArrivalLS:N1} {Localization.Instance["CelestialInfo.LS"]}";
    public virtual string StatusInfo => WasDiscovered 
        ? (WasMapped ? Localization.Instance["CelestialInfo.Mapped"] : Localization.Instance["CelestialInfo.Discovered"]) 
        : Localization.Instance["CelestialInfo.Undiscovered"];
    public virtual string LandableInfo => string.Empty;
    public virtual string MassInfo => string.Empty;
    public IEnumerable<CelestialBody> SubItems => Children;
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