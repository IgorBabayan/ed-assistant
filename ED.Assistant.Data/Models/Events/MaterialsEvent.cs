namespace ED.Assistant.Data.Models.Events;

public class MaterialsEvent : BaseJournalEvent
{
	internal const string EventName = "Materials";

	[JsonPropertyName("Raw")]
	public IEnumerable<MaterialItem>? Raw { get; set; } = default;

	[JsonPropertyName("Manufactured")]
	public IEnumerable<MaterialItem>? Manufactured { get; set; } = default;

	[JsonPropertyName("Encoded")]
	public IEnumerable<MaterialItem>? Encoded { get; set; } = default;
}

public class MaterialItem
{
	[JsonPropertyName("Name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("Name_Localised")]
	public string NameLocalised { get; set; } = string.Empty;

	[JsonPropertyName("Count")]
	public ushort Count { get; set; } = default;
}