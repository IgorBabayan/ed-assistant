namespace EdAssistant.Services.DockVisibility;

public class DockVisibilityChangedEventArgs(DockEnum dock, bool isVisible) : EventArgs
{
    public DockEnum Dock { get; } = dock;
    public bool IsVisible { get; } = isVisible;
}