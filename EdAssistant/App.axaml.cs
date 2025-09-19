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
            var window = _host.Services.GetRequiredService<MainWindow>();
            window.DataContext = _host.Services.GetRequiredService<MainViewModel>();
            desktop.MainWindow = window;
            
            var navigationService = _host.Services.GetRequiredService<INavigationService>();
            navigationService.Initialize(window.NavigationHost);
            navigationService.RegisterViewMappings();
        
            var journalIntegrationService = _host.Services.GetRequiredService<IJournalIntegrationService>();
            window.Loaded += async (_, _) =>
            {
                await navigationService.NavigateAsync<HomeViewModel>();
                await journalIntegrationService.InitializeAsync();
            };
            window.Closing += (_, _) => journalIntegrationService.Dispose();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
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
                services.AddLogging(x =>
                {
                    x.ClearProviders();
                    x.AddDebug();
                    x.SetMinimumLevel(LogLevel.Warning);
                    x.AddFilter("Avalonia", LogLevel.Warning);
                });

                services.AddMemoryCache();
                services.RegisterServices();
                services.RegisterViewModels();
                services.RegisterViews();
            })
            .Build();
    }
}
