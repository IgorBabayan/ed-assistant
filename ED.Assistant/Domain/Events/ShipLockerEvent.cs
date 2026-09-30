namespace ED.Assistant.Domain.Events;

public class ShipLockerEvent : BaseJournalEvent
{
	internal const string EventName = "ShipLocker";

	[JsonPropertyName("Items")]
	public IEnumerable<MaterialItem>? Items { get; set; }

	[JsonPropertyName("Components")]
	public IEnumerable<MaterialItem>? Components { get; set; }

	[JsonPropertyName("Consumables")]
	public IEnumerable<MaterialItem>? Consumables { get; set; }

	[JsonPropertyName("Data")]
	public IEnumerable<MaterialItem>? Data { get; set; }
}
