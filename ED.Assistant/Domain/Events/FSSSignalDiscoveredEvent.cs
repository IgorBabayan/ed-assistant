namespace ED.Assistant.Domain.Events;

public sealed class FSSSignalDiscoveredEvent : BaseJournalEvent
{
    internal const string EventName = "FSSSignalDiscovered";

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("SignalName")]
    public string SignalNameId { get; set; } = string.Empty;

    [JsonPropertyName("SignalName_Localised")]
    public string SignalName { get; set; } = string.Empty;

    /// <summary>
    /// Signal category, e.g. "USS", "FleetCarrier", "StationCoriolis", "Outpost",
    /// "ResourceExtraction", "Combat", "NavBeacon", "Megaship", "Installation", "Titan".
    /// Missing in older journals.
    /// </summary>
    [JsonPropertyName("SignalType")]
    public string SignalKind { get; set; } = string.Empty;

    [JsonPropertyName("IsStation")]
    public bool IsStation { get; set; }

    [JsonPropertyName("USSType")]
    public string USSTypeId { get; set; } = string.Empty;

    [JsonPropertyName("USSType_Localised")]
    public string USSType { get; set; } = string.Empty;

    [JsonPropertyName("SpawningFaction")]
    public string SpawningFaction { get; set; } = string.Empty;

    [JsonPropertyName("SpawningState_Localised")]
    public string SpawningState { get; set; } = string.Empty;

    [JsonPropertyName("ThreatLevel")]
    public int? ThreatLevel { get; set; }

    /// <summary>Seconds left before a temporary signal (USS) disappears.</summary>
    [JsonPropertyName("TimeRemaining")]
    public double? TimeRemaining { get; set; }

    /// <summary>
    /// The game repeats these events after a relog in the same system,
    /// so identical signals are collapsed into one entry.
    /// </summary>
    public string Key => $"{SystemAddress}:{SignalKind}:{SignalNameId}:{USSTypeId}:{ThreatLevel}";
}