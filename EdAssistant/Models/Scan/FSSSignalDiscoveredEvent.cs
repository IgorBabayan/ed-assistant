namespace EdAssistant.Models.Scan;

public class FSSSignalDiscoveredEvent : JournalEvent
{
    public override JournalEventType EventType => JournalEventType.FSSSignalDiscovered;

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("SignalName")]
    public required string SignalName { get; set; }

    [JsonPropertyName("SignalType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StationType SignalType { get; set; }
}