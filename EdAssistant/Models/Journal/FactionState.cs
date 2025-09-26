namespace EdAssistant.Models.Journal;

public class FactionState
{
    [JsonPropertyName("State")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("Trend")]
    public int? Trend { get; set; }
}