using ED.Assistant.Data.Services;
using ED.Assistant.Data.Services.Events;
using ED.Assistant.Data.Services.Path;
using ED.Assistant.Data.Services.Settings;
using Microsoft.Extensions.DependencyInjection;
using static ED.Assistant.Data.Services.IJournalEventDispatcher;

namespace ED.Assistant.Data;

public static class DependencyInjections
{
    public static IServiceCollection RegisterDataServices(this IServiceCollection services)
    {
        services.AddSingleton<WindowsPathResolver>();
        services.AddSingleton<LinuxPathResolver>();
        services.AddSingleton<MacPathResolver>();

        services.AddSingleton<IPlatformPathResolver>(sp =>
        {
            if (OperatingSystem.IsWindows())
                return sp.GetRequiredService<WindowsPathResolver>();

            if (OperatingSystem.IsLinux())
                return sp.GetRequiredService<LinuxPathResolver>();

            if (OperatingSystem.IsMacOS())
                return sp.GetRequiredService<MacPathResolver>();

            throw new PlatformNotSupportedException("Unsupported OS");
        });

        services.AddSingleton<IPathFinder, PathFinder>()
            .AddSingleton<ISettingsStorage, SettingsStorage>()
            .AddSingleton<ILogStorage, LogStorage>();
        return services;
    }
}
