using ED.Assistant.Data.Services.Events;
using ED.Assistant.Data.Services.Path;
using ED.Assistant.Services.DialogService;

namespace ED.Assistant.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
	private readonly IDialogService _dialogService;
	private readonly ILogStorage _logStorage;
	private readonly IPathFinder _pathFinder;
	private readonly SettingsViewModel _settingsViewModel;

	public MainWindowViewModel(IDialogService dialogService, SettingsViewModel settingsViewModel,
		ILogStorage logStorage, IPathFinder pathFinder)
	{
		_dialogService = dialogService;
		_settingsViewModel = settingsViewModel;
		_logStorage = logStorage;
		_pathFinder = pathFinder;
	}

	[ObservableProperty]
    private string? _cMDR = "o7, Commander";

	[ObservableProperty]
	private string? _ship = "Ship not found";

	[ObservableProperty]
	private string? _status = "Ready";

	[RelayCommand]
    private async Task Load(CancellationToken cancellationToken)
    {
        await _logStorage.LoadLastLogs(_pathFinder.GetPathToLogs(), cancellationToken);
	}

	[RelayCommand]
	private async Task Settings() 
		=> await _dialogService.ShowDialogAsync<SettingsViewModel, bool>(_settingsViewModel);
}
