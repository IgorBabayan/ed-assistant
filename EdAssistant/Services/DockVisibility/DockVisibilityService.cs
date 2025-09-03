using EdAssistant.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EdAssistant.Services.DockVisibility;

class DockVisibilityService : IDockVisibilityService
{
    private readonly Dictionary<DockEnum, bool> _dockVisibility = new()
    {
        [DockEnum.Cargo] = true,
        [DockEnum.Materials] = true,
        [DockEnum.ShipLocker] = true,
        [DockEnum.System] = true,
        [DockEnum.Planet] = true,
        [DockEnum.MarketConnector] = true,
        [DockEnum.Log] = true
    };
    private readonly string _settingsPath;

    public event EventHandler<DockVisibilityChangedEventArgs>? VisibilityChanged;

    public DockVisibilityService()
    {
        _settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EdAssistant", "settings.json");

        LoadSettings();
    }

    public bool GetVisibility(DockEnum dock) => _dockVisibility.GetValueOrDefault(dock, true);

    public void SetVisibility(DockEnum dock, bool isVisible)
    {
        if (_dockVisibility.GetValueOrDefault(dock) == isVisible)
            return;

        _dockVisibility[dock] = isVisible;
        SaveSettings();
        VisibilityChanged?.Invoke(this, new DockVisibilityChangedEventArgs(dock, isVisible));
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
        var loaded = JsonSerializer.Deserialize<Dictionary<DockEnum, bool>>(json);
        if (loaded == null)
            return;

        _dockVisibility.Clear();
        foreach (var kvp in loaded)
        {
            _dockVisibility[kvp.Key] = kvp.Value;
        }
    }
}