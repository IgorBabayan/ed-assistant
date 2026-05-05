namespace ED.Assistant.Domain.Events;

public class ShipLockerEvent : BaseJournalEvent
{
	internal const string EventName = "ShipLocker";

	[JsonPropertyName("Items")]
	public IEnumerable<MaterialItem>? Items { get; set; } = default;

	[JsonPropertyName("Components")]
	public IEnumerable<MaterialItem>? Components { get; set; } = default;

	[JsonPropertyName("Consumables")]
	public IEnumerable<MaterialItem>? Consumables { get; set; } = default;

	[JsonPropertyName("Data")]
	public IEnumerable<MaterialItem>? Data { get; set; }
}
