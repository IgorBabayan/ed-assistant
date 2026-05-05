namespace ED.Assistant.Domain.Events;

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

	[JsonPropertyName("Signals")]
	public IEnumerable<SignalItem>? Signals { get; set; } = default;
}

public class GenusItem
{
	[JsonPropertyName("Genus")]
	public string GenusId { get; set; } = string.Empty;

	[JsonPropertyName("Genus_Localised")]
	public string Genus { get; set; } = string.Empty;
}