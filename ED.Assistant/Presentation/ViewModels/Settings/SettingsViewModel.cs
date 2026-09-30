using ED.Assistant.Application.Dialog;
using ED.Assistant.Application.Path;
using ED.Assistant.Application.Settings;
using ED.Assistant.Domain.Config;

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
	
	[ObservableProperty]
	public partial bool HideExcludedSignals { get; set; }
	
	[ObservableProperty]
	public partial decimal? ReadLogsForDays { get; set; } = AppSettings.DEFAULT_READ_LOGS_FOR_DAYS;

	[ObservableProperty]
	public partial DockPosition DockPosition { get; set; }

	public IReadOnlyList<DockPosition> DockPositions { get; } = Enum.GetValues<DockPosition>();

	public event Action<bool?>? CloseRequested;

	public SettingsViewModel(IPathFinder pathFinder, IFolderPickerService folderPickerService,
		ISettingsStorage settingsStorage)
	{
		_pathFinder = pathFinder;
		_folderPickerService = folderPickerService;
		_settingsStorage = settingsStorage;

	}

	[RelayCommand]
	private async Task Save(CancellationToken cancellationToken = default)
	{
		var path = _pathFinder.GetConfigPath();
		await _settingsStorage.SaveAsync(path, new()
		{ 
			LogFolder = LogFolder,
			IsAutoWatchEnable = EnableAutoWatch,
			HideExcludedSignals = HideExcludedSignals,
			ReadLogsForDays = ReadLogsForDays is { } days
				? (int)Math.Clamp(days, 0, int.MaxValue)
				: AppSettings.DEFAULT_READ_LOGS_FOR_DAYS,
			DockPosition = DockPosition
		}, cancellationToken);

		CloseRequested?.Invoke(true);
	}

	[RelayCommand]
	private void Cancel() => CloseRequested?.Invoke(false);

	[RelayCommand]
	private async Task OpenFolder(Window? owner)
	{
		var folder = await _folderPickerService.PickFolderAsync("Select Elite Dangerous log folder", owner);
		if (folder is not null)
		{
			LogFolder = folder;
		}
	}

	public async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		var settings = await _settingsStorage.LoadAsync(_pathFinder.GetConfigPath(), cancellationToken);
		LogFolder = string.IsNullOrWhiteSpace(settings.LogFolder) ? _pathFinder.GetPathToLogs() : settings.LogFolder;
		EnableAutoWatch = settings.IsAutoWatchEnable;
		HideExcludedSignals = settings.HideExcludedSignals;
		ReadLogsForDays = settings.ReadLogsForDays;
		DockPosition = settings.DockPosition;
	}
}
