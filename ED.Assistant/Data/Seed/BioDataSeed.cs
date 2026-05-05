using System.IO;
using System.Text.Json;

namespace ED.Assistant.Data.Seed;

sealed class BioDataSeed
{
	private static readonly JsonSerializerOptions _options = new()
	{
		PropertyNameCaseInsensitive = true
	};

	public async Task<List<BioGenus>> LoadGeneraAsync(string basePath)
		=> await LoadAsync<BioGenus>(Path.Combine(basePath, "genus.json"));

	public async Task<List<BioSpecies>> LoadSpeciesAsync(string basePath)
		=> await LoadAsync<BioSpecies>(Path.Combine(basePath, "species.json"));

	public async Task<List<BioVariant>> LoadVariantsAsync(string basePath)
		=> await LoadAsync<BioVariant>(Path.Combine(basePath, "variants.json"));

	public async Task<List<BioVariantRule>> LoadVariantRulesAsync(string basePath)
		=> await LoadAsync<BioVariantRule>(Path.Combine(basePath, "variant_rules.json"));

	public async Task<List<BioSource>> LoadSourcesAsync(string basePath)
		=> await LoadAsync<BioSource>(Path.Combine(basePath, "sources.json"));

	private static async Task<List<T>> LoadAsync<T>(string path)
	{
		if (!File.Exists(path))
			throw new FileNotFoundException($"Seed file not found: {path}");

		await using var stream = File.OpenRead(path);
		var data = await JsonSerializer.DeserializeAsync<List<T>>(stream, _options);
		return data ?? [];
	}
}
