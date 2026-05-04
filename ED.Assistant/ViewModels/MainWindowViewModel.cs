using ED.Assistant.Data.Services.Path;
using ED.Assistant.Data.Services.Settings;
using ED.Assistant.Services.DialogService;
using ED.Assistant.Services.Navigation;
using System.ComponentModel;

namespace ED.Assistant.ViewModels;

public partial class MainWindowViewModel : LoadableViewModel
{
	private readonly IDialogService _dialogService;
	private readonly INavigationService _navigationService;
	private readonly ISettingsStorage _settingsStorage;
	private readonly IPathFinder _pathFinder;
	private readonly SettingsViewModel _settingsViewModel;

	private class DefaultState
	{
		public const string CMDR = "o7, Commander";
		public const string Ship = "Ship not found";
		public const string Status = "Ready";
		public const string LogFile = "File not loaded";
		public const string LastEvent = "Event not found";
		public const string WatchStatus = "Auto watch disabled";
	}

	[ObservableProperty]
	private string _cMDR = DefaultState.CMDR;

	[ObservableProperty]
	private string _ship = DefaultState.Ship;

	[ObservableProperty]
	private string _status = DefaultState.Status;

	[ObservableProperty]
	private string _logFile = DefaultState.LogFile;

	[ObservableProperty]
	private string _lastEvent = DefaultState.LastEvent;

	[ObservableProperty]
	private string _watchStatus = DefaultState.WatchStatus;

	public INavigationStore NavigationStore { get; }

	public bool IsDashboardActive => NavigationStore.CurrentViewModel is DashboardViewModel;
	public bool IsSystemActive => NavigationStore.CurrentViewModel is SystemViewModel;
	public bool IsExobilogicalActive => NavigationStore.CurrentViewModel is SignalsViewModel;
	public bool IsJournalActive => NavigationStore.CurrentViewModel is JournalViewModel;
	public bool IsMaterialActive => NavigationStore.CurrentViewModel is MaterialViewModel;
	public bool IsShipLockerActive => NavigationStore.CurrentViewModel is ShipLockerViewModel;

	public MainWindowViewModel(IDialogService dialogService, SettingsViewModel settingsViewModel,
		INavigationStore navigationStore, IJournalStateStore stateStore, IMemoryCache memoryCache,
		INavigationService navigationService, IJournalLoaderService journalLoader,
		ISettingsStorage settingsStorage, IPathFinder pathFinder) : base(journalLoader, stateStore, memoryCache)
	{
		NavigationStore = navigationStore;

		_dialogService = dialogService;
		_settingsStorage = settingsStorage;
		_pathFinder = pathFinder;
		_settingsViewModel = settingsViewModel;
		_navigationService = navigationService;

		_ = InitializeAsync();

		if (NavigationStore is INotifyPropertyChanged notify)
		{
			notify.PropertyChanged += OnPropertyChanged;
		}
	}

	protected override void OnDispose()
	{
		if (NavigationStore is INotifyPropertyChanged notify)
		{
			notify.PropertyChanged -= OnPropertyChanged;
		}
	}

	protected override void UpdateFromState(JournalState state)
	{
		CMDR = $"o7, {state.Commander?.Name ?? "Commander"}";
		Ship = state.LoadGame?.ShipFullTitle ?? DefaultState.Ship;
		LogFile = state.FileName ?? DefaultState.LogFile;
		LastEvent = string.IsNullOrWhiteSpace(state.LastEvent?.Event)
			? DefaultState.LastEvent
			: $"event: '${state.LastEvent!.Event}'";
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
	private async Task NavigateToExobilogicalView(CancellationToken cancellationToken = default)
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
	private async Task Settings(CancellationToken cancellationToken = default)
	{
		var result = await _dialogService.ShowDialogAsync<SettingsViewModel, bool>(_settingsViewModel);
		if (result)
		{
			var settings = await _settingsStorage.LoadAsync(_pathFinder.GetConfigPath(), cancellationToken);
			await UpdateWatchStatus(settings.IsAutoWatchEnable, cancellationToken);
		}
	}

	private void RaiseActiveProperty()
	{
		OnPropertyChanged(nameof(IsDashboardActive));
		OnPropertyChanged(nameof(IsSystemActive));
		OnPropertyChanged(nameof(IsExobilogicalActive));
		OnPropertyChanged(nameof(IsJournalActive));
		OnPropertyChanged(nameof(IsMaterialActive));
		OnPropertyChanged(nameof(IsShipLockerActive));
	}

	private async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			await _navigationService.NavigateToAsync<DashboardViewModel>();
			var settings = await _settingsStorage.LoadAsync(_pathFinder.GetConfigPath(), cancellationToken);
			await UpdateWatchStatus(settings.IsAutoWatchEnable, cancellationToken);
		}
		catch (Exception)
		{
		}
	}

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(NavigationStore.CurrentViewModel))
			LoadCommand.NotifyCanExecuteChanged();
	}

	private async Task UpdateWatchStatus(bool isAutoWatchEnabled, CancellationToken cancellationToken)
	{
		WatchStatus = isAutoWatchEnabled ? "Auto watch enabled" : DefaultState.WatchStatus;
		if (isAutoWatchEnabled)
		{

		}
		else
		{

		}
	}
}
