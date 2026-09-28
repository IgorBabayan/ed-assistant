namespace ED.Assistant.Presentation.ViewModels.System;

public sealed class OrganicSignalViewModel
{
	public string Type { get; init; } = string.Empty;
	public string Name { get; init; } = "—";
	public string Variant { get; init; } = "—";

	public int CollectedCount { get; init; }

	public string BaseValue { get; init; } = "—";
	public string Distance { get; init; } = "—";
	
	public bool IsPrediction { get; init; }
	public bool IsExcluded { get; init; }
	public string SpeciesId { get; init; } = string.Empty;
	public string GenusId { get; init; } = string.Empty;
}