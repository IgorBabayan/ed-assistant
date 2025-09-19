namespace EdAssistant.Services.DockVisibility;

class DockVisibilityService : IDockVisibilityService
{
    private readonly Dictionary<PageEnum, bool> _dockVisibility = new()
    {
        [PageEnum.Cargo] = true,
        [PageEnum.Materials] = true,
        [PageEnum.ShipLocker] = true,
        [PageEnum.System] = true,
        [PageEnum.Planet] = true,
        [PageEnum.MarketConnector] = true,
        [PageEnum.Log] = true
    };
    
    private readonly ISettingsService _settingsService;
    private const string DOCK_VISIBILITY_PREFIX = "DockVisibility";

    public event EventHandler<DockVisibilityChangedEventArgs>? VisibilityChanged;

    public DockVisibilityService(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        LoadSettings();
    }

    public bool GetVisibility(PageEnum page) => _dockVisibility.GetValueOrDefault(page, true);

    public void SetVisibility(PageEnum page, bool isVisible)
    {
        if (_dockVisibility.GetValueOrDefault(page) == isVisible)
            return;

        _dockVisibility[page] = isVisible;
        SaveSettings();
        VisibilityChanged?.Invoke(this, new DockVisibilityChangedEventArgs(page, isVisible));
    }

    public void SaveSettings()
    {
        foreach (var kvp in _dockVisibility)
        {
            var settingKey = $"{DOCK_VISIBILITY_PREFIX}.{kvp.Key}";
            _settingsService.SetSetting(settingKey, kvp.Value);
        }
    }

    public void LoadSettings()
    {
        foreach (var page in _dockVisibility.Keys.ToList())
        {
            var settingKey = $"{DOCK_VISIBILITY_PREFIX}.{page}";
            var defaultValue = _dockVisibility[page];
            _dockVisibility[page] = _settingsService.GetSetting(settingKey, defaultValue);
        }
    }
}