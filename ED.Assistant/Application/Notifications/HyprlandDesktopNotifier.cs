using ED.Assistant.Plugins;

namespace ED.Assistant.Application.Notifications;

sealed class HyprlandDesktopNotifier : IDesktopNotifier
{
    private const string Icon = "1";
    private const string DurationMs = "6000";
    private const string Color = "rgb(66ee99)";

    public static bool IsRunning =>
        !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HYPRLAND_INSTANCE_SIGNATURE"));

    public void Show(string title, string message)
    {
        var text = EscapeLuaString($"{title}\n{message}");

        var lua =
            $"hl.notification.create({{ " +
            $"text = \"{text}\", " +
            $"timeout = {DurationMs}, " +
            $"icon = \"{Icon}\", " +
            $"color = \"{Color}\" " +
            $"}})";

        CommandRunner.Run("hyprctl", "eval", lua);
    }

    private static string EscapeLuaString(string value) =>
        value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n");
}