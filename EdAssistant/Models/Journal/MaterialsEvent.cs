namespace EdAssistant.Models.Journal;

public sealed class MaterialsEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }

    [JsonPropertyName("Raw")] public List<NameCount>? Raw { get; set; }
    [JsonPropertyName("Manufactured")] public List<NameCount>? Manufactured { get; set; }
    [JsonPropertyName("Encoded")] public List<NameCount>? Encoded { get; set; }

    public override JournalEventType EventType => JournalEventType.Materials;
}