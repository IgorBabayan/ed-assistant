using ED.Assistant.Data.Services.Path;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Data;

public static class DependencyInjections
{
    public static void RegisterServices(this IServiceCollection services)
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

        services.AddSingleton<IPathFinder, PathFinder>();
    }
}
