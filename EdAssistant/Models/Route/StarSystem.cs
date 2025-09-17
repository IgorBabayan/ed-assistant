namespace EdAssistant.Models.Route;

public class StarSystem
{
    [JsonPropertyName("StarSystem")]
    public required string Name { get; set; }

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("StarPos")]
    public required double[] Position { get; set; }

    [JsonPropertyName("StarClass")]
    public required string StarClass { get; set; }
}