namespace EdAssistant.ViewModels.Pages;

public sealed partial class SettingsViewModel : PageViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ConnectionStatus))]
    [NotifyPropertyChangedFor(nameof(IsConnected))]
    private string? _journalsFolderPath;
    
    private readonly IFolderPickerService _folderPickerService;
    private readonly ISettingsService _settingsService;
    private readonly IDockVisibilityService _dockVisibilityService;
    private readonly ILogger<SettingsViewModel> _logger;
    
    public bool Cargo
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.Cargo);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.Cargo, value);
            _settingsService.SetSetting("Cargo", value);
        }
    }

    public bool Materials
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.Materials);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.Materials, value);
            _settingsService.SetSetting("Materials", value);
        }
    }

    public bool Storage
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.ShipLocker);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.ShipLocker, value);
            _settingsService.SetSetting("ShipLocker", value);
        }
    }

    public bool System
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.System);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.System, value);
            _settingsService.SetSetting("System", value);
        }
    }

    public bool Planet
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.Planet);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.Planet, value);
            _settingsService.SetSetting("Planet", value);
        }
    }

    public bool MarketConnector
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.MarketConnector);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.MarketConnector, value);
            _settingsService.SetSetting("MarketConnector", value);
        }
    }

    public bool Log
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.Log);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.Log, value);
            _settingsService.SetSetting("Log", value);
        }
    }
    
    public string ConnectionStatus
    {
        get
        {
            if (string.IsNullOrWhiteSpace(JournalsFolderPath))
                return Localization.Instance["SettingsPage.NotConnected"];

            if (!Directory.Exists(JournalsFolderPath))
                return Localization.Instance["SettingsPage.PathNotFound"];

            try
            {
                var journalFiles = Directory.GetFiles(JournalsFolderPath, "*.log");
                if (journalFiles.Length > 0)
                    return Localization.Instance["SettingsPage.Connected"];
                
                return Localization.Instance["SettingsPage.NoJournalFiles"];
            }
            catch
            {
                return Localization.Instance["SettingsPage.AccessDenied"];
            }
        }
    }
    
    public bool IsConnected
    {
        get
        {
            if (string.IsNullOrWhiteSpace(JournalsFolderPath))
                return false;

            if (!Directory.Exists(JournalsFolderPath))
                return false;

            try
            {
                var journalFiles = Directory.GetFiles(JournalsFolderPath, "*.log");
                return journalFiles.Length > 0;
            }
            catch
            {
                return false;
            }
        }
    }

    public string Version => $"v{Assembly.GetExecutingAssembly().GetName().Version}";

    public SettingsViewModel(IFolderPickerService folderPickerService, ISettingsService settingsService, 
        IDockVisibilityService dockVisibilityService, ILogger<SettingsViewModel> logger)
    {
        _folderPickerService = folderPickerService;
        _settingsService = settingsService;
        _dockVisibilityService = dockVisibilityService;
        _logger = logger;

        JournalsFolderPath = folderPickerService.GetDefaultJournalsPath();
        _dockVisibilityService.VisibilityChanged += OnDockVisibilityChanged;
    }

    protected override void OnDispose(bool disposing)
    {
        if (disposing)
        {
            _dockVisibilityService.VisibilityChanged -= OnDockVisibilityChanged;
        }
        base.OnDispose(disposing);
    }

    protected override Task OnInitializeAsync()
    {
        LoadSettings();
        return base.OnInitializeAsync();
    }
    
    private void LoadSettings()
    {
        _dockVisibilityService.SetVisibility(PageEnum.Cargo, _settingsService.GetSetting("Cargo", true));
        _dockVisibilityService.SetVisibility(PageEnum.Materials, _settingsService.GetSetting("Materials", true));
        _dockVisibilityService.SetVisibility(PageEnum.ShipLocker, _settingsService.GetSetting("ShipLocker", true));
        _dockVisibilityService.SetVisibility(PageEnum.System, _settingsService.GetSetting("System", true));
        _dockVisibilityService.SetVisibility(PageEnum.Planet, _settingsService.GetSetting("Planet", true));
        _dockVisibilityService.SetVisibility(PageEnum.MarketConnector, _settingsService.GetSetting("MarketConnector", true));
        _dockVisibilityService.SetVisibility(PageEnum.Log, _settingsService.GetSetting("Log", true));
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
                Localization.Instance["SettingsPage.Exceptions.FailedGettingFolder"],
                folder.Name, folder.GetType().Name);
        }

        JournalsFolderPath = path ?? folder.Name;
    }

    private void OnDockVisibilityChanged(object? sender, DockVisibilityChangedEventArgs e) =>
        OnPropertyChanged(e.Page switch
        {
            PageEnum.Cargo => nameof(Cargo),
            PageEnum.Materials => nameof(Materials),
            PageEnum.ShipLocker => nameof(Storage),
            PageEnum.System => nameof(System),
            PageEnum.Planet => nameof(Planet),
            PageEnum.MarketConnector => nameof(MarketConnector),
            PageEnum.Log => nameof(Log),
            _ => string.Empty
        });

    /*private readonly IFolderPickerService _folderPickerService;
    private readonly ISettingsService _settingsService;
    private readonly IDockVisibilityService _dockVisibilityService;
    private readonly IGameDataService _gameDataService;
    private readonly ILogger<SettingsViewModel> _logger;

    public bool Cargo
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.Cargo);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.Cargo, value);
            _settingsService.SetSetting("Cargo", value);
        }
    }

    public bool Materials
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.Materials);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.Materials, value);
            _settingsService.SetSetting("Materials", value);
        }
    }

    public bool Storage
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.ShipLocker);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.ShipLocker, value);
            _settingsService.SetSetting("ShipLocker", value);
        }
    }

    public bool System
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.System);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.System, value);
            _settingsService.SetSetting("System", value);
        }
    }

    public bool Planet
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.Planet);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.Planet, value);
            _settingsService.SetSetting("Planet", value);
        }
    }

    public bool MarketConnector
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.MarketConnector);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.MarketConnector, value);
            _settingsService.SetSetting("MarketConnector", value);
        }
    }

    public bool Log
    {
        get => _dockVisibilityService.GetVisibility(PageEnum.Log);
        set
        {
            _dockVisibilityService.SetVisibility(PageEnum.Log, value);
            _settingsService.SetSetting("Log", value);
        }
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ReadAllCommand))]
    [NotifyPropertyChangedFor(nameof(ConnectionStatus))]
    [NotifyPropertyChangedFor(nameof(IsConnected))]
    private string? journalsFolderPath;
    
    public string ConnectionStatus
    {
        get
        {
            if (string.IsNullOrWhiteSpace(JournalsFolderPath))
                return Localization.Instance["Settings.NotConnected"];

            if (!Directory.Exists(JournalsFolderPath))
                return Localization.Instance["Settings.PathNotFound"];

            try
            {
                var journalFiles = Directory.GetFiles(JournalsFolderPath, "*.log");
                if (journalFiles.Length > 0)
                    return Localization.Instance["Settings.Connected"];
                
                return Localization.Instance["Settings.NoJournalFiles"];
            }
            catch
            {
                return Localization.Instance["Settings.AccessDenied"];
            }
        }
    }
    
    public bool IsConnected
    {
        get
        {
            if (string.IsNullOrWhiteSpace(JournalsFolderPath))
                return false;

            if (!Directory.Exists(JournalsFolderPath))
                return false;

            try
            {
                var journalFiles = Directory.GetFiles(JournalsFolderPath, "*.log");
                return journalFiles.Length > 0;
            }
            catch
            {
                return false;
            }
        }
    }

    public string Version => $"v{Assembly.GetExecutingAssembly().GetName().Version}";

    public SettingsViewModel(IFolderPickerService folderPickerService, ILogger<SettingsViewModel> logger, IDockVisibilityService dockVisibilityService, ISettingsService settingsService, IGameDataService gameDataService)
    {
        _folderPickerService = folderPickerService;
        _logger = logger;
        _dockVisibilityService = dockVisibilityService;
        _settingsService = settingsService;
        _gameDataService = gameDataService;

        JournalsFolderPath = _folderPickerService.GetDefaultJournalsPath();
        LoadSettings();

        _dockVisibilityService.VisibilityChanged += OnDockVisibilityChanged;
    }

    public override void Dispose() => _dockVisibilityService.VisibilityChanged -= OnDockVisibilityChanged;

    private void LoadSettings()
    {
        _dockVisibilityService.SetVisibility(PageEnum.Cargo,
            _settingsService.GetSetting("Cargo", true));
        _dockVisibilityService.SetVisibility(PageEnum.Materials,
            _settingsService.GetSetting("Materials", true));
        _dockVisibilityService.SetVisibility(PageEnum.ShipLocker,
            _settingsService.GetSetting("ShipLocker", true));
        _dockVisibilityService.SetVisibility(PageEnum.System,
            _settingsService.GetSetting("System", true));
        _dockVisibilityService.SetVisibility(PageEnum.Planet,
            _settingsService.GetSetting("Planet", true));
        _dockVisibilityService.SetVisibility(PageEnum.MarketConnector,
            _settingsService.GetSetting("MarketConnector", true));
        _dockVisibilityService.SetVisibility(PageEnum.Log,
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
    private async Task ReadAllAsync() => await _gameDataService.LoadAll(JournalsFolderPath!);
    
    [RelayCommand(CanExecute = nameof(CanReadAll))]
    private async Task ReadLastAsync() => await _gameDataService.LoadLast(JournalsFolderPath!);

    private bool CanReadAll() => !string.IsNullOrWhiteSpace(JournalsFolderPath) && Directory.Exists(JournalsFolderPath);

    private void OnDockVisibilityChanged(object? sender, DockVisibilityChangedEventArgs e) =>
        OnPropertyChanged(e.Page switch
        {
            PageEnum.Cargo => nameof(Cargo),
            PageEnum.Materials => nameof(Materials),
            PageEnum.ShipLocker => nameof(Storage),
            PageEnum.System => nameof(System),
            PageEnum.Planet => nameof(Planet),
            PageEnum.MarketConnector => nameof(MarketConnector),
            PageEnum.Log => nameof(Log),
            _ => string.Empty
        });*/
}