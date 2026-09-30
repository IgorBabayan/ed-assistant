using ED.Assistant.Domain.Config;
using System.IO;
using System.Text.Json;

namespace ED.Assistant.Application.Settings;

internal class SettingsStorage : ISettingsStorage
{
	private readonly JsonSerializerOptions _serializerOptions = new()
	{
		WriteIndented = true
	};

	public event Action<AppSettings>? SettingsSaved;

	public async Task SaveAsync(string filePath, AppSettings settings, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

		// Write to a temp file and swap it in, so a crash mid-write can't leave a broken/empty config
		var tempPath = filePath + ".tmp";
		await using (var stream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
		{
			await JsonSerializer.SerializeAsync(stream, settings, _serializerOptions, cancellationToken);
		}

		File.Move(tempPath, filePath, overwrite: true);

		SettingsSaved?.Invoke(settings);
	}

	public async Task<AppSettings> LoadAsync(string filePath, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

		if (!File.Exists(filePath))
			return new AppSettings();

		await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
		return await JsonSerializer.DeserializeAsync<AppSettings>(stream, _serializerOptions, cancellationToken)
			?? new AppSettings();
	}
}