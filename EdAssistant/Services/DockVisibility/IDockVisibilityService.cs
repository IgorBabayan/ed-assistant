namespace EdAssistant.Services.DockVisibility;

public interface IDockVisibilityService
{
    bool GetVisibility(DockEnum dock);
    void SetVisibility(DockEnum dock, bool isVisible);
    void SaveSettings();
    void LoadSettings();
    event EventHandler<DockVisibilityChangedEventArgs> VisibilityChanged;
}