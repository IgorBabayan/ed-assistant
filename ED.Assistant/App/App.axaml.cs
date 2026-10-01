using System.Diagnostics;
using System.IO;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ED.Assistant.Application.Path;
using ED.Assistant.Application.Plugins;
using ED.Assistant.Application.Settings;
using ED.Assistant.Data;
using ED.Assistant.Data.Storage;
using ED.Assistant.Domain.Config;
using ED.Assistant.Extensions;
using ED.Assistant.Plugins;
using ED.Assistant.Presentation.ViewModels.Plugin;
using ED.Assistant.Presentation.ViewModels.Shell;
using ED.Assistant.Presentation.Views.Shell;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.App;

public class App : Avalonia.Application
{
    private readonly CancellationTokenSource _pluginLifetime = new();
    private ServiceProvider? _provider;
    private MainWindowViewModel? _mainViewModel;

    internal void DisposeServices()
    {
        if (_provider is null)
            return;

        // Stop plugin background work first: it may still be reading journals or writing to the DB
        _pluginLifetime.Cancel();

        _mainViewModel?.Dispose();
        // Stop background database work before disposing its dependencies.
        if (_mainViewModel is not null)
            _provider.GetRequiredService<IJournalWatchService>().Dispose();
        _provider.Dispose();
        Presentation.Converters.BiologyImageConverter.ClearCache();
        _provider = null;
    }

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        // Create service collection and register services from ED.Assistant.Data
        var services = new ServiceCollection();
        services.RegisterDataServices()
			.RegisterServices()
			.RegisterViewModels()
            .RegisterWindows()
            .AddMemoryCache()
            .AddDbContext<AppDbContext>((sp, options) =>
			{
                var dbPathProvider = sp.GetRequiredService<IDbPathProvider>();
				options.UseSqlite(new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder
                { DataSource = dbPathProvider.GetDatabasePath() }.ToString());
			})
			.RegisterDbServices();

        RegisterPlugins(services);

		// Build provider and keep a reference to it for later use.
		var provider = _provider = services.BuildServiceProvider();

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Synchronous on purpose: MainWindow must be assigned before this method returns,
            // otherwise the classic desktop lifetime starts without a window (async void also
            // turned any migration exception into an unobserved crash).
	        using (var scope = provider.CreateScope())
	        {
		        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
	        }

			// Resolve the MainWindowViewModel from DI and assign as DataContext
			desktop.MainWindow = new MainWindow
            {
                DataContext = _mainViewModel = provider.GetRequiredService<MainWindowViewModel>()
            };

            // After the host migration, so plugins can add their tables to an up-to-date database
            StartPluginServices(provider);
		}

        base.OnFrameworkInitializationCompleted();
    }
    
    private static AppSettings LoadStartupSettings()
    {
	    try
	    {
		    var pathFinder = new PathFinder(PlatformPathResolverFactory.Create());
		    return new SettingsStorage().Load(pathFinder.GetConfigPath());
	    }
	    catch (Exception ex)
	    {
		    // A broken or unreadable config must not stop the app: every addon counts as enabled
		    Trace.WriteLine($"Can't read settings at startup: {ex}");
		    return new AppSettings();
	    }
    }

    private static void RegisterPlugins(IServiceCollection services)
    {
        var pluginsRoot = IOPath.Combine(
	        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ed-assistant", "plugins");

        var catalog = PluginCatalog.Create(pluginsRoot);
        // DbPathProvider has no dependencies; needed here because the container isn't built yet
        var databasePath = new DbPathProvider().GetDatabasePath();

        var startupSettings = LoadStartupSettings();
        foreach (var loaded in catalog.LoadEnabled(startupSettings.IsAddonEnabled))
        {
	        try
	        {
		        var dataDir = Directory.CreateDirectory(
			        IOPath.Combine(pluginsRoot, "_data", loaded.Plugin.Id)).FullName;

		        // Register into a scratch collection first: a plugin that throws halfway
		        // through ConfigureServices leaves nothing behind in the host container
		        var pluginServices = new ServiceCollection();
		        loaded.Plugin.ConfigureServices(pluginServices,
			        new PluginContext(loaded.Directory, dataDir, databasePath));

		        foreach (var descriptor in pluginServices)
			        services.Add(descriptor);

		        services.AddSingleton(loaded);
	        }
	        catch (Exception ex)
	        {
		        catalog.MarkFailed(loaded.Directory, ex);
		        Trace.WriteLine($"Plugin in '{loaded.Directory}' failed to register services: {ex}");
	        }
        }

        services.AddSingleton<IPluginCatalog>(catalog);
        services.AddSingleton<IPluginHost, PluginHost>();
        services.AddSingleton<IPluginRegistry, PluginRegistry>();
    }

    private void StartPluginServices(IServiceProvider provider)
    {
	    List<IPluginBackgroundService> pluginServices;
	    try
	    {
		    pluginServices = provider.GetServices<IPluginBackgroundService>().ToList();
	    }
	    catch (Exception ex)
	    {
		    Trace.WriteLine($"Plugin background services could not be created: {ex}");
		    return;
	    }

	    foreach (var service in pluginServices)
		    _ = RunPluginServiceAsync(service, _pluginLifetime.Token);
    }

    private static async Task RunPluginServiceAsync(IPluginBackgroundService service, CancellationToken cancellationToken)
    {
	    try
	    {
		    // Task.Run: a plugin doing synchronous work before its first await must not block the UI
		    await Task.Run(() => service.StartAsync(cancellationToken), cancellationToken);
	    }
	    catch (OperationCanceledException)
	    {
	    }
	    catch (Exception ex)
	    {
		    Trace.WriteLine($"Plugin service {service.GetType().FullName} failed: {ex}");
	    }
    }
}
