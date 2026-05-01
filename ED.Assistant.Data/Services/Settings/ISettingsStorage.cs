using ED.Assistant.Data.Models.Config;
using System.Text.Json;

namespace ED.Assistant.Data.Services.Settings;

public interface ISettingsStorage
{
	Task SaveAsync(string filePath, AppSettings settings, CancellationToken cancellationToken = default);
	Task<AppSettings> LoadAsync(string filePath, CancellationToken cancellationToken = default);

}

class SettingsStorage : ISettingsStorage
{
	private readonly JsonSerializerOptions _serializerOptions;

	public SettingsStorage() => _serializerOptions = new()
	{
		WriteIndented = true
	};

	public async Task SaveAsync(string filePath, AppSettings settings, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(filePath))
			throw new ArgumentNullException(nameof(filePath));

		if (File.Exists(filePath))
			File.Delete(filePath);

		using var stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
		await JsonSerializer.SerializeAsync(stream, settings, _serializerOptions, cancellationToken);
	}

	public async Task<AppSettings> LoadAsync(string filePath, CancellationToken cancellationToken = default)
	{
		if (!File.Exists("config.json"))
			throw new FileNotFoundException("Settings file not found.", filePath);

		using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
		return await JsonSerializer.DeserializeAsync<AppSettings>(stream, _serializerOptions, cancellationToken)
			?? new AppSettings();
	}
}