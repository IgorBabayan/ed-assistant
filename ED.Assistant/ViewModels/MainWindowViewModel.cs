using ED.Assistant.Data.Models.Events;
using ED.Assistant.Services.DialogService;
using ED.Assistant.Services.Journal;
using ED.Assistant.Services.Navigation;
using System.ComponentModel;

namespace ED.Assistant.ViewModels;

public partial class MainWindowViewModel : BaseViewModel
{
	private readonly IDialogService _dialogService;
	private readonly INavigationService _navigationService;
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
		INavigationStore navigationStore, IJournalStateStore stateStore,
		INavigationService navigationService) : base(stateStore)
	{
		_dialogService = dialogService;
		_settingsViewModel = settingsViewModel;
		_navigationService = navigationService;

		NavigationStore = navigationStore;

		if (NavigationStore is INotifyPropertyChanged notify)
		{
			notify.PropertyChanged += (_, e) =>
			{
				if (e.PropertyName == nameof(NavigationStore.CurrentViewModel))
					LoadCommand.NotifyCanExecuteChanged();
			};
		}
	}

	protected override void UpdateFromState(JournalState state)
	{
		CMDR = $"o7, {state.Commander?.Name ?? "Commander"}";
		Ship = state.LoadGame?.ShipFullTitle ?? DefaultState.Ship;
	}

	[RelayCommand]
	private void NavigateToDashboardView()
	{
		if (NavigationStore.CurrentViewModel is not DashboardViewModel)
			_navigationService.NavigateTo<DashboardViewModel>();
	}

	[RelayCommand]
	private void NavigateToSystemView()
	{
		if (NavigationStore.CurrentViewModel is not SystemViewModel)
			_navigationService.NavigateTo<SystemViewModel>();
	}

	[RelayCommand]
	private void NavigateToBodiesView()
	{
		if (NavigationStore.CurrentViewModel is not BodiesViewModel)
			_navigationService.NavigateTo<BodiesViewModel>();
	}

	[RelayCommand]
	private void NavigateToSignalsView()
	{
		if (NavigationStore.CurrentViewModel is not SignalsViewModel)
			_navigationService.NavigateTo<SignalsViewModel>();
	}

	[RelayCommand]
	private void NavigateToJournalView()
	{
		if (NavigationStore.CurrentViewModel is not JournalViewModel)
			_navigationService.NavigateTo<JournalViewModel>();
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
}
