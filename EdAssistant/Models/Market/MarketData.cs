namespace EdAssistant.Models.Market;

public class MarketData
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("event")]
    public required string Event { get; set; }

    [JsonPropertyName("MarketID")]
    public long MarketId { get; set; }

    [JsonPropertyName("StationName")]
    public required string StationName { get; set; }

    [JsonPropertyName("StationType")]
    public required string StationType { get; set; }

    [JsonPropertyName("StarSystem")]
    public required string StarSystem { get; set; }

    [JsonPropertyName("Items")]
    public required List<MarketItem> Items { get; set; }
}