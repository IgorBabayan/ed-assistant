namespace ED.Assistant.Domain.Events;

public class MaterialItem
{
	[JsonPropertyName("Name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("Name_Localised")]
	public string NameLocalised { get; set; } = string.Empty;

	[JsonPropertyName("Count")]
	public ushort Count { get; set; }

	[JsonPropertyName("OwnerID")]
	public ushort OwnerId { get; set; }

	[JsonPropertyName("MissionID")]
	public long MissionId { get; set; }

	[JsonIgnore]
	public string FullName => !string.IsNullOrWhiteSpace(NameLocalised)
		? NameLocalised
		: string.IsNullOrEmpty(Name)
			? string.Empty
			: $"{char.ToUpperInvariant(Name[0])}{Name[1..]}";
}
