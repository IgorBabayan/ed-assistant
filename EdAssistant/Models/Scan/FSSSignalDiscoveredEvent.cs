namespace EdAssistant.Models.Scan;

public class FSSSignalDiscoveredEvent : JournalEvent
{
    public override JournalEventTypeEnum EventTypeEnum => JournalEventTypeEnum.FSSSignalDiscovered;

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("SignalName")]
    public required string SignalName { get; set; }

    [JsonPropertyName("SignalType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StationTypeEnum SignalTypeEnum { get; set; }
}