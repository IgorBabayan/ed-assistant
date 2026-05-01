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

	private class DefaultState
	{
		public const string CMDR = "o7, Commander";
		public const string Ship = "Ship not found";
		public const string Status = "Ready";
	}

	[ObservableProperty]
	private string? _cMDR = DefaultState.CMDR;

	[ObservableProperty]
	private string? _ship = DefaultState.Ship;

	[ObservableProperty]
	private string? _status = DefaultState.Status;

	public INavigationStore NavigationStore { get; }

	public MainWindowViewModel(IDialogService dialogService, SettingsViewModel settingsViewModel,
		ILogStorage logStorage, IPathFinder pathFinder, INavigationStore navigationStore)
	{
		_dialogService = dialogService;
		_settingsViewModel = settingsViewModel;
		_logStorage = logStorage;
		_pathFinder = pathFinder;

		NavigationStore = navigationStore;
	}

	[RelayCommand]
    private async Task Load(CancellationToken cancellationToken)
    {
        var state = await _logStorage.LoadLastLogsAsync(_pathFinder.GetPathToLogs(), cancellationToken);

		if (!string.IsNullOrWhiteSpace(state.Commander?.Name))
			CMDR = $"o7, {state.Commander.Name}";

		if (!string.IsNullOrWhiteSpace(state.LoadGame?.ShipFullTitle))
			Ship = state.LoadGame.ShipFullTitle;
	}

	[RelayCommand]
	private async Task Settings() 
		=> await _dialogService.ShowDialogAsync<SettingsViewModel, bool>(_settingsViewModel);
}
