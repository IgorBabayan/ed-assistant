namespace ED.Assistant.Data.Models.Events;

public class ScanEvent : BaseJournalEvent
{
	internal const string EventName = "Scan";

	[JsonPropertyName("BodyID")]
	public int BodyId { get; set; }
}
