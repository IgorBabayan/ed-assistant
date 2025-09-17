namespace EdAssistant.Models.Journal;

public class DockedEvent : JournalEvent
{
    [JsonPropertyName("StationName")]
    public required string StationName { get; set; }

    [JsonPropertyName("StationType")]
    public required string StationType { get; set; }

    [JsonPropertyName("StarSystem")]
    public required string StarSystem { get; set; }

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("MarketID")]
    public long MarketId { get; set; }

    [JsonPropertyName("DistFromStarLS")]
    public double DistFromStarLS { get; set; }

    public override JournalEventType EventType => JournalEventType.Docked;
}