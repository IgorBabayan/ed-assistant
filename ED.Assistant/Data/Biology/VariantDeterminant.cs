namespace ED.Assistant.Data.Biology;

public sealed class VariantDeterminant
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public ICollection<BioSpecies> Species { get; set; } = [];
}
