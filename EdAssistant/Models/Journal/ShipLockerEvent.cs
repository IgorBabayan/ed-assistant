namespace EdAssistant.Models.Journal;

public sealed class ShipLockerEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }

    public List<NameCount>? Items { get; set; }
    public List<NameCount>? Components { get; set; }
    public List<NameCount>? Consumables { get; set; }
    public List<NameCount>? Data { get; set; }

    public override JournalEventType EventType => JournalEventType.ShipLocker;
}