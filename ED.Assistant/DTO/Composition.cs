namespace ED.Assistant.DTO;

public class Composition
{
	[JsonPropertyName("Ice")]
	public double Ice { get; set; }

	[JsonPropertyName("Rock")]
	public double Rock { get; set; }

	[JsonPropertyName("Metal")]
	public double Metal { get; set; }
}

public class CompositionItem
{
	[JsonPropertyName("Name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("Percent")]
	public double Percent { get; set; }
}
