using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using EdAssistant.Services.Navigate;
using EdAssistant.Translations;
using EdAssistant.ViewModels;
using EdAssistant.ViewModels.Pages;
using EdAssistant.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace EdAssistant;

public partial class App : Application
{
    private IHost _host = null!;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        Localization.Instance.UseLanguage("en");
        RequestedThemeVariant = ThemeVariant.Dark;
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        ConfigureServices();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            /*desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };*/
            var window = _host.Services.GetRequiredService<MainWindow>();
            window.DataContext = _host.Services.GetRequiredService<MainViewModel>();
            desktop.MainWindow = window;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            /*singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };*/
            var view = new MainView
            {
                DataContext = _host.Services.GetRequiredService<MainViewModel>()
            };
            singleViewPlatform.MainView = view;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<INavigationService, NavigationService>();

                services.AddSingleton<MainViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<MaterialsViewModel>();
                services.AddTransient<StorageViewModel>();
                services.AddTransient<SystemViewModel>();
                services.AddTransient<PlanetViewModel>();
                services.AddTransient<MarketConnectorViewModel>();
                services.AddTransient<LogViewModel>();
                services.AddTransient<SettingsViewModel>();

                services.AddSingleton<MainWindow>();
                services.AddTransient<HomeView>();
                services.AddTransient<MaterialsView>();
            })
            .Build();
    }
}
