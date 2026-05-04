namespace ED.Assistant.Data.Models.Events;

public class SAASignalsFoundEvent : BaseJournalEvent
{
	internal const string EventName = "SAASignalsFound";

	[JsonPropertyName("BodyID")]
	public int BodyId { get; set; }

	[JsonPropertyName("BodyName")]
	public string BodyName { get; set; } = string.Empty;

	[JsonPropertyName("SystemAddress")]
	public long SystemAddress { get; set; }

	[JsonPropertyName("Genuses")]
	public IEnumerable<GenusItem>? Genuses { get; set; } = default;

	[JsonPropertyName("Genuses")]
	public IEnumerable<SignalItem>? Signals { get; set; } = default;
}

public class GenusItem
{
	[JsonPropertyName("Genus_Localised")]
	public string Genus { get; set; } = string.Empty;
}