namespace ED.Assistant.Domain.Events;

public interface IJournalEvent
{
	string Event { get; set; }
	DateTime Timestamp { get; set; }
}

public abstract class BaseJournalEvent : IJournalEvent
{
	[JsonPropertyName("event")]
	public string Event { get; set; } = string.Empty;

	[JsonPropertyName("timestamp")]
	public DateTime Timestamp { get; set; } = default;
}