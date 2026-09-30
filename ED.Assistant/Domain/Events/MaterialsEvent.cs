namespace ED.Assistant.Domain.Events;

public class MaterialsEvent : BaseJournalEvent
{
	internal const string EventName = "Materials";

	[JsonPropertyName("Raw")]
	public IEnumerable<MaterialItem>? Raw { get; set; }

	[JsonPropertyName("Manufactured")]
	public IEnumerable<MaterialItem>? Manufactured { get; set; }

	[JsonPropertyName("Encoded")]
	public IEnumerable<MaterialItem>? Encoded { get; set; }
}
