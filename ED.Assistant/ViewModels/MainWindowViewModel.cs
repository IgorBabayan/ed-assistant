using ED.Assistant.Data.Models.Events;
using ED.Assistant.Services.DialogService;
using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public partial class MainWindowViewModel : BaseViewModel
{
	private readonly IDialogService _dialogService;
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
		INavigationStore navigationStore, IJournalStateStore stateStore) : base(stateStore)
	{
		_dialogService = dialogService;
		_settingsViewModel = settingsViewModel;

		NavigationStore = navigationStore;
	}

	[RelayCommand]
	private async Task Settings() 
		=> await _dialogService.ShowDialogAsync<SettingsViewModel, bool>(_settingsViewModel);

	[RelayCommand(CanExecute = nameof(CanLoad))]
	private async Task Load()
	{
		if (NavigationStore.CurrentViewModel is ILoadableViewModel loadable)
			await loadable.LoadCommand.ExecuteAsync(null);
	}

	private bool CanLoad() => NavigationStore.CurrentViewModel is ILoadableViewModel;

	protected override void UpdateFromState(JournalState state)
	{
		CMDR = $"o7, {state.Commander?.Name ?? "Commander"}";
		Ship = state.LoadGame?.ShipFullTitle ?? DefaultState.Ship;
	}
}
