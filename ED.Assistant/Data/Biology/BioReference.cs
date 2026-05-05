namespace ED.Assistant.Data.Biology;

public sealed class BioReference
{
	public int Id { get; set; }

	public int? GenusId { get; set; }
	public int? SpeciesId { get; set; }
	public int? VariantId { get; set; }

	public int SourceId { get; set; }
	public string SourceUrl { get; set; } = string.Empty;

	public BioGenus? Genus { get; set; }
	public BioSpecies? Species { get; set; }
	public BioVariant? Variant { get; set; }
	public BioSource? Source { get; set; }
}
