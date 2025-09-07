namespace EdAssistant.Models.Journal;

public sealed class CarrierLocationEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }
    public string? CarrierType { get; set; }
    public long CarrierID { get; set; }
    public string? StarSystem { get; set; }
    public long SystemAddress { get; set; }
    public int BodyID { get; set; }

    public override JournalEventType EventType => JournalEventType.CarrierLocation;
}