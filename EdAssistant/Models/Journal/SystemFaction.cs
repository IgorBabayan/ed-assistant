namespace EdAssistant.Models.Journal;

public class SystemFaction
{
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("FactionState")]
    public string? FactionState { get; set; }
}