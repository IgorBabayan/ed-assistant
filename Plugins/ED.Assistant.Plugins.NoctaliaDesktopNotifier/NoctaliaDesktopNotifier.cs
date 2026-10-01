using System.Text.Json;

namespace ED.Assistant.Plugins.NoctaliaDesktopNotifier;

sealed class NoctaliaDesktopNotifier : IDesktopNotifier
{
    private const int DurationMs = 6000;
    private const string Icon = "circle-check";
    private const string AppName = "ED Assistant";
    private const string Executable = "noctalia";
    
    public static bool IsAvailable => CommandRunner.IsOnPath(Executable);

    public void Show(string title, string message)
    {
        var payload = JsonSerializer.Serialize(new
        {
            app_name = AppName,
            summary = title,
            body = message,
            urgency = "normal",
            timeout_ms = DurationMs,
            icon = Icon
        });

        CommandRunner.Run(
            "noctalia",
            "msg",
            "notification-show",
            payload);
    }
}
