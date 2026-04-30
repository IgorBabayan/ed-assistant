using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using ED.Assistant.ViewModels;
using ED.Assistant.Views;
using ED.Assistant.Services.DialogService;
using ED.Assistant.Extensions;

namespace ED.Assistant;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        // Create service collection and register services from ED.Assistant.Data
        var services = new ServiceCollection();
        services.RegisterDataServices()
            .RegisterViewModels()
			.RegisterServices();

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
