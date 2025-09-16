namespace EdAssistant.DTO.Scan;

public class Ring : CelestialBody
{
    public string RingClass { get; set; } = string.Empty;
    public double MassMT { get; set; }
    public double InnerRad { get; set; }
    public double OuterRad { get; set; }
    public override string TypeInfo => $"{Localization.Instance["CelestialInfo.Ring"]} ({RingClass})";
    public override string MassInfo => $"{MassMT:N0} {Localization.Instance["CelestialInfo.RingMassInfo"]}";
}