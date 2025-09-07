namespace EdAssistant.Models.Journal;

public interface IJournalEvent
{
    DateTime Timestamp { get; }
    JournalEventType EventType { get; }
}