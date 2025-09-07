namespace EdAssistant.Models.Journal;

public abstract class JournalEventBase : IJournalEvent
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonIgnore]
    public abstract JournalEventType EventType { get; }
}