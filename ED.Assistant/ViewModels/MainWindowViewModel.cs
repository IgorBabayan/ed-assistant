using ED.Assistant.Services.DialogService;
using ED.Assistant.Services.Navigation;
using System.ComponentModel;

namespace ED.Assistant.ViewModels;

public partial class MainWindowViewModel : LoadableViewModel
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
	public bool IsSignalsActive => NavigationStore.CurrentViewModel is SignalsViewModel;
	public bool IsJournalActive => NavigationStore.CurrentViewModel is JournalViewModel;
	public bool IsMaterialActive => NavigationStore.CurrentViewModel is MaterialViewModel;
	public bool IsShipLockerActive => NavigationStore.CurrentViewModel is ShipLockerViewModel;

	public MainWindowViewModel(IDialogService dialogService, SettingsViewModel settingsViewModel,
		INavigationStore navigationStore, IJournalStateStore stateStore, IMemoryCache memoryCache,
		INavigationService navigationService, IJournalLoaderService journalLoader)
		: base(journalLoader, stateStore, memoryCache)
	{
		NavigationStore = navigationStore;

		_dialogService = dialogService;
		_settingsViewModel = settingsViewModel;
		_navigationService = navigationService;

		_ = InitializeAsync();

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
	private async Task NavigateToDashboardView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not DashboardViewModel)
		{
			await _navigationService.NavigateToAsync<DashboardViewModel>();
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private async Task NavigateToSystemView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not SystemViewModel)
		{
			await _navigationService.NavigateToAsync<SystemViewModel>();
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private async Task NavigateToSignalsView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not SignalsViewModel)
		{
			await _navigationService.NavigateToAsync<SignalsViewModel>();
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private async Task NavigateToJournalView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not JournalViewModel)
		{
			await _navigationService.NavigateToAsync<JournalViewModel>();
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private async Task NavigateToMaterialView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not MaterialViewModel)
		{
			await _navigationService.NavigateToAsync<MaterialViewModel>();
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private async Task NavigateToShipLockerView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not ShipLockerViewModel)
		{
			await _navigationService.NavigateToAsync<ShipLockerViewModel>();
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private async Task Settings() 
		=> await _dialogService.ShowDialogAsync<SettingsViewModel, bool>(_settingsViewModel);

	private void RaiseActiveProperty()
	{
		OnPropertyChanged(nameof(IsDashboardActive));
		OnPropertyChanged(nameof(IsSystemActive));
		OnPropertyChanged(nameof(IsSignalsActive));
		OnPropertyChanged(nameof(IsJournalActive));
		OnPropertyChanged(nameof(IsMaterialActive));
		OnPropertyChanged(nameof(IsShipLockerActive));
	}

	private async Task InitializeAsync()
	{
		try
		{
			await _navigationService.NavigateToAsync<DashboardViewModel>();
		}
		catch (Exception)
		{
		}
	}
}
