using ED.Assistant.Plugins;

namespace ED.Assistant.Application.Notifications;

sealed class MacDesktopNotifier : IDesktopNotifier
{
    public void Show(string title, string message) =>
        CommandRunner.Run("osascript", "-e",
            $"display notification \"{Escape(message)}\" with title \"{Escape(title)}\"");

    private static string Escape(string s) =>
        s.Replace("\\", "\\\\").Replace("\"", "\\\"");
}