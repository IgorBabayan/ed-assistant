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
    protected override SignalTypeEnum StationType => SignalTypeEnum.UnknownStation;
}

public class Outpost : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.Outpost;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Station.Outpost"];
}

public class AsteroidBase : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.AsteroidBase;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Station.AsteroidBase"];
}

public class Coriolis : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.StationCoriolis;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Station.StationCoriolis"];
}

public class Orbis : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.StationONeilOrbis;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Station.StationONeilOrbis"];
}

public class Ocellus : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.StationONeilCylinder;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Station.StationONeilCylinder"];
}

public class Installation : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.Installation;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Station.Installation"];
}

public class Megaship : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.Megaship;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Station.Megaship"];
}

public class StationMegaship : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.StationMegaShip;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Station.StationMegaShip"];
}

public class FleetCarrier : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.FleetCarrier;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Station.FleetCarrier"];
}

public class SquadronCarrier : Station
{
    protected override SignalTypeEnum StationType => SignalTypeEnum.SquadronCarrier;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Station.SquadronCarrier"];
}