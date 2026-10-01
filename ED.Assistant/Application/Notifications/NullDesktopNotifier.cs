using ED.Assistant.Plugins;

namespace ED.Assistant.Application.Notifications;

sealed class NullDesktopNotifier : IDesktopNotifier
{
    public void Show(string title, string message) { }
}
