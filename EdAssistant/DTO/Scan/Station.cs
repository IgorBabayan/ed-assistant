namespace EdAssistant.DTO.Scan;

public abstract class Station : CelestialBody
{
    public abstract StationType Type { get; }
    
    public override int BodyId => -1;
    public override string TypeInfo => Type.ToString();
    public override string DistanceInfo => string.Empty;
    public override string StatusInfo => string.Empty;
    public override string LandableInfo => string.Empty;
    public override string MassInfo => string.Empty;
    public override string BodyType => Type.ToString();
}

public class Outpost : Station
{
    public override StationType Type => StationType.Outpost;
}

public class Asteroid : Station
{
    public override StationType Type => StationType.AsteroidBase;
}

public class Coriolis : Station
{
    public override StationType Type => StationType.Coriolis;
}

public class Orbis : Station
{
    public override StationType Type => StationType.Orbis;
}

public class Ocellus : Station
{
    public override StationType Type => StationType.Ocellus;
}

public class UnknownStation : Station
{
    public override StationType Type => StationType.Unknown;
}