using ED.Assistant.Application.Dialog;
using ED.Assistant.Application.Navigation;
using ED.Assistant.Application.Path;
using ED.Assistant.Application.Settings;
using ED.Assistant.Presentation.ViewModels.Dashboard;
using ED.Assistant.Presentation.ViewModels.Exobiology;
using ED.Assistant.Presentation.ViewModels.Journal;
using ED.Assistant.Presentation.ViewModels.Material;
using ED.Assistant.Presentation.ViewModels.Settings;
using ED.Assistant.Presentation.ViewModels.ShipLocker;
using ED.Assistant.Presentation.ViewModels.System;
using System.ComponentModel;
using System.Diagnostics;
using ED.Assistant.Application.Evaluation;
using ED.Assistant.Application.Linux;
using ED.Assistant.Domain.Config;
using ED.Assistant.Presentation.ViewModels.Evaluator;
using ED.Assistant.Presentation.ViewModels.Import;
using Material.Icons;

namespace ED.Assistant.Presentation.ViewModels.Shell;

public partial class MainWindowViewModel : LoadableViewModel
{
	private readonly IDialogService _dialogService;
	private readonly INavigationService _navigationService;
	private readonly ISettingsStorage _settingsStorage;
	private readonly IPathFinder _pathFinder;
	private readonly IJournalWatchService _journalWatchService;
	private readonly IDesktopService _desktopService;
	private readonly IFolderPickerService _folderPickerService;
	private readonly IEvaluatorImportService _evaluatorImportService;
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
	
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsDockBottom), nameof(IsDockLeft), nameof(IsDockRight))]
	public partial DockPosition DockPosition { get; set; } = DockPosition.Bottom;

	public bool IsDockBottom => DockPosition == DockPosition.Bottom;
	public bool IsDockLeft => DockPosition == DockPosition.Left;
	public bool IsDockRight => DockPosition == DockPosition.Right;
	
	public ObservableCollection<object> DockItems { get; } = [];

	public bool IsNotHyprland => !DesktopEnvironmentHelper.IsHyprland();
	
	public bool IsLinux => OperatingSystem.IsLinux();

	// The shell header is always on screen and is never a navigation target
	protected override bool IsAlwaysVisible => true;

	public MainWindowViewModel(IDialogService dialogService, SettingsViewModel settingsViewModel,
		INavigationStore navigationStore, IJournalStateStore stateStore, IMemoryCache memoryCache,
		INavigationService navigationService, IJournalLoaderService journalLoader,
		ISettingsStorage settingsStorage, IPathFinder pathFinder,
		IJournalWatchService journalWatchService, IDesktopService desktopService, IFolderPickerService folderPickerService,
		IEvaluatorImportService evaluatorImportService) 
			: base(journalLoader, stateStore, memoryCache)
	{
		NavigationStore = navigationStore;

		_journalWatchService = journalWatchService;
		_desktopService = desktopService;
		_folderPickerService = folderPickerService;
		_evaluatorImportService = evaluatorImportService;
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
		
		BuildDockItems();
	}

	protected override void OnDispose()
	{
		if (NavigationStore is INotifyPropertyChanged notify)
		{
			notify.PropertyChanged -= OnPropertyChanged;
		}

		base.OnDispose();
	}

	protected override void UpdateFromState(JournalState state)
	{
		// Read the state here (watcher thread, state is consistent now), apply on the UI thread
		var cmdr = $"o7, {state.Commander?.Name ?? "Commander"}";
		var ship = state.LoadGame?.ShipFullTitle ?? DefaultState.Ship;
		var logFile = state.FileName ?? DefaultState.LogFile;
		var lastEvent = string.IsNullOrWhiteSpace(state.LastEvent?.Event)
			? DefaultState.LastEvent
			: $"event: '{state.LastEvent!.Event}'";

		RunOnUIThread(() =>
		{
			CMDR = cmdr;
			Ship = ship;
			LogFile = logFile;
			LastEvent = lastEvent;
		});
	}
	
	private void BuildDockItems()
	{
		DockItems.Add(new DockItemViewModel("Dashboard", MaterialIconKind.ViewDashboard, NavigateToDashboardViewCommand, typeof(DashboardViewModel)));
		DockItems.Add(new DockItemViewModel("Materials", MaterialIconKind.HexagonMultiple, NavigateToMaterialViewCommand, typeof(MaterialViewModel)));
		DockItems.Add(new DockItemViewModel("Ship locker", MaterialIconKind.PackageVariant, NavigateToShipLockerViewCommand, typeof(ShipLockerViewModel)));
		DockItems.Add(new DockSeparatorViewModel());
		DockItems.Add(new DockItemViewModel("System", MaterialIconKind.Orbit, NavigateToSystemViewCommand, typeof(SystemViewModel)));
		DockItems.Add(new DockItemViewModel("Exobiology", MaterialIconKind.Leaf, NavigateToExobilogicalViewCommand, typeof(ExobiologyViewModel)));
		DockItems.Add(new DockItemViewModel("Evaluator", MaterialIconKind.CalculatorVariant, NavigateToEvaluatorViewCommand, typeof(EvaluatorViewModel)));
		DockItems.Add(new DockSeparatorViewModel());
		DockItems.Add(new DockItemViewModel("Journal", MaterialIconKind.BookOpenVariant, NavigateToJournalViewCommand, typeof(JournalViewModel)));
	}

	private void UpdateDockActiveState()
	{
		var current = NavigationStore.CurrentViewModel;
		foreach (var item in DockItems.OfType<DockItemViewModel>())
			item.IsActive = item.TargetViewModel.IsInstanceOfType(current);
	}

	partial void OnIsAutoWatchEnabledChanged(bool value) => _ = UpdateWatchStatus(value);

	[RelayCommand]
	private async Task NavigateToDashboardView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not DashboardViewModel)
		{
			await _navigationService.NavigateToAsync<DashboardViewModel>(cancellationToken);
		}
	}

	[RelayCommand]
	private async Task NavigateToSystemView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not SystemViewModel)
		{
			await _navigationService.NavigateToAsync<SystemViewModel>(cancellationToken);
		}
	}

	[RelayCommand]
	private async Task NavigateToExobilogicalView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not ExobiologyViewModel)
		{
			await _navigationService.NavigateToAsync<ExobiologyViewModel>(cancellationToken);
		}
	}

	[RelayCommand]
	private async Task NavigateToJournalView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not JournalViewModel)
		{
			await _navigationService.NavigateToAsync<JournalViewModel>(cancellationToken);
		}
	}

	[RelayCommand]
	private async Task NavigateToMaterialView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not MaterialViewModel)
		{
			await _navigationService.NavigateToAsync<MaterialViewModel>(cancellationToken);
		}
	}

	[RelayCommand]
	private async Task NavigateToShipLockerView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not ShipLockerViewModel)
		{
			await _navigationService.NavigateToAsync<ShipLockerViewModel>(cancellationToken);
		}
	}

	[RelayCommand]
	private async Task NavigateToEvaluatorView(CancellationToken cancellationToken = default)
	{
		if (NavigationStore.CurrentViewModel is not EvaluatorViewModel)
		{
			await _navigationService.NavigateToAsync<EvaluatorViewModel>(cancellationToken);
		}
	}

	[RelayCommand]
	private async Task Settings(CancellationToken cancellationToken = default)
	{
		var configPath = _pathFinder.GetConfigPath();
		var previousDays = (await _settingsStorage.LoadAsync(configPath, cancellationToken)).ReadLogsForDays;

		var result = await _dialogService.ShowDialogAsync<SettingsViewModel, bool>(_settingsViewModel);
		if (result)
		{
			var settings = await _settingsStorage.LoadAsync(configPath, cancellationToken);
			IsAutoWatchEnabled = settings.IsAutoWatchEnable;

			if (settings.ReadLogsForDays != previousDays)
				await _journalLoader.LoadLastLogsAsync(cancellationToken);
			
			DockPosition = settings.DockPosition;
		}
	}

	[RelayCommand(CanExecute = nameof(IsLinux))]
	private async Task CreateDesktop(CancellationToken cancellationToken = default)
	{
		_desktopService.BuildDesktopFile();
		await _desktopService.SaveDesktopFileAsync(cancellationToken);
	}
	
	[RelayCommand]
	private async Task Import(CancellationToken cancellationToken = default)
	{
		var folder = await AskImportFolderAsync();
		if (string.IsNullOrWhiteSpace(folder))
			return;

		Status = "Importing bio-samples…";
		try
		{
			await _evaluatorImportService.ImportAsync(folder, cancellationToken);
			await NavigateToEvaluatorView(cancellationToken);
		}
		catch (OperationCanceledException)
		{
			Status = DefaultState.Status;
		}
		catch (Exception ex)
		{
			Status = $"Import failed: {ex.Message}";
		}
	}
	
	private Task<string?> AskImportFolderAsync()
	{
		if (DesktopEnvironmentHelper.IsHyprland())
		{
			var dialog = new ImportFolderViewModel(_folderPickerService, _pathFinder.GetPathToLogs());
			return _dialogService.ShowDialogAsync<ImportFolderViewModel, string>(dialog);
		}

		return _folderPickerService.PickFolderAsync("Select folder with Elite Dangerous journals");
	}

	private async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			await _navigationService.NavigateToAsync<DashboardViewModel>(cancellationToken);
			await _journalLoader.LoadLastLogsAsync(cancellationToken);

			var settings = await _settingsStorage.LoadAsync(_pathFinder.GetConfigPath(), cancellationToken);
			IsAutoWatchEnabled = settings.IsAutoWatchEnable;
			
			DockPosition = settings.DockPosition;
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex)
		{
			// Fire-and-forget from the constructor: never let it throw, but don't hide the reason
			Debug.WriteLine($"MainWindow initialization failed: {ex}");
		}
	}

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(NavigationStore.CurrentViewModel))
			LoadCommand.NotifyCanExecuteChanged();
		
		UpdateDockActiveState();
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