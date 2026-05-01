namespace ED.Assistant.Data.Models.Events;

public interface IJournalEvent
{
	string Event { get; }
	DateTime Timestamp { get; }
}

public abstract class BaseJournalEvent : IJournalEvent
{
	[JsonPropertyName("Event")]
	public string Event { get; } = string.Empty;

	[JsonPropertyName("timestamp")]
	public DateTime Timestamp { get; } = default;
}