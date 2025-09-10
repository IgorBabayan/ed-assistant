using EdAssistant.Services.Initialization;

namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.Settings)]
public sealed partial class SettingsViewModel : PageViewModel
{
    private readonly IFolderPickerService _folderPickerService;
    private readonly ISettingsService _settingsService;
    private readonly IDockVisibilityService _dockVisibilityService;
    private readonly IInitializationService _initializationService;
    private readonly ILogger<SettingsViewModel> _logger;

    public bool Cargo
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.Cargo);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.Cargo, value);
            _settingsService.SetSetting("Cargo", value);
        }
    }

    public bool Materials
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.Materials);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.Materials, value);
            _settingsService.SetSetting("Materials", value);
        }
    }

    public bool Storage
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.ShipLocker);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.ShipLocker, value);
            _settingsService.SetSetting("ShipLocker", value);
        }
    }

    public bool System
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.System);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.System, value);
            _settingsService.SetSetting("System", value);
        }
    }

    public bool Planet
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.Planet);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.Planet, value);
            _settingsService.SetSetting("Planet", value);
        }
    }

    public bool MarketConnector
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.MarketConnector);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.MarketConnector, value);
            _settingsService.SetSetting("MarketConnector", value);
        }
    }

    public bool Log
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.Log);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.Log, value);
            _settingsService.SetSetting("Log", value);
        }
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ReadAllCommand))]
    private string? journalsFolderPath;

    public SettingsViewModel(IFolderPickerService folderPickerService, ILogger<SettingsViewModel> logger, IDockVisibilityService dockVisibilityService, ISettingsService settingsService, IInitializationService initializationService)
    {
        _folderPickerService = folderPickerService;
        _logger = logger;
        _dockVisibilityService = dockVisibilityService;
        _settingsService = settingsService;
        _initializationService = initializationService;

        JournalsFolderPath = _folderPickerService.GetDefaultJournalsPath();
        LoadSettings();

        _dockVisibilityService.VisibilityChanged += OnDockVisibilityChanged;
    }

    public override void Dispose() => _dockVisibilityService.VisibilityChanged -= OnDockVisibilityChanged;

    private void LoadSettings()
    {
        _dockVisibilityService.SetVisibility(DockEnum.Cargo,
            _settingsService.GetSetting("Cargo", true));
        _dockVisibilityService.SetVisibility(DockEnum.Materials,
            _settingsService.GetSetting("Materials", true));
        _dockVisibilityService.SetVisibility(DockEnum.ShipLocker,
            _settingsService.GetSetting("ShipLocker", true));
        _dockVisibilityService.SetVisibility(DockEnum.System,
            _settingsService.GetSetting("System", true));
        _dockVisibilityService.SetVisibility(DockEnum.Planet,
            _settingsService.GetSetting("Planet", true));
        _dockVisibilityService.SetVisibility(DockEnum.MarketConnector,
            _settingsService.GetSetting("MarketConnector", true));
        _dockVisibilityService.SetVisibility(DockEnum.Log,
            _settingsService.GetSetting("Log", true));
    }

    [RelayCommand]
    private async Task BrowseFolderAsync()
    {
        var folder = !string.IsNullOrWhiteSpace(JournalsFolderPath) && Directory.Exists(JournalsFolderPath)
            ? await _folderPickerService.PickSingleFolderAsync(suggestedStartPath: JournalsFolderPath)
            : await _folderPickerService.PickSingleFolderAsync();
        if (folder is null)
            return;

        string? path = null;
        try
        {
            path = folder.TryGetLocalPath();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,
                Localization.Instance["Exceptions.FailedGettingFolder"],
                folder.Name, folder.GetType().Name);
        }

        JournalsFolderPath = path ?? folder.Name;
    }

    [RelayCommand(CanExecute = nameof(CanReadAll))]
    private async Task ReadAllAsync() => await _initializationService.InitializeAsync();

    private bool CanReadAll() => !string.IsNullOrWhiteSpace(JournalsFolderPath) && Directory.Exists(JournalsFolderPath);

    private void OnDockVisibilityChanged(object? sender, DockVisibilityChangedEventArgs e) =>
        OnPropertyChanged(e.Dock switch
        {
            DockEnum.Cargo => nameof(Cargo),
            DockEnum.Materials => nameof(Materials),
            DockEnum.ShipLocker => nameof(Storage),
            DockEnum.System => nameof(System),
            DockEnum.Planet => nameof(Planet),
            DockEnum.MarketConnector => nameof(MarketConnector),
            DockEnum.Log => nameof(Log),
            _ => string.Empty
        });
}