namespace EdAssistant.Models.Journal;

public class Engineer
{
    [JsonPropertyName("Engineer")]
    public string Name { get; set; }

    [JsonPropertyName("EngineerID")]
    public int EngineerID { get; set; }

    [JsonPropertyName("Progress")]
    public string Progress { get; set; }

    [JsonPropertyName("RankProgress")]
    public int RankProgress { get; set; }

    [JsonPropertyName("Rank")]
    public int Rank { get; set; }
}