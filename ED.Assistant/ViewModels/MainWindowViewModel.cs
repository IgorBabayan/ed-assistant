using ED.Assistant.Data.Models.Events;
using ED.Assistant.Data.Services.Events;
using ED.Assistant.Data.Services.Path;
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

	public bool IsDashboardActive => NavigationStore.CurrentViewModel is DashboardViewModel;
	public bool IsSystemActive => NavigationStore.CurrentViewModel is SystemViewModel;
	public bool IsBodiesActive => NavigationStore.CurrentViewModel is BodiesViewModel;
	public bool IsSignalsActive => NavigationStore.CurrentViewModel is SignalsViewModel;
	public bool IsJournalActive => NavigationStore.CurrentViewModel is JournalViewModel;

	public MainWindowViewModel(IDialogService dialogService, SettingsViewModel settingsViewModel,
		INavigationStore navigationStore, IJournalStateStore stateStore,
		INavigationService navigationService, ILogStorage logStorage,
		IPathFinder pathFinder) : base(logStorage, pathFinder, stateStore)
	{
		_dialogService = dialogService;
		_settingsViewModel = settingsViewModel;

		_navigationService = navigationService;
		_navigationService.NavigateTo<DashboardViewModel>();

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
		{
			_navigationService.NavigateTo<DashboardViewModel>();
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private void NavigateToSystemView()
	{
		if (NavigationStore.CurrentViewModel is not SystemViewModel)
		{
			_navigationService.NavigateTo<SystemViewModel>();
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private void NavigateToBodiesView()
	{
		if (NavigationStore.CurrentViewModel is not BodiesViewModel)
		{
			_navigationService.NavigateTo<BodiesViewModel>();
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private void NavigateToSignalsView()
	{
		if (NavigationStore.CurrentViewModel is not SignalsViewModel)
		{
			_navigationService.NavigateTo<SignalsViewModel>();
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private void NavigateToJournalView()
	{
		if (NavigationStore.CurrentViewModel is not JournalViewModel)
		{
			_navigationService.NavigateTo<JournalViewModel>();
			RaiseActiveProperty();
		}
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
	private void RaiseActiveProperty()
	{
		OnPropertyChanged(nameof(IsDashboardActive));
		OnPropertyChanged(nameof(IsSystemActive));
		OnPropertyChanged(nameof(IsBodiesActive));
		OnPropertyChanged(nameof(IsSignalsActive));
		OnPropertyChanged(nameof(IsJournalActive));
	}
}
