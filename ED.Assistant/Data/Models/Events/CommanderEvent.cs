namespace ED.Assistant.Data.Models.Events;

public class CommanderEvent : BaseJournalEvent
{
	internal const string EventName = "Commander";

	[JsonPropertyName("Name")]
	public string? Name { get; set; } = default;

	[JsonPropertyName("FID")]
	public string? FID { get; set; } = default;
}
