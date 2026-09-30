#if WINDOWS
using Microsoft.Toolkit.Uwp.Notifications;

namespace ED.Assistant.Application.Notifications;

sealed class WindowsDesktopNotifier : IDesktopNotifier
{
    public void Show(string title, string message)
    {
        try
        {
            new ToastContentBuilder()
                .AddText(title)
                .AddText(message)
                .Show();
        }
        catch (Exception)
        {
        }
    }
}
#endif