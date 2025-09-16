namespace EdAssistant.DTO.Scan;

public class Star : CelestialBody
{
    public StarType? StarType { get; set; }
    public int? Subclass { get; set; }
    public double? StellarMass { get; set; }
    public double? Radius { get; set; }
    public double? AbsoluteMagnitude { get; set; }
    public int? AgeMY { get; set; }
    public double? SurfaceTemperature { get; set; }
    public string? Luminosity { get; set; }
    public List<Ring>? Rings { get; set; }
    public override string TypeInfo => $"Star ({StarType})";
    public override string MassInfo => StellarMass?.ToString("N2") + " M☉" ?? "";
    public override string DistanceInfo => DistanceFromArrivalLS == 0 ? "Primary" : base.DistanceInfo;
}