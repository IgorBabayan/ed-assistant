using Avalonia;
using Avalonia.Dialogs;
using ED.Assistant.Application.Path;

namespace ED.Assistant.App;

internal sealed class Program
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

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        var builder = AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();

        if (OperatingSystem.IsLinux())
        {
            builder = builder.With(new X11PlatformOptions { WmClass = "ed-assistant" });
            if (DesktopEnvironmentHelper.IsHyprland())
                builder = builder.UseManagedSystemDialogs();
        }
        return builder;
    }
}
