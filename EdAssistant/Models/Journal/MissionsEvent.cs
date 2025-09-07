namespace EdAssistant.Models.Journal;

public sealed class MissionsEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }
    public List<JsonElement>? Active { get; set; }
    public List<JsonElement>? Failed { get; set; }
    public List<JsonElement>? Complete { get; set; }

    public override JournalEventType EventType => JournalEventType.Missions;
}