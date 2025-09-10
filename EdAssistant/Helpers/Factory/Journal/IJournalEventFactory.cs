namespace EdAssistant.Helpers.Factory.Journal;

public interface IJournalEventFactory
{
    JournalEvent? CreateEvent(string json);
}