namespace ED.Assistant.Data.Biology;

public sealed class BioGenus
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;
	public string DisplayName { get; set; } = string.Empty;
	public string? Description { get; set; }

	public List<BioSpecies> Species { get; set; } = [];
	public List<BioReference> References { get; set; } = [];
}
