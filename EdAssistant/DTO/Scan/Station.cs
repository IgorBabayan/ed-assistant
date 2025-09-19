namespace EdAssistant.DTO.Scan;

public abstract class Station : CelestialBody
{
    public abstract StationTypeEnum TypeEnum { get; }
    
    public override int BodyId => -1;
    public override string TypeInfo => TypeEnum.ToString();
    public override string DistanceInfo => string.Empty;
    public override string StatusInfo => string.Empty;
    public override string LandableInfo => string.Empty;
    public override string MassInfo => string.Empty;
}

public class FleetCarrier : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.FleetCarrier;
}

public class NavBeacon : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.NavBeacon;
}

public class Outpost : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.Outpost;
}

public class Asteroid : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.AsteroidBase;
}

public class Coriolis : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.Coriolis;
}

public class Orbis : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.Orbis;
}

public class Ocellus : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.Ocellus;
}

public class UnknownStation : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.Unknown;
}

public class Installation : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.Installation;
}

public class ConflictZone : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.ConflictZone;
}

public class ResourceExtraction : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.ResourceExtraction;
}

public class Carrier : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.Carrier;
}

public class USS : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.USS;
    public string? USSType { get; set; }
    public int? ThreatLevel { get; set; }
    public double? TimeRemaining { get; set; }
    
    public override string TypeInfo => 
        !string.IsNullOrEmpty(USSType) ? $"USS ({USSType})" : "USS";
    
    public override string StatusInfo => 
        ThreatLevel.HasValue ? $"Threat Level {ThreatLevel}" : base.StatusInfo;
}

public class NotableStellarPhenomena : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.NotableStellarPhenomena;
}

public class ListeningPost : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.ListeningPost;
}

public class NumberStation : Station
{
    public override StationTypeEnum TypeEnum => StationTypeEnum.NumberStation;
}