using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ED.Assistant.Extensions;
using ED.Assistant.Presentation.Views.Shell;
using ED.Assistant.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.App.App;

public partial class App : Avalonia.Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        // Create service collection and register services from ED.Assistant.Data
        var services = new ServiceCollection();
        services.RegisterDataServices()
			.RegisterServices()
			.RegisterViewModels()
            .RegisterWindows()
            .AddMemoryCache();

        // Build provider and keep a reference to it for later use.
        var provider = services.BuildServiceProvider();

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
