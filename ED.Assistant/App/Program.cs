using Avalonia.Dialogs;
using ED.Assistant.Application.Path;

namespace ED.Assistant.App;

internal static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        finally
        {
            (Avalonia.Application.Current as App)?.DisposeServices();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer,
    // which finds it by reflection, so it has to stay public.
    // ReSharper disable once MemberCanBePrivate.Global
    public static AppBuilder BuildAvaloniaApp()
    {
        var builder = AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();

        if (!OperatingSystem.IsLinux())
            return builder;

        builder = builder.With(new X11PlatformOptions { WmClass = "ed-assistant" });
        if (DesktopEnvironmentHelper.IsHyprland())
            builder = builder.UseManagedSystemDialogs();

        return builder;
    }
}
