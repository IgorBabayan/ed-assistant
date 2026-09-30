namespace ED.Assistant.Domain.Events;

public class CommanderEvent : BaseJournalEvent
{
	internal const string EventName = "Commander";

	[JsonPropertyName("Name")]
	public string? Name { get; set; }

	[JsonPropertyName("FID")]
	public string? FID { get; set; }
}
