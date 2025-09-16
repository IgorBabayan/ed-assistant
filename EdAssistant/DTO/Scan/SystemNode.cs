namespace EdAssistant.DTO.Scan;

public class SystemNode : CelestialBody
{
    public long SystemAddress { get; set; }

    public override string DisplayName => BodyName;
    public override string TypeInfo => $"System ({GetStarCount()} stars)";
    public override string DistanceInfo => "";
    public override string StatusInfo => $"{GetTotalBodies()} bodies";

    public SystemNode(string systemName, long systemAddress)
    {
        BodyName = systemName;
        BodyType = "System";
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