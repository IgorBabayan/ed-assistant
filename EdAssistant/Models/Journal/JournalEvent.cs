namespace EdAssistant.Models.Journal;

public abstract class JournalEvent
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("event")]
    public required string Event { get; set; }

    public abstract JournalEventType EventType { get; }
}