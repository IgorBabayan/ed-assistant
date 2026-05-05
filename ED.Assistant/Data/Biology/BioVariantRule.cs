namespace ED.Assistant.Data.Biology;

public sealed class BioVariantRule
{
	public int Id { get; set; }

	public int VariantId { get; set; }

	public string? StarClass { get; set; }
	public string? MaterialName { get; set; }
	public string? RegionName { get; set; }

	public string? Notes { get; set; }

	public BioVariant? Variant { get; set; }
}
