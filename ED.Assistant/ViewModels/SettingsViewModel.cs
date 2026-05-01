using ED.Assistant.Data.Services.Path;
using ED.Assistant.Data.Services.Settings;
using ED.Assistant.Services.DialogService;

namespace ED.Assistant.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
	private readonly IPathFinder _pathFinder;
	private readonly IFolderPickerService _folderPickerService;
	private readonly ISettingsStorage _settingsStorage;

	[ObservableProperty]
	private string? _logFolder = string.Empty;

	public event Action<bool?>? CloseRequested;

	public SettingsViewModel(IPathFinder pathFinder, IFolderPickerService folderPickerService,
		ISettingsStorage settingsStorage)
	{
		_pathFinder = pathFinder;
		_folderPickerService = folderPickerService;
		_settingsStorage = settingsStorage;

		LogFolder = _pathFinder.GetPathToLogs() ?? string.Empty;
	}

	[RelayCommand]
	private async Task Save(CancellationToken cancellationToken = default)
	{
		var path = _pathFinder.GetConfigPath();
		await _settingsStorage.SaveAsync(path, new() { LogFolder = LogFolder }, cancellationToken);

		CloseRequested?.Invoke(true);
	}

	[RelayCommand]
	private void Cancel() => CloseRequested?.Invoke(false);

	[RelayCommand]
	private async Task OpenFolder()
	{
		var folder = await _folderPickerService.PickFolderAsync("Select Elite Dangerous log folder");

		if (folder is not null)
		{
			LogFolder = folder;
		}
	}
}
