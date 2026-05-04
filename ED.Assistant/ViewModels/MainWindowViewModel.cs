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
	private readonly IJournalWatchService _journalWatchService;
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
	public partial string CMDR { get; set; } = DefaultState.CMDR;

	[ObservableProperty]
	public partial string Ship { get; set; } = DefaultState.Ship;

	[ObservableProperty]
	public partial string Status { get; set; } = DefaultState.Status;

	[ObservableProperty]
	public partial string LogFile { get; set; } = DefaultState.LogFile;

	[ObservableProperty]
	public partial string LastEvent { get; set; } = DefaultState.LastEvent;

	[ObservableProperty]
	public partial string WatchStatus { get; set; } = DefaultState.WatchStatus;

	[ObservableProperty]
	public partial bool IsAutoWatchEnabled { get; set; }

	public INavigationStore NavigationStore { get; }

	public bool IsDashboardActive => NavigationStore.CurrentViewModel is DashboardViewModel;
	public bool IsSystemActive => NavigationStore.CurrentViewModel is SystemViewModel;
	public bool IsExobilogicalActive => NavigationStore.CurrentViewModel is ExobiologyViewModel;
	public bool IsJournalActive => NavigationStore.CurrentViewModel is JournalViewModel;
	public bool IsMaterialActive => NavigationStore.CurrentViewModel is MaterialViewModel;
	public bool IsShipLockerActive => NavigationStore.CurrentViewModel is ShipLockerViewModel;

	public MainWindowViewModel(IDialogService dialogService, SettingsViewModel settingsViewModel,
		INavigationStore navigationStore, IJournalStateStore stateStore, IMemoryCache memoryCache,
		INavigationService navigationService, IJournalLoaderService journalLoader,
		ISettingsStorage settingsStorage, IPathFinder pathFinder,
		IJournalWatchService journalWatchService) : base(journalLoader, stateStore, memoryCache)
	{
		NavigationStore = navigationStore;

		_journalWatchService = journalWatchService;
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
			: $"event: '{state.LastEvent!.Event}'";
	}

	partial void OnIsAutoWatchEnabledChanged(bool value) => _ = UpdateWatchStatus(value);

	[RelayCommand]
	private async Task NavigateToDashboardView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not DashboardViewModel)
		{
			await _navigationService.NavigateToAsync<DashboardViewModel>(cancellationToken);
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private async Task NavigateToSystemView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not SystemViewModel)
		{
			await _navigationService.NavigateToAsync<SystemViewModel>(cancellationToken);
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private async Task NavigateToExobilogicalView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not ExobiologyViewModel)
		{
			await _navigationService.NavigateToAsync<ExobiologyViewModel>(cancellationToken);
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private async Task NavigateToJournalView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not JournalViewModel)
		{
			await _navigationService.NavigateToAsync<JournalViewModel>(cancellationToken);
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private async Task NavigateToMaterialView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not MaterialViewModel)
		{
			await _navigationService.NavigateToAsync<MaterialViewModel>(cancellationToken);
			RaiseActiveProperty();
		}
	}

	[RelayCommand]
	private async Task NavigateToShipLockerView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not ShipLockerViewModel)
		{
			await _navigationService.NavigateToAsync<ShipLockerViewModel>(cancellationToken);
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
			IsAutoWatchEnabled = settings.IsAutoWatchEnable;
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
			await _navigationService.NavigateToAsync<DashboardViewModel>(cancellationToken);
			await _journalLoader.LoadLastLogsAsync(cancellationToken);

			var settings = await _settingsStorage.LoadAsync(_pathFinder.GetConfigPath(), cancellationToken);
			IsAutoWatchEnabled = settings.IsAutoWatchEnable;
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

	private async Task UpdateWatchStatus(bool isAutoWatchEnabled, CancellationToken cancellationToken = default)
	{
		WatchStatus = isAutoWatchEnabled ? "Auto watch enabled" : DefaultState.WatchStatus;
		
		if (isAutoWatchEnabled)
		{
			var logFolder = _pathFinder.GetPathToLogs();
			await _journalWatchService.StartAsync(logFolder, cancellationToken);
		}
		else
		{
			_journalWatchService.Stop();
		}
	}
}
