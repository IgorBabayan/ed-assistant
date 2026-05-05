namespace ED.Assistant.Data.Biology;

public sealed class BioSource
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;
	public string? Url { get; set; }
	public string? Notes { get; set; }

	public List<BioReference> References { get; set; } = [];
}
