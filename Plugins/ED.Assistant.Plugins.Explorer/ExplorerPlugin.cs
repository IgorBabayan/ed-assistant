using ED.Assistant.Plugins.Explorer.Data;
using ED.Assistant.Plugins.Explorer.Services;
using ED.Assistant.Plugins.Explorer.ViewModels;
using ED.Assistant.Plugins.Explorer.Views;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Plugins.Explorer;

public sealed class ExplorerPlugin : IPlugin
{
    public string Id => "explorer";
    public string Name => "Explorer";
    public Version Version => typeof(ExplorerPlugin).Assembly.GetName().Version ?? new Version(1, 0);

    public void ConfigureServices(IServiceCollection services, IPluginContext context)
    {
        services.AddSingleton(new ExplorerDbFactory(context.DatabasePath));

        services.AddSingleton<ExplorerSyncService>();
        services.AddSingleton<IPluginBackgroundService>(sp => sp.GetRequiredService<ExplorerSyncService>());

        services.AddSingleton<ExplorerViewModel>();
    }

    public IEnumerable<PluginPage> GetPages() =>
    [
        new PluginPage("Explorer", MaterialIconKind.Telescope, typeof(ExplorerViewModel), () => new ExplorerView())
    ];
}
