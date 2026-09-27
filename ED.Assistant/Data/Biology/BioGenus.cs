namespace ED.Assistant.Data.Biology;

public sealed class BioGenus
{
	public int Id { get; set; }
	public string? JournalName { get; set; }
	public string Name { get; set; } = string.Empty;
	public string DisplayName { get; set; } = string.Empty;

	public ICollection<BioSpecies> Species { get; set; } = [];
}
