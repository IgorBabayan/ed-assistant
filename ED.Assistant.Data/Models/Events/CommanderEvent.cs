namespace ED.Assistant.Data.Models.Events;

public class CommanderEvent : IJournalEvent
{
	internal const string EventName = "Commander";

	[JsonPropertyName("Event")]
	public string Event { get; set; } = string.Empty;

	[JsonPropertyName("Name")]
	public string? Name { get; set; } = default;

	[JsonPropertyName("FID")]
	public string? FID { get; set; } = default;

	[JsonPropertyName("timestamp")]
	public DateTime Timestamp { get; set; } = default;
}
