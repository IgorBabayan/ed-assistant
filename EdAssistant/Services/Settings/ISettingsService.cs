namespace EdAssistant.Services.Settings;

public interface ISettingsService
{
    T GetSetting<T>(string key, T defaultValue = default(T));
    void SetSetting<T>(string key, T value);
    void Save();
    void Load();
}