namespace EdAssistant.Models.Cargo;

public class InventoryItem
{
    [JsonPropertyName("Name")]
    public required string Name { get; set; }

    [JsonPropertyName("Count")]
    public int Count { get; set; }

    [JsonPropertyName("Stolen")]
    public int Stolen { get; set; }
}