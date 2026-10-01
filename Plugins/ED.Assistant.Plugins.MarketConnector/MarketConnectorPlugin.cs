using ED.Assistant.Plugins.MarketConnector.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Plugins.MarketConnector;

public sealed class MarketConnectorPlugin : IPlugin
{
    public string Id => "market-connector";
    public string Name => "Market Connector";
    public Version Version => typeof(MarketConnectorPlugin).Assembly.GetName().Version ?? new Version(1, 0);

    public void ConfigureServices(IServiceCollection services, IPluginContext context)
    {
        // Singletons: senders and settings pages share one store per settings type
        services.AddSingleton(context.GetSettings<EddnSettings>());
        services.AddSingleton(context.GetSettings<EdsmSettings>());

        // Transient: the host resolves a fresh one each time Settings opens
        services.AddTransient<EddnSettingsViewModel>();
        services.AddTransient<EdsmSettingsViewModel>();

        // Next: EDDN/EDSM/Inara senders as IPluginBackgroundService, subscribed to settings.Changed
    }

    public IEnumerable<PluginPage> GetPages() => [];

    public IEnumerable<PluginSettingsPage> GetSettingsPages() =>
    [
        new PluginSettingsPage("EDDN", typeof(EddnSettingsViewModel), () => new EddnSettingsView()),
        new PluginSettingsPage("EDSM", typeof(EdsmSettingsViewModel), () => new EdsmSettingsView())
    ];
}
