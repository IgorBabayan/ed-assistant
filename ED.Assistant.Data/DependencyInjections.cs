using ED.Assistant.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Data;

public static class DependencyInjections
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IPathFinder, PathFinder>();
    }
}
