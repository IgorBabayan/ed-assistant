namespace EdAssistant.Models.Scan;

public class AtmosphereComposition
{
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("Percent")]
    public double Percent { get; set; }
}