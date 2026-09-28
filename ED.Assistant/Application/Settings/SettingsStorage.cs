using ED.Assistant.Domain.Config;
using System.IO;
using System.Text.Json;

namespace ED.Assistant.Application.Settings;

class SettingsStorage : ISettingsStorage
{
	private readonly JsonSerializerOptions _serializerOptions;
	
	public event Action<AppSettings>? SettingsSaved;

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

		await using (var stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
		{
			await JsonSerializer.SerializeAsync(stream, settings, _serializerOptions, cancellationToken);
		}
		
		SettingsSaved?.Invoke(settings);
	}

	public async Task<AppSettings> LoadAsync(string filePath, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(filePath))
			throw new ArgumentNullException(nameof(filePath));

		if (!File.Exists(filePath))
			return new();

		using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
		return await JsonSerializer.DeserializeAsync<AppSettings>(stream, _serializerOptions, cancellationToken)
			?? new();
	}
}