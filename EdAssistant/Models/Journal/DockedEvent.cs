namespace EdAssistant.Models.Journal;

public class DockedEvent : JournalEvent
{
    [JsonPropertyName("StationName")]
    public string StationName { get; set; }

    [JsonPropertyName("StationType")]
    public string StationType { get; set; }

    [JsonPropertyName("StarSystem")]
    public string StarSystem { get; set; }

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("MarketID")]
    public long MarketID { get; set; }

    [JsonPropertyName("DistFromStarLS")]
    public double DistFromStarLS { get; set; }

    public override JournalEventType EventType => JournalEventType.Docked;
}