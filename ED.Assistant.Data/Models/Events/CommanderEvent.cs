namespace ED.Assistant.Data.Models.Events;

public class CommanderEvent
{
	internal const string EventName = "Commander";
	internal const string EventNameJSON = "\"event\":\"Commander\"";

	[JsonPropertyName("Name")]
	public string? Name { get; set; }

	[JsonPropertyName("FID")]
	public string? FID { get; set; }

	[JsonPropertyName("timestamp")]
	public DateTime Timestamp { get; set; }
}
