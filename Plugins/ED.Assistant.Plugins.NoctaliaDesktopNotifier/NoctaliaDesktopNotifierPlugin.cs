using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Plugins.NoctaliaDesktopNotifier;

public sealed class NoctaliaDesktopNotifierPlugin : IPlugin
{
    public string Id => "noctalia-desktop-notifier";
    public string Name => "Noctalia Notifications";
    public Version Version => typeof(NoctaliaDesktopNotifierPlugin).Assembly.GetName().Version ?? new Version(1, 0);

    public void ConfigureServices(IServiceCollection services, IPluginContext context)
    {
        // Without Noctalia, keep the host's default notifier (notify-send / Hyprland)
        if (!OperatingSystem.IsLinux() || !NoctaliaDesktopNotifier.IsAvailable)
            return;

        // Registered after the host's notifier, so this one is resolved for IDesktopNotifier
        services.AddSingleton<IDesktopNotifier, NoctaliaDesktopNotifier>();
    }

    public IEnumerable<PluginPage> GetPages() => [];
}
