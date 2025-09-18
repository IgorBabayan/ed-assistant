namespace EdAssistant.Services.DockVisibility;

public interface IDockVisibilityService
{
    bool GetVisibility(PageEnum page);
    void SetVisibility(PageEnum page, bool isVisible);
    void SaveSettings();
    void LoadSettings();
    event EventHandler<DockVisibilityChangedEventArgs> VisibilityChanged;
}