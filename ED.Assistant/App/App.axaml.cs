using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ED.Assistant.Data;
using ED.Assistant.Data.Seed;
using ED.Assistant.Extensions;
using ED.Assistant.Helpers;
using ED.Assistant.Presentation.ViewModels.Shell;
using ED.Assistant.Presentation.Views.Shell;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.App.App;

public partial class App : Avalonia.Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override async void OnFrameworkInitializationCompleted()
    {
        // Create service collection and register services from ED.Assistant.Data
        var services = new ServiceCollection();
        services.RegisterDataServices()
			.RegisterServices()
			.RegisterViewModels()
            .RegisterWindows()
            .AddMemoryCache()
            .AddDbContext<AppDbContext>(options =>
			{
				var dbPath = DbPathProvider.GetDatabasePath();
				options.UseSqlite($"Data Source={dbPath}");
			});

		// Build provider and keep a reference to it for later use.
		var provider = services.BuildServiceProvider();

		using (var scope = provider.CreateScope())
		{
			var seeder = scope.ServiceProvider.GetRequiredService<IBioDataSeeder>();
			await seeder.SeedAsync();
		}

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
			// Resolve the MainWindowViewModel from DI and assign as DataContext
			desktop.MainWindow = new MainWindow
            {
                DataContext = provider.GetRequiredService<MainWindowViewModel>(),
            };
		}

        base.OnFrameworkInitializationCompleted();
    }
}
