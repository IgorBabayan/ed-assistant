namespace EdAssistant.Models.Scan;

public class FSSSignalDiscoveredEvent : JournalEvent
{
    public override JournalEventTypeEnum EventTypeEnum => JournalEventTypeEnum.FSSSignalDiscovered;

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("SignalName")]
    public string SignalName { get; set; } = string.Empty;

    [JsonPropertyName("SignalName_Localised")]
    public string? SignalNameLocalised { get; set; }

    [JsonPropertyName("SignalType")]
    public string SignalType { get; set; } = string.Empty;

    // Additional properties that may be present depending on signal type
    [JsonPropertyName("IsStation")]
    public bool? IsStation { get; set; }

    [JsonPropertyName("USSType")]
    public string? USSType { get; set; }

    [JsonPropertyName("USSType_Localised")]
    public string? USSTypeLocalised { get; set; }

    [JsonPropertyName("ThreatLevel")]
    public int? ThreatLevel { get; set; }

    [JsonPropertyName("TimeRemaining")]
    public double? TimeRemaining { get; set; }

    [JsonPropertyName("SpawningState")]
    public string? SpawningState { get; set; }

    [JsonPropertyName("SpawningState_Localised")]
    public string? SpawningStateLocalised { get; set; }

    [JsonPropertyName("SpawningFaction")]
    public string? SpawningFaction { get; set; }

    [JsonPropertyName("SpawningFaction_Localised")]
    public string? SpawningFactionLocalised { get; set; }
}