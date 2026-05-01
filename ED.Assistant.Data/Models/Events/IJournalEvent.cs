namespace ED.Assistant.Data.Models.Events;

public interface IJournalEvent
{
	string Event { get; }
	DateTime Timestamp { get; }
}
