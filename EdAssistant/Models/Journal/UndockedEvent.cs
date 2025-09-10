namespace EdAssistant.Models.Journal;

public class UndockedEvent : JournalEvent
{
    [JsonPropertyName("StationName")]
    public string StationName { get; set; }

    [JsonPropertyName("StationType")]
    public string StationType { get; set; }

    [JsonPropertyName("MarketID")]
    public long MarketID { get; set; }

    public override JournalEventType EventType => JournalEventType.Undocked;
}