namespace ED.Assistant.Data.Biology;

public sealed class BioSpecies
{
	public int Id { get; set; }

	public int GenusId { get; set; }
	public BioGenus Genus { get; set; } = null!;

	public string Name { get; set; } = string.Empty;
	public string DisplayName { get; set; } = string.Empty;

	public int BaseValue { get; set; }
	public int MinScanDistanceM { get; set; }

	public int VariantDeterminantId { get; set; }
	public VariantDeterminant VariantDeterminant { get; set; } = null!;

	public BioSpawnRule? SpawnRule { get; set; }

	public ICollection<SpeciesAtmosphereCondition> AtmosphereConditions { get; set; } = [];
}
