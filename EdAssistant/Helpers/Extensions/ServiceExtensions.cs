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
        services.AddSingleton<IJournalEventFactory, JournalEventFactory>();
        services.AddSingleton<ICelestialStructure, CelestialStructure>();
        services.AddSingleton<ITemplateCacheManager, TemplateCacheManager>();
        services.AddSingleton<IResourceService, ResourceService>();
        services.AddSingleton<IJournalService, JournalService>();
        services.AddSingleton<IEliteDangerousJournalWatcher, EliteDangerousJournalWatcher>();
        services.AddSingleton<IJournalMonitorService, JournalMonitorService>();
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