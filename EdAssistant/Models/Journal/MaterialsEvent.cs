namespace EdAssistant.Models.Journal;

public class MaterialsEvent : JournalEvent
{
    [JsonPropertyName("Raw")]
    public List<Material> Raw { get; set; } = new();

    [JsonPropertyName("Manufactured")]
    public List<Material> Manufactured { get; set; } = new();

    [JsonPropertyName("Encoded")]
    public List<Material> Encoded { get; set; } = new();

    public override JournalEventType EventType => JournalEventType.Materials;
}