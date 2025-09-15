namespace EdAssistant.Models.Scan;

public class Material
{
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("Name_Localised")]
    public string? Name_Localised { get; set; }

    [JsonPropertyName("Percent")]
    public double Percent { get; set; }
}