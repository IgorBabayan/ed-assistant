namespace ED.Assistant.Data.Models.Events;

public class MaterialItem
{
	[JsonPropertyName("Name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("Name_Localised")]
	public string NameLocalised { get; set; } = string.Empty;

	[JsonPropertyName("Count")]
	public ushort Count { get; set; } = default;

	[JsonPropertyName("OwnerID")]
	public ushort OwnerId { get; set; } = default;

	[JsonPropertyName("MissionID")]
	public long MissionId { get; set; }

	[JsonInclude]
	public string FullName => string.IsNullOrWhiteSpace(NameLocalised)
		? $"{char.ToUpper(Name[0])}{Name[1..]}"
		: NameLocalised;
}
