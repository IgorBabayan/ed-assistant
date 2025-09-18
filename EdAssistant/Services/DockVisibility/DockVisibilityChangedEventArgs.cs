namespace EdAssistant.Services.DockVisibility;

public class DockVisibilityChangedEventArgs(PageEnum page, bool isVisible) : EventArgs
{
    public PageEnum Page { get; } = page;
    public bool IsVisible { get; } = isVisible;
}