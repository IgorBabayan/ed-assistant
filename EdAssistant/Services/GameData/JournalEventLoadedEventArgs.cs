namespace EdAssistant.Services.GameData;

public class JournalEventLoadedEventArgs(JournalEvent journalEvent) : EventArgs
{
    public JournalEvent Event { get; } = journalEvent;
    public JournalEventType EventType => Event.EventType;
}