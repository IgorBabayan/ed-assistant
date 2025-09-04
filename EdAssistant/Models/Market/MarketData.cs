namespace EdAssistant.Models.Market;

public class MarketData
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("event")]
    public string Event { get; set; }

    [JsonPropertyName("MarketID")]
    public long MarketId { get; set; }

    [JsonPropertyName("StationName")]
    public string StationName { get; set; }

    [JsonPropertyName("StationType")]
    public string StationType { get; set; }

    [JsonPropertyName("StarSystem")]
    public string StarSystem { get; set; }

    [JsonPropertyName("Items")]
    public List<MarketItem> Items { get; set; }
}