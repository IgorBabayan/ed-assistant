namespace ED.Assistant.Presentation.ViewModels.Dashboard;

public sealed class DashboardSignalViewModel
{
    /// <summary>Badge text, e.g. "Biological", "USS", "Carrier".</summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>Style key used by the Border.signal-badge[Tag=...] selectors.</summary>
    public string Tag { get; init; } = SignalTags.Signal;

    public string Name { get; init; } = string.Empty;

    public string Detail { get; init; } = string.Empty;

    public string Distance { get; init; } = "—";

    public int? ThreatLevel { get; init; }

    public bool HasDetail => !string.IsNullOrWhiteSpace(Detail);

    public bool HasThreat => ThreatLevel is not null;

    public bool IsHighThreat => ThreatLevel >= 3;
}

public sealed class RecentEventViewModel
{
    public string Time { get; init; } = string.Empty;

    public string FullTime { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;
}

public static class SignalTags
{
    // Body signals
    public const string Biological = nameof(Biological);
    public const string Geological = nameof(Geological);
    public const string Human = nameof(Human);
    public const string Thargoid = nameof(Thargoid);
    public const string Guardian = nameof(Guardian);
    public const string Hotspot = nameof(Hotspot);
    public const string Other = nameof(Other);

    // System signals
    public const string USS = nameof(USS);
    public const string Combat = nameof(Combat);
    public const string Resource = nameof(Resource);
    public const string Station = nameof(Station);
    public const string Carrier = nameof(Carrier);
    public const string Megaship = nameof(Megaship);
    public const string Installation = nameof(Installation);
    public const string Beacon = nameof(Beacon);
    public const string Titan = nameof(Titan);
    public const string Signal = nameof(Signal);
}