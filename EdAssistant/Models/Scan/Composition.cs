namespace EdAssistant.Models.Scan;

public class Composition
{
    [JsonPropertyName("Ice")]
    public double Ice { get; set; }

    [JsonPropertyName("Rock")]
    public double Rock { get; set; }

    [JsonPropertyName("Metal")]
    public double Metal { get; set; }
}