namespace ED.Assistant.Data.Biology;

public sealed class BioCatalogVersion
{
	public string Id { get; set; } = string.Empty;
	public string ContentHash { get; set; } = string.Empty;
	public string SourceCommit { get; set; } = string.Empty;
}
