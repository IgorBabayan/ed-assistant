namespace ED.Assistant.Domain.Events;

public class RankEvent: BaseJournalEvent
{
	internal const string EventName = "Rank";

	[JsonPropertyName("Combat")]
	public ushort Combat { get; set; }

	[JsonPropertyName("Trade")]
	public ushort Trade { get; set; }

	[JsonPropertyName("Explore")]
	public ushort Explore { get; set; }

	[JsonPropertyName("Soldier")]
	public ushort Soldier { get; set; }

	[JsonPropertyName("Exobiologist")]
	public ushort Exobiologist { get; set; }

	[JsonPropertyName("Empire")]
	public ushort Empire { get; set; }

	[JsonPropertyName("Federation")]
	public ushort Federation { get; set; }

	[JsonPropertyName("CQC")]
	public ushort CQC { get; set; }
}
