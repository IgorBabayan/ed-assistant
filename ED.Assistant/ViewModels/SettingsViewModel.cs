using ED.Assistant.Data.Models.Events;
using ED.Assistant.Data.Services.Events;
using ED.Assistant.Data.Services.Path;
using ED.Assistant.Data.Services.Settings;
using ED.Assistant.Services.DialogService;
using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
	private readonly IFolderPickerService _folderPickerService;
	private readonly ISettingsStorage _settingsStorage;

	[ObservableProperty]
	private string? _logFolder = string.Empty;

	public event Action<bool?>? CloseRequested;

	public SettingsViewModel(ILogStorage logStorage, IPathFinder pathFinder, 
		IFolderPickerService folderPickerService, ISettingsStorage settingsStorage,
		IJournalStateStore stateStore) : base(logStorage, pathFinder, stateStore)
	{
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

	protected override void UpdateFromState(JournalState state) { }
}
