/*namespace EdAssistant.DTO.Scan;

public partial class SystemNode : CelestialBody
{
    public SystemNode(string systemName, long systemAddress)
    {
        BodyName = systemName;
        BodyType = "System";
        SystemAddress = systemAddress;
        DistanceFromArrivalLS = 0;
    }

    public long SystemAddress { get; set; }

    public override string DisplayName => BodyName;
    public override string TypeInfo => $"System ({GetStarCount()} stars)";
    public override string DistanceInfo => "";
    public override string StatusInfo => $"{GetTotalBodies()} bodies";

    private int GetStarCount()
    {
        return Children.Count(c => c is Star);
    }

    private int GetTotalBodies()
    {
        int count = 0;
        foreach (var child in Children)
        {
            count += 1 + CountChildren(child.Children);
        }
        return count;
    }

    private int CountChildren(IList<CelestialBody> children)
    {
        int count = children.Count;
        foreach (var child in children)
        {
            count += CountChildren(child.Children);
        }
        return count;
    }
}*/