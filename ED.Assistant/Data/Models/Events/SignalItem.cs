namespace ED.Assistant.Data.Models.Events;

public class SignalItem
{
	[JsonPropertyName("Type")]
	public string Type { get; set; } = string.Empty;

	[JsonPropertyName("Type_Localised")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("Count")]
	public int Count { get; set; }
}