namespace EdAssistant.Models.Journal;

public class Engineer
{
    [JsonPropertyName("Engineer")]
    public required string Name { get; set; }

    [JsonPropertyName("EngineerID")]
    public int EngineerId { get; set; }

    [JsonPropertyName("Progress")]
    public required string Progress { get; set; }

    [JsonPropertyName("RankProgress")]
    public int RankProgress { get; set; }

    [JsonPropertyName("Rank")]
    public int Rank { get; set; }
}