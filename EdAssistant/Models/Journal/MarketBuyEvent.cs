namespace EdAssistant.Models.Journal;

public class MarketBuyEvent : JournalEvent
{
    [JsonPropertyName("MarketID")]
    public long MarketId { get; set; }

    [JsonPropertyName("Type")]
    public required string Type { get; set; }

    [JsonPropertyName("Count")]
    public int Count { get; set; }

    [JsonPropertyName("BuyPrice")]
    public int BuyPrice { get; set; }

    [JsonPropertyName("TotalCost")]
    public long TotalCost { get; set; }

    public override JournalEventType EventType => JournalEventType.MarketBuy;
}