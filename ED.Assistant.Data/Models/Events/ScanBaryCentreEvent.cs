namespace ED.Assistant.Data.Models.Events;

public class ScanBaryCentreEvent : BaseJournalEvent
{
	internal const string EventName = "ScanBaryCentre";

	[JsonPropertyName("BodyID")]
	public int BodyId { get; set; }
}
