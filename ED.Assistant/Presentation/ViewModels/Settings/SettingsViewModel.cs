using ED.Assistant.Application.Dialog;
using ED.Assistant.Application.Path;
using ED.Assistant.Application.Settings;

namespace ED.Assistant.Presentation.ViewModels.Settings;

public partial class SettingsViewModel : BaseViewModel
{
	private readonly IFolderPickerService _folderPickerService;
	private readonly ISettingsStorage _settingsStorage;
	private readonly IPathFinder _pathFinder;

	[ObservableProperty]
	public partial string? LogFolder { get; set; } = string.Empty;

	[ObservableProperty]
	public partial bool EnableAutoWatch { get; set; }

	public event Action<bool?>? CloseRequested;

	public SettingsViewModel(IPathFinder pathFinder, IFolderPickerService folderPickerService,
		ISettingsStorage settingsStorage)
	{
		_pathFinder = pathFinder;
		_folderPickerService = folderPickerService;
		_settingsStorage = settingsStorage;

		_ = InitializeAsync();
	}

	[RelayCommand]
	private async Task Save(CancellationToken cancellationToken = default)
	{
		var path = _pathFinder.GetConfigPath();
		await _settingsStorage.SaveAsync(path, new()
		{ 
			LogFolder = LogFolder,
			IsAutoWatchEnable = EnableAutoWatch
		}, cancellationToken);

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

	private async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			var settings = await _settingsStorage.LoadAsync(_pathFinder.GetConfigPath(), cancellationToken);
			LogFolder = settings.LogFolder ?? _pathFinder.GetPathToLogs();
			EnableAutoWatch = settings.IsAutoWatchEnable;
		}
		catch (Exception)
		{
		}
	}
}
