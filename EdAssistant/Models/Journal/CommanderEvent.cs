namespace EdAssistant.Models.Journal;

public sealed class CommanderEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }
    [JsonPropertyName("FID")] public string? Fid { get; set; }
    [JsonPropertyName("Name")] public string? Name { get; set; }

    public override JournalEventType EventType => JournalEventType.Commander;
}