namespace ED.Assistant.Data.Models.Events;

public class MaterialsEvent : BaseJournalEvent
{
	internal const string EventName = "Materials";

	[JsonPropertyName("Raw")]
	public IEnumerable<MaterialItem>? Raw { get; set; } = default;

	[JsonPropertyName("Manufactured")]
	public IEnumerable<MaterialItem>? Manufactured { get; set; } = default;

	[JsonPropertyName("Encoded")]
	public IEnumerable<MaterialItem>? Encoded { get; set; } = default;
}
