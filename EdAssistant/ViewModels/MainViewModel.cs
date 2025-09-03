using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EdAssistant.Helpers.Attributes;
using EdAssistant.Models.Enums;
using EdAssistant.Services.Desktop;
using EdAssistant.Services.DockVisibility;
using EdAssistant.Services.Navigate;
using EdAssistant.Translations;
using EdAssistant.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EdAssistant.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IDockVisibilityService _dockVisibilityService;
    private readonly IDesktopService _desktopService;

    private static readonly Dictionary<Type, DockEnum> _viewModelToDockCache = new();
    private static readonly Dictionary<DockEnum, Type> _dockToViewModelCache = new();

    public string WindowTitle => Localization.Instance["MainWindow.Title"];

    public DockEnum CurrentDock
    {
        get
        {
            if (CurrentViewModel == null)
                return default;

            var viewModelType = CurrentViewModel.GetType();
            return _viewModelToDockCache.GetValueOrDefault(viewModelType);
        }
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(NavigateToCommand))]
    private PageViewModel? currentViewModel;

    public bool IsCargo => _dockVisibilityService.GetVisibility(DockEnum.Cargo);
    public bool IsMaterials => _dockVisibilityService.GetVisibility(DockEnum.Materials);
    public bool IsStorage => _dockVisibilityService.GetVisibility(DockEnum.ShipLocker);
    public bool IsSystem => _dockVisibilityService.GetVisibility(DockEnum.System);
    public bool IsPlanet => _dockVisibilityService.GetVisibility(DockEnum.Planet);
    public bool IsMarketConnector => _dockVisibilityService.GetVisibility(DockEnum.MarketConnector);
    public bool IsLog => _dockVisibilityService.GetVisibility(DockEnum.Log);
    public bool CanCreateDesktopFile => OperatingSystem.IsLinux();

    public bool IsPlanetarySystem => IsSystem || IsPlanet;
    public bool IsInventory => IsCargo || IsMaterials || IsStorage;

    static MainViewModel() => InitializeMappings();

    public MainViewModel(INavigationService navigationService, IDockVisibilityService dockVisibilityService, IDesktopService desktopService)
    {
        _navigationService = navigationService;
        _dockVisibilityService = dockVisibilityService;
        _desktopService = desktopService;
        CurrentViewModel = _navigationService.Current;

        _dockVisibilityService.VisibilityChanged += OnDockVisibilityChanged;
    }

    private static void InitializeMappings()
    {
        var viewModelTypes = typeof(PageViewModel).Assembly
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(PageViewModel)) &&
                        t.GetCustomAttribute<DockMappingAttribute>() != null);

        foreach (var type in viewModelTypes)
        {
            var attribute = type.GetCustomAttribute<DockMappingAttribute>();
            if (attribute is not null)
            {
                _viewModelToDockCache[type] = attribute.Dock;
                _dockToViewModelCache[attribute.Dock] = type;
            }
        }
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
    {
        if (CurrentViewModel is null)
            return true;

        var currentViewModelType = CurrentViewModel.GetType();
        var targetViewModelType = _dockToViewModelCache.GetValueOrDefault(dock);

        return currentViewModelType != targetViewModelType;
    }

    private void OnDockVisibilityChanged(object? sender, DockVisibilityChangedEventArgs e)
    {
        OnPropertyChanged(e.Dock switch
        {
            DockEnum.Cargo => nameof(IsCargo),
            DockEnum.Materials => nameof(IsMaterials),
            DockEnum.ShipLocker => nameof(IsStorage),
            DockEnum.System => nameof(IsSystem),
            DockEnum.Planet => nameof(IsPlanet),
            DockEnum.MarketConnector => nameof(IsMarketConnector),
            DockEnum.Log => nameof(IsLog),
            _ => string.Empty
        });

        if (e.Dock is DockEnum.System or DockEnum.Planet)
        {
            OnPropertyChanged(nameof(IsPlanetarySystem));
        }

        if (e.Dock is DockEnum.Cargo or DockEnum.Materials or DockEnum.ShipLocker)
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
}