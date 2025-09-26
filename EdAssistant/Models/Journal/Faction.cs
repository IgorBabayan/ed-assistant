namespace EdAssistant.Models.Journal;

public class Faction
{
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("FactionState")]
    public string? FactionState { get; set; }

    [JsonPropertyName("Government")]
    public string? Government { get; set; }

    [JsonPropertyName("Influence")]
    public double Influence { get; set; }

    [JsonPropertyName("Allegiance")]
    public string? Allegiance { get; set; }

    [JsonPropertyName("Happiness")]
    public string? Happiness { get; set; }

    [JsonPropertyName("Happiness_Localised")]
    public string? HappinessLocalised { get; set; }

    [JsonPropertyName("MyReputation")]
    public double MyReputation { get; set; }

    [JsonPropertyName("PendingStates")]
    public List<FactionState>? PendingStates { get; set; }

    [JsonPropertyName("ActiveStates")]
    public List<FactionState>? ActiveStates { get; set; }
}