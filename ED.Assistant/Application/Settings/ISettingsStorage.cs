using ED.Assistant.Domain.Config;

namespace ED.Assistant.Application.Settings;

public interface ISettingsStorage
{
	event Action<AppSettings>? SettingsSaved;
	
	Task SaveAsync(string filePath, AppSettings settings, CancellationToken cancellationToken = default);
	Task<AppSettings> LoadAsync(string filePath, CancellationToken cancellationToken = default);
}
