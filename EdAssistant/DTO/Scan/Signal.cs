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

public class UnknownSignal : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.UnknownSignal;
}

public class ConflictZone : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.ConflictZone;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Signal.ConflictZone"];
}
public class ResourceExtraction : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.ResourceExtraction;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Signal.ResourceExtraction"];
}

public class NavBeacon : Signal
{
    protected override SignalTypeEnum SignalType => SignalTypeEnum.NavBeacon;
    public override string TypeInfo => Localization.Instance["CelestialBodyFactory.Signal.NavBeacon"];
}