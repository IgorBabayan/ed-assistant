namespace EdAssistant.Helpers.Extensions;

public static class NavigationServiceExtensions
{
    private static INavigationService RegisterViewMapping<TViewModel, TView>(this INavigationService navigationService)
        where TViewModel : BaseViewModel
        where TView : UserControl
    {
        navigationService.RegisterMapping<TViewModel, TView>();
        return navigationService;
    }

    public static void RegisterViewMappings(this INavigationService navigationService)
    {
        navigationService.RegisterViewMapping<HomeViewModel, HomeView>()
            .RegisterViewMapping<CargoViewModel, CargoView>()
            .RegisterViewMapping<MaterialsViewModel, MaterialsView>()
            .RegisterViewMapping<StorageViewModel, StorageView>()
            .RegisterViewMapping<SystemViewModel, SystemView>()
            .RegisterViewMapping<PlanetViewModel, PlanetView>()
            .RegisterViewMapping<MarketConnectorViewModel, MarketConnectorView>()
            .RegisterViewMapping<LogViewModel, LogView>()
            .RegisterViewMapping<SettingsViewModel, SettingsView>();   
    }
}