namespace EdAssistant.Models.Scan;

public class ParentInfo
{
    [JsonPropertyName("Star")]
    public int? Star { get; set; }

    [JsonPropertyName("Planet")]
    public int? Planet { get; set; }

    [JsonPropertyName("Ring")]
    public int? Ring { get; set; }

    [JsonPropertyName("Null")]
    public int? Null { get; set; }
}