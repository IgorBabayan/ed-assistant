namespace ED.Assistant.Data.Models.Events;

public class FSSBodySignalsEvent : BaseJournalEvent
{
	internal const string EventName = "FSSBodySignals";

	[JsonPropertyName("BodyID")]
	public int BodyId { get; set; }

	[JsonPropertyName("BodyName")]
	public string BodyName { get; set; } = string.Empty;

	[JsonPropertyName("SystemAddress")]
	public long SystemAddress { get; set; }

	[JsonPropertyName("Signals")]
	public IEnumerable<SignalItem>? Signals { get; set; } = default;
}

public class SignalItem
{
	[JsonPropertyName("Type")]
	public string Type { get; set; } = string.Empty;

	[JsonPropertyName("Type_Localised")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("Count")]
	public int Count { get; set; }
}