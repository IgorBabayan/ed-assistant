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