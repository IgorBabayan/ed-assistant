namespace EdAssistant.Services.Navigate;

sealed class NavigationService(IServiceProvider _serviceProvider) : INavigationService
{
    public event EventHandler? CurrentChanged;
    public PageViewModel? Current { get; private set; } = _serviceProvider.GetRequiredService<HomeViewModel>();

    public void NavigateTo(DockEnum dock)
    {
        Current = dock switch
        {
            DockEnum.Home => _serviceProvider.GetRequiredService<HomeViewModel>(),
            DockEnum.Cargo => _serviceProvider.GetRequiredService<CargoViewModel>(),
            DockEnum.Materials => _serviceProvider.GetRequiredService<MaterialsViewModel>(),
            DockEnum.ShipLocker => _serviceProvider.GetRequiredService<StorageViewModel>(),
            DockEnum.System => _serviceProvider.GetRequiredService<SystemViewModel>(),
            DockEnum.Planet => _serviceProvider.GetRequiredService<PlanetViewModel>(),
            DockEnum.MarketConnector => _serviceProvider.GetRequiredService<MarketConnectorViewModel>(),
            DockEnum.Log => _serviceProvider.GetRequiredService<LogViewModel>(),
            DockEnum.Settings => _serviceProvider.GetRequiredService<SettingsViewModel>(),
            _ => Current
        };
        CurrentChanged?.Invoke(this, EventArgs.Empty);
    }
}