using ED.Assistant.Plugins;

namespace ED.Assistant.Application.Notifications;

sealed class LinuxDesktopNotifier : IDesktopNotifier
{
    public void Show(string title, string message) =>
        CommandRunner.Run("notify-send", "-a", "ED Assistant", title, message);
}