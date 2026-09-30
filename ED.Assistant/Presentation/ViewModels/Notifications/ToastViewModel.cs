using Material.Icons;

namespace ED.Assistant.Presentation.ViewModels.Notifications;

public sealed partial class ToastViewModel(string title, string message, MaterialIconKind icon,
    Action<ToastViewModel> dismiss) : ObservableObject
{
    public string Title { get; } = title;
    public string Message { get; } = message;
    public MaterialIconKind Icon { get; } = icon;

    [RelayCommand]
    private void Close() => dismiss(this);
}