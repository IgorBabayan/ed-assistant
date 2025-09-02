using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EdAssistant.Models.Enums;
using EdAssistant.Services.Navigate;
using EdAssistant.Translations;
using EdAssistant.ViewModels.Pages;

namespace EdAssistant.ViewModels;

public sealed record DockVisibilityChanged(DockEnum Dock, bool IsVisible);

public partial class MainViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    public string WindowTitle => Localization.Instance["MainWindow.Title"];

    public DockEnum CurrentDock
    {
        get
        {
            return CurrentViewModel switch
            {
                HomeViewModel => DockEnum.Home,
                MaterialsViewModel => DockEnum.Materials,
                StorageViewModel => DockEnum.Storage,
                SystemViewModel => DockEnum.System,
                PlanetViewModel => DockEnum.Planet,
                MarketConnectorViewModel => DockEnum.MarketConnector,
                LogViewModel => DockEnum.Log,
                SettingsViewModel => DockEnum.Settings,
                _ => default
            };
        }
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(NavigateToCommand))]
    private PageViewModel? currentViewModel;

    [ObservableProperty]
    private bool isMaterials;

    [ObservableProperty]
    private bool isStorage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPlanetarySystem))]
    private bool isSystem;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPlanetarySystem))]
    private bool isPlanet;

    [ObservableProperty]
    private bool isMarketConnector;

    [ObservableProperty]
    private bool isLog;

    public bool IsPlanetarySystem => IsSystem || IsPlanet;

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        CurrentViewModel = _navigationService.Current;

        WeakReferenceMessenger.Default.Register<DockVisibilityChanged>(this, (x, y) =>
        {
            switch (y.Dock)
            {
                case DockEnum.Materials:
                    IsMaterials = y.IsVisible;
                    break;

                case DockEnum.Storage:
                    IsStorage = y.IsVisible;
                    break;

                case DockEnum.System:
                    IsSystem = y.IsVisible;
                    break;

                case DockEnum.Planet:
                    IsPlanet = y.IsVisible;
                    break;

                case DockEnum.MarketConnector:
                    IsMarketConnector = y.IsVisible;
                    break;

                case DockEnum.Log:
                    IsLog = y.IsVisible;
                    break;
            }
        });
    }

    partial void OnCurrentViewModelChanged(PageViewModel? oldValue, PageViewModel? newValue)
        => OnPropertyChanged(nameof(CurrentDock));

    [RelayCommand(CanExecute = nameof(CanNavigateTo))]
    private void NavigateTo(DockEnum dock)
    {
        _navigationService.NavigateTo(dock);
        CurrentViewModel = _navigationService.Current;
    }

    private bool CanNavigateTo(DockEnum dock)
        => dock switch
        {
            DockEnum.Home => CurrentViewModel is not HomeViewModel,
            DockEnum.Materials => CurrentViewModel is not MaterialsViewModel,
            DockEnum.Storage => CurrentViewModel is not StorageViewModel,
            DockEnum.System => CurrentViewModel is not SystemViewModel,
            DockEnum.Planet => CurrentViewModel is not PlanetViewModel,
            DockEnum.MarketConnector => CurrentViewModel is not MarketConnectorViewModel,
            DockEnum.Log => CurrentViewModel is not LogViewModel,
            DockEnum.Settings => CurrentViewModel is not SettingsViewModel,
            _ => true
        };
}