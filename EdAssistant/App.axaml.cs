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
