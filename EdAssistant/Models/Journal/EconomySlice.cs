namespace EdAssistant.Models.Journal;

public sealed class EconomySlice
{
    [JsonPropertyName("Name")] public string? Name { get; set; }
    [JsonPropertyName("Name_Localised")] public string? NameLocalised { get; set; }
    [JsonPropertyName("Proportion")] public double? Proportion { get; set; }
}