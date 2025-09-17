namespace EdAssistant.Models.Journal;

public class Material
{
    [JsonPropertyName("Name")]
    public required string Name { get; set; }

    [JsonPropertyName("Name_Localised")]
    public required string NameLocalised { get; set; }

    [JsonPropertyName("Count")]
    public int Count { get; set; }
}