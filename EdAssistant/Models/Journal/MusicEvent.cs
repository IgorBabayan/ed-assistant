namespace EdAssistant.Models.Journal;

public sealed class MusicEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }
    public string? MusicTrack { get; set; }

    public override JournalEventType EventType => JournalEventType.Music;
}