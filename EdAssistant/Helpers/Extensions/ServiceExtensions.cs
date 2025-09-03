using EdAssistant.Services.Desktop;
using EdAssistant.Services.DockVisibility;
using EdAssistant.Services.GameData;
using EdAssistant.Services.Navigate;
using EdAssistant.Services.Settings;
using EdAssistant.Services.Storage;
using EdAssistant.ViewModels;
using EdAssistant.ViewModels.Pages;
using EdAssistant.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EdAssistant.Helpers.Extensions;

public static class ServiceExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IDesktopService, DesktopService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IFolderPickerService, FolderPickerService>();
        services.AddSingleton<IDockVisibilityService, DockVisibilityService>();
        services.AddSingleton<ISettingsService, JsonSettingsService>();
        services.AddSingleton<IGameDataService, GameDataService>();
    }

    public static void RegisterViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<CargoViewModel>();
        services.AddSingleton<MaterialsViewModel>();
        services.AddSingleton<StorageViewModel>();
        services.AddSingleton<SystemViewModel>();
        services.AddSingleton<PlanetViewModel>();
        services.AddSingleton<MarketConnectorViewModel>();
        services.AddSingleton<LogViewModel>();
        services.AddSingleton<SettingsViewModel>();
    }

    public static void RegisterViews(this IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddSingleton<HomeView>();
        services.AddSingleton<CargoView>();
        services.AddSingleton<MaterialsView>();
        services.AddSingleton<StorageView>();
        services.AddSingleton<SystemView>();
        services.AddSingleton<PlanetView>();
        services.AddSingleton<MarketConnectorView>();
        services.AddSingleton<LogView>();
        services.AddSingleton<SettingsView>();
    }
}