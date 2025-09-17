namespace EdAssistant.Models.Cargo;

public class CargoEvent
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("event")]
    public required string Event { get; set; }

    [JsonPropertyName("Vessel")]
    public required string Vessel { get; set; }

    [JsonPropertyName("Count")]
    public int Count { get; set; }

    [JsonPropertyName("Inventory")]
    public List<InventoryItem> Inventory { get; set; } = new();
}