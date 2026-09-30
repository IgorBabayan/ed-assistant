namespace ED.Assistant.Application.Notifications;

public interface IDesktopNotifier
{
    void Show(string title, string message);
}

sealed class NullDesktopNotifier : IDesktopNotifier
{
    public void Show(string title, string message) { }
}