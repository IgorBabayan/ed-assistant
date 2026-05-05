namespace ED.Assistant.Domain.Events;

public class RankEvent: BaseJournalEvent
{
	internal const string EventName = "Rank";

	[JsonPropertyName("Combat")]
	public ushort Combat { get; set; } = default;

	[JsonPropertyName("Trade")]
	public ushort Trade { get; set; } = default;

	[JsonPropertyName("Explore")]
	public ushort Explore { get; set; } = default;

	[JsonPropertyName("Soldier")]
	public ushort Soldier { get; set; } = default;

	[JsonPropertyName("Exobiologist")]
	public ushort Exobiologist { get; set; } = default;

	[JsonPropertyName("Empire")]
	public ushort Empire { get; set; } = default;

	[JsonPropertyName("Federation")]
	public ushort Federation { get; set; } = default;

	[JsonPropertyName("CQC")]
	public ushort CQC { get; set; } = default;
}
