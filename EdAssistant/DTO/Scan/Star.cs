namespace EdAssistant.DTO.Scan;

public class Star : CelestialBody
{
    public StarTypeEnum? StarType { get; set; }
    public int? Subclass { get; set; }
    public double? StellarMass { get; set; }
    public double? Radius { get; set; }
    public double? AbsoluteMagnitude { get; set; }
    public int? AgeMY { get; set; }
    public double? SurfaceTemperature { get; set; }
    public string? Luminosity { get; set; }
    public List<Ring>? Rings { get; set; }
    public override string TypeInfo => $"{Localization.Instance["CelestialInfo.Star"]} ({StarType})";
    public override string MassInfo => $"{StellarMass?.ToString("N2")} {Localization.Instance["CelestialInfo.MassInfo"]}";
    public override string DistanceInfo => DistanceFromArrivalLS == 0 
        ? Localization.Instance["CelestialInfo.Primary"]
        : base.DistanceInfo;
}