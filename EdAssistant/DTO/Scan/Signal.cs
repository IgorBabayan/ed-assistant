namespace EdAssistant.DTO.Scan;

public abstract class Signal : CelestialBody
{
    protected abstract SignalTypeEnum SignalType { get; }
    public override int BodyId => -1;
    public override string TypeInfo => SignalType.ToString();
    public override string DistanceInfo => string.Empty;
    public override string StatusInfo => string.Empty;
    public override string LandableInfo => string.Empty;
    public override string MassInfo => string.Empty;
}

public class ConflictZone : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.ConflictZone;
}
public class ResourceExtraction : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.ResourceExtraction;
}

public class NotableStellarPhenomena : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.NotableStellarPhenomena;
}

public class ListeningPost : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.ListeningPost;
}

public class MissionTarget : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.MissionTarget;
}

public class NavBeacon : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.NavBeacon;
}

public class ThargoidStructure : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.ThargoidStructure;
}

public class ThargoidBarnacle : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.ThargoidBarnacle;
}

public class ThargoidSite : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.ThargoidSite;
}

public class GuardianStructure : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.GuardianStructure;
}

public class GuardianSite : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.GuardianSite;
}

public class GuardianBeacon : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.GuardianBeacon;
}

public class AnomalonNebula : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.AnomalonNebula;
}

public class CrashSite : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.CrashSite;
}

public class DistressCall : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.DistressCall;
}

public class Tourist : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.Tourist;
}

public class Mining : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.Mining;
}

public class PowerPlay : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.PowerPlay;
}

public class Wreck : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.Wreck;
}

public class SalvageableWreck : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.SalvageableWreck;
}

public class Community : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.Community;
}

public class Ceremony : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.Ceremony;
}

public class PlanetarySettlement : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.PlanetarySettlement;
}

public class PlanetaryInstallation : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.PlanetaryInstallation;
}

public class PlanetaryPort : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.PlanetaryPort;
}

public class Checkpoint : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.Checkpoint;
}

public class Scenario : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.Scenario;
}

public class Combat : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.Combat;
}

public class Generic : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.Generic;
}