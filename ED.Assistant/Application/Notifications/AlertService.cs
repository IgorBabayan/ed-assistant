using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using ED.Assistant.Plugins;
using Material.Icons;

namespace ED.Assistant.Application.Notifications;

public sealed class AlertService(InAppNotificationService toasts, IDesktopNotifier desktop)
{
    public void Notify(string title, string message,
        MaterialIconKind icon = MaterialIconKind.Information)
    {
        toasts.Show(title, message, icon);

        Dispatcher.UIThread.Post(() =>
        {
            var window = (Avalonia.Application.Current?.ApplicationLifetime
                as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

            if (window is { IsActive: true })
                return;

            desktop.Show(title, message);
        });
    }
}