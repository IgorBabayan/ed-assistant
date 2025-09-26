namespace EdAssistant.DTO.Scan;

public abstract class Station : CelestialBody
{
    protected abstract SignalTypeEnum StationType { get; }
    
    public override int BodyId => -1;
    public override string TypeInfo => StationType.ToString();
    public override string DistanceInfo => string.Empty;
    public override string StatusInfo => string.Empty;
    public override string LandableInfo => string.Empty;
    public override string MassInfo => string.Empty;
}

public class UnknownStation : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.Unknown;
}

public class Outpost : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.Outpost;
}

public class AsteroidBase : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.AsteroidBase;
}

public class Coriolis : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.StationCoriolis;
}

public class Orbis : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.Orbis;
}

public class Ocellus : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.Ocellus;
}

public class Installation : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.Installation;
}

public class Settlement : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.Settlement;
}

public class MegaShip : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.Megaship;
}

public class StationMegaShip : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.StationMegaShip;
}

public class FleetCarrier : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.FleetCarrier;
}

public class SquadronCarrier : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.SquadronCarrier;
}

public class Carrier : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.Carrier;
}

public class USS : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.USS;
    public string? USSType { get; set; }
    public int? ThreatLevel { get; set; }
    public double? TimeRemaining { get; set; }
    
    public override string TypeInfo => 
        !string.IsNullOrEmpty(USSType) ? $"USS ({USSType})" : "USS";
    
    public override string StatusInfo => 
        ThreatLevel.HasValue ? $"Threat Level {ThreatLevel}" : base.StatusInfo;
}

public class NumberStation : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.NumberStation;
}