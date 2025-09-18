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
    private readonly string _settingsPath;

    public event EventHandler<DockVisibilityChangedEventArgs>? VisibilityChanged;

    public DockVisibilityService()
    {
        _settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EdAssistant", "settings.json");

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
        Directory.CreateDirectory(Path.GetDirectoryName(_settingsPath)!);
        var json = JsonSerializer.Serialize(_dockVisibility);
        File.WriteAllText(_settingsPath, json);
    }

    public void LoadSettings()
    {
        if (!File.Exists(_settingsPath))
            return;

        var json = File.ReadAllText(_settingsPath);
        var loaded = JsonSerializer.Deserialize<Dictionary<PageEnum, bool>>(json);
        if (loaded == null)
            return;

        _dockVisibility.Clear();
        foreach (var kvp in loaded)
        {
            _dockVisibility[kvp.Key] = kvp.Value;
        }
    }
}