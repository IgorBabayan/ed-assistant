namespace EdAssistant.ViewModels;

public partial class MainViewModel : BaseViewModel, IRecipient<CommanderMessage>
{
    private readonly INavigationService _navigationService;
    private readonly IDesktopService _desktopService;
    private readonly IDockVisibilityService _dockVisibilityService;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(NavigateToCommand))]
    private PageEnum _currentPage = PageEnum.Home;

    [ObservableProperty]
    private string? _windowTitle;

    public static bool CanCreateDesktopFile => OperatingSystem.IsLinux();
    public bool IsCargo => _dockVisibilityService.GetVisibility(PageEnum.Cargo);
    public bool IsMaterials => _dockVisibilityService.GetVisibility(PageEnum.Materials);
    public bool IsStorage => _dockVisibilityService.GetVisibility(PageEnum.ShipLocker);
    public bool IsSystem => _dockVisibilityService.GetVisibility(PageEnum.System);
    public bool IsPlanet => _dockVisibilityService.GetVisibility(PageEnum.Planet);
    public bool IsMarketConnector => _dockVisibilityService.GetVisibility(PageEnum.MarketConnector);
    public bool IsLog => _dockVisibilityService.GetVisibility(PageEnum.Log);

    public bool IsPlanetarySystem => IsSystem || IsPlanet;
    public bool IsInventory => IsCargo || IsMaterials || IsStorage;

    public MainViewModel(INavigationService navigationService, IDesktopService desktopService,
        IDockVisibilityService dockVisibilityService)
    {
        _navigationService = navigationService;
        _desktopService = desktopService;
        _dockVisibilityService = dockVisibilityService;

        _navigationService.Navigated += OnNavigated;
        _dockVisibilityService.VisibilityChanged += OnDockVisibilityChanged;
        
        WeakReferenceMessenger.Default.Register(this);
    }
    
    public void Receive(CommanderMessage commander) => 
        WindowTitle = string.Format(Localization.Instance["MainWindow.Title"], commander.Name);

    protected override void OnDispose(bool disposing)
    {
        if (disposing)
        {
            _navigationService.Navigated -= OnNavigated;
            _dockVisibilityService.VisibilityChanged -= OnDockVisibilityChanged;    
        }
        
        base.OnDispose(disposing);
    }
    
    private static PageEnum GetPageFromViewModelType(Type viewModelType)
    {
        return viewModelType.Name switch
        {
            nameof(HomeViewModel) => PageEnum.Home,
            nameof(CargoViewModel) => PageEnum.Cargo,
            nameof(MaterialsViewModel) => PageEnum.Materials,
            nameof(StorageViewModel) => PageEnum.ShipLocker,
            nameof(SystemViewModel) => PageEnum.System,
            nameof(PlanetViewModel) => PageEnum.Planet,
            nameof(MarketConnectorViewModel) => PageEnum.MarketConnector,
            nameof(LogViewModel) => PageEnum.Log,
            nameof(SettingsViewModel) => PageEnum.Settings,
            _ => PageEnum.Home
        };
    }

    private void OnNavigated(object? sender, NavigationEventArgs e) => 
        CurrentPage = GetPageFromViewModelType(e.ViewModelType);

    private void OnDockVisibilityChanged(object? sender, DockVisibilityChangedEventArgs e)
    {
        OnPropertyChanged(e.Page switch
        {
            PageEnum.Cargo => nameof(IsCargo),
            PageEnum.Materials => nameof(IsMaterials),
            PageEnum.ShipLocker => nameof(IsStorage),
            PageEnum.System => nameof(IsSystem),
            PageEnum.Planet => nameof(IsPlanet),
            PageEnum.MarketConnector => nameof(IsMarketConnector),
            PageEnum.Log => nameof(IsLog),
            _ => string.Empty
        });

        if (e.Page is PageEnum.System or PageEnum.Planet)
        {
            OnPropertyChanged(nameof(IsPlanetarySystem));
        }

        if (e.Page is PageEnum.Cargo or PageEnum.Materials or PageEnum.ShipLocker)
        {
            OnPropertyChanged(nameof(IsInventory));
        }
    }

    [RelayCommand(CanExecute = nameof(CanCreateDesktopFile))]
    private void CreateDesktopFile()
    {
        _desktopService.CreateDesktopFile();
        _desktopService.Save();
    }
    
    [RelayCommand(CanExecute = nameof(CanNavigateTo))]
    private async Task NavigateToAsync(PageEnum page)
    {
        switch (page)
        {
            case PageEnum.Home:
                await _navigationService.NavigateAsync<HomeViewModel>();
                break;
                
            case PageEnum.Cargo:
                await _navigationService.NavigateAsync<CargoViewModel>();
                break;
            
            case PageEnum.Materials:
                await _navigationService.NavigateAsync<MaterialsViewModel>();
                break;
            
            case PageEnum.ShipLocker:
                await _navigationService.NavigateAsync<StorageViewModel>();
                break;
            
            case PageEnum.System:
                await _navigationService.NavigateAsync<SystemViewModel>();
                break;
            
            case PageEnum.Planet:
                await _navigationService.NavigateAsync<PlanetViewModel>();
                break;
            
            case PageEnum.MarketConnector:
                await _navigationService.NavigateAsync<MarketConnectorViewModel>();
                break;
            
            case PageEnum.Log:
                await _navigationService.NavigateAsync<LogViewModel>();
                break;
            
            case PageEnum.Settings:
                await _navigationService.NavigateAsync<SettingsViewModel>();
                break;
        }
    }
    
    private bool CanNavigateTo(PageEnum page) => CurrentPage != page;
}