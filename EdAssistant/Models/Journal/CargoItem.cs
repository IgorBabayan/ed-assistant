namespace EdAssistant.Models.Journal;

public class CargoItem
{
    [JsonPropertyName("Name")]
    public required string Name { get; set; }

    [JsonPropertyName("Count")]
    public int Count { get; set; }
}