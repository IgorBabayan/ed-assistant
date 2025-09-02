using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EdAssistant.Services.Settings;

class JsonSettingsService : ISettingsService
{
    private readonly Dictionary<string, object> _settings = new();
    private readonly string _filePath;

    public JsonSettingsService()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "EdAssistant");
        Directory.CreateDirectory(appFolder);
        _filePath = Path.Combine(appFolder, "settings.json");

        Load();
    }

    public T GetSetting<T>(string key, T defaultValue = default(T))
    {
        if (_settings.TryGetValue(key, out var value))
        {
            if (value is JsonElement jsonElement)
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(jsonElement.GetRawText());
                }
                catch
                {
                    return defaultValue;
                }
            }

            if (value is T directValue)
            {
                return directValue;
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
        return defaultValue;
    }

    public void SetSetting<T>(string key, T value)
    {
        _settings[key] = value;
        Save();
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public void Load()
    {
        if (!File.Exists(_filePath))
            return;

        var json = File.ReadAllText(_filePath);
        var loaded = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        if (loaded == null) 
            return;

        _settings.Clear();
        foreach (var kvp in loaded)
        {
            _settings[kvp.Key] = kvp.Value;
        }
    }
}