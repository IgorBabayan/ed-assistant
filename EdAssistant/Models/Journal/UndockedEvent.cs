namespace EdAssistant.Models.Journal;

public class UndockedEvent : JournalEvent
{
    [JsonPropertyName("StationName")]
    public required string StationName { get; set; }

    [JsonPropertyName("StationType")]
    public required string StationType { get; set; }

    [JsonPropertyName("MarketID")]
    public long MarketId { get; set; }

    public override JournalEventType EventType => JournalEventType.Undocked;
}