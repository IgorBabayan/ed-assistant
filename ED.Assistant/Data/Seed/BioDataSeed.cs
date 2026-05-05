using System.IO;
using System.Text.Json;

namespace ED.Assistant.Data.Seed;

sealed class BioDataSeed : IBioDataSeed
{
	private static readonly JsonSerializerOptions _options = new()
	{
		PropertyNameCaseInsensitive = true
	};

	public Task<IReadOnlyList<BioGenus>> LoadGeneraAsync(string basePath)
		=> LoadAsync<BioGenus>(basePath, "genus.json");

	public Task<IReadOnlyList<BioSpecies>> LoadSpeciesAsync(string basePath)
		=> LoadAsync<BioSpecies>(basePath, "species.json");

	public Task<IReadOnlyList<BioVariant>> LoadVariantsAsync(string basePath)
		=> LoadAsync<BioVariant>(basePath, "variants.json");

	public Task<IReadOnlyList<BioVariantRule>> LoadVariantRulesAsync(string basePath)
		=> LoadAsync<BioVariantRule>(basePath, "variant-rules.json");

	public Task<IReadOnlyList<BioSource>> LoadSourcesAsync(string basePath)
		=> LoadAsync<BioSource>(basePath, "sources.json");

	public Task<IReadOnlyList<BioSpawnCondition>> LoadBioSpawnConditionAsync(string basePath)
		=> LoadAsync<BioSpawnCondition>(basePath, "spawn_condition.json");

	private static async Task<IReadOnlyList<T>> LoadAsync<T>(string basePath, string fileName)
	{
		var path = IOPath.Combine(basePath, fileName);

		if (!File.Exists(path))
			throw new FileNotFoundException($"Seed file not found: {path}");

		await using var stream = File.OpenRead(path);

		var data = await JsonSerializer.DeserializeAsync<List<T>>(stream, _options);
		return data ?? [];
	}
}
