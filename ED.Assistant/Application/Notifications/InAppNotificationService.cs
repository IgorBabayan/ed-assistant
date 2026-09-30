using Avalonia.Threading;
using ED.Assistant.Presentation.ViewModels.Notifications;
using Material.Icons;

namespace ED.Assistant.Application.Notifications;

public sealed class InAppNotificationService
{
    private const int MaxVisible = 4;
    private static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(8);

    public ObservableCollection<ToastViewModel> Toasts { get; } = [];

    // Safe to call from the journal watcher thread
    public void Show(string title, string message, MaterialIconKind icon = MaterialIconKind.Information,
        TimeSpan? duration = null)
    {
        Dispatcher.UIThread.Post(() =>
        {
            var toast = new ToastViewModel(title, message, icon, Dismiss);
            Toasts.Add(toast);

            while (Toasts.Count > MaxVisible)
                Toasts.RemoveAt(0);

            DispatcherTimer.RunOnce(() => Dismiss(toast), duration ?? DefaultDuration);
        });
    }

    private void Dismiss(ToastViewModel toast) => Toasts.Remove(toast);
}