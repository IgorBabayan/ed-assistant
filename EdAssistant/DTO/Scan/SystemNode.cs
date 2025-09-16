namespace EdAssistant.DTO.Scan;

public class SystemNode : CelestialBody
{
    public long SystemAddress { get; set; }

    public override string DisplayName => BodyName;
    public override string TypeInfo => 
        $"{Localization.Instance["CelestialInfo.System"]} ({GetStarCount()} {Localization.Instance["CelestialInfo.Stars"]})";
    public override string DistanceInfo => string.Empty;
    public override string StatusInfo => $"{GetTotalBodies()} {Localization.Instance["CelestialInfo.Bodies"]}";

    public SystemNode(string systemName, long systemAddress)
    {
        BodyName = systemName;
        BodyType = Localization.Instance["CelestialInfo.System"];
        SystemAddress = systemAddress;
        DistanceFromArrivalLS = 0;
    }

    private int GetStarCount() =>
        Children.Count(c => c is Star);

    private int GetTotalBodies() =>
        Children.Sum(child => 1 + CountChildren(child.Children));

    private int CountChildren(IList<CelestialBody> children) =>
        children.Count + children.Sum(child => CountChildren(child.Children));
}