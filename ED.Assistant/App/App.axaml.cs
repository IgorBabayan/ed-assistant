using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ED.Assistant.Data;
using ED.Assistant.Data.Storage;
using ED.Assistant.Extensions;
using ED.Assistant.Presentation.ViewModels.Shell;
using ED.Assistant.Presentation.Views.Shell;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.App;

public partial class App : Avalonia.Application
{
    private ServiceProvider? _provider;
    private MainWindowViewModel? _mainViewModel;

    internal void DisposeServices()
    {
        if (_provider is null)
            return;

        _mainViewModel?.Dispose();
        // Stop background database work before disposing its dependencies.
        if (_mainViewModel is not null)
            _provider.GetRequiredService<IJournalWatchService>().Dispose();
        _provider.Dispose();
        ED.Assistant.Presentation.Converters.BiologyImageConverter.ClearCache();
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
                DataContext = _mainViewModel = provider.GetRequiredService<MainWindowViewModel>(),
            };
		}

        base.OnFrameworkInitializationCompleted();
    }
}
