namespace EdAssistant.Models.Journal;

public class CargoItem
{
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("Count")]
    public int Count { get; set; }

    [JsonPropertyName("Stolen")]
    public int Stolen { get; set; }
}