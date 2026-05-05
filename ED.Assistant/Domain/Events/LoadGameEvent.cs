namespace ED.Assistant.Domain.Events;

public class LoadGameEvent : BaseJournalEvent
{
	internal const string EventName = "LoadGame";

	[JsonPropertyName("Ship")]
	public string Ship { get; set; } = string.Empty;

	[JsonPropertyName("ShipName")]
	public string ShipName { get; set; } = string.Empty;

	[JsonPropertyName("ShipIdent")]
	public string ShipIdent { get; set; } = string.Empty;

	[JsonPropertyName("FuelLevel")]
	public double FuelLevel { get; set; } = default;

	[JsonPropertyName("FuelCapacity")]
	public double FuelCapacity { get; set; } = default;

	[JsonPropertyName("Credits")]
	public decimal Credits { get; set; }

	[JsonIgnore]
	public string ShipFullTitle => $"{Ship} {ShipName} ({ShipIdent})";
}
