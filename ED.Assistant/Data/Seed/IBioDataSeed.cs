namespace ED.Assistant.Data.Seed;

public interface IBioDataSeed
{
	Task<IReadOnlyList<BioGenus>> LoadGeneraAsync(string basePath);
	Task<IReadOnlyList<BioSpecies>> LoadSpeciesAsync(string basePath);
	Task<IReadOnlyList<BioVariant>> LoadVariantsAsync(string basePath);
	Task<IReadOnlyList<BioVariantRule>> LoadVariantRulesAsync(string basePath);
	Task<IReadOnlyList<BioSource>> LoadSourcesAsync(string basePath);
	Task<IReadOnlyList<BioSpawnCondition>> LoadBioSpawnConditionAsync(string basePath);
}
