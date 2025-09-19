namespace EdAssistant.Services.Settings;

class JsonSettingsService : ISettingsService
{
    private readonly ILogger<JsonSettingsService> _logger;
    private readonly Dictionary<string, JsonElement> _settings = new();
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options;

    public JsonSettingsService(ILogger<JsonSettingsService> logger)
    {
        _logger = logger;
        
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appDataPath, "EdAssistant");
        Directory.CreateDirectory(appFolder);
        _filePath = Path.Combine(appFolder, "settings.json");

        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        Load();
    }

    public T GetSetting<T>(string key, T defaultValue = default(T))
    {
        if (!_settings.TryGetValue(key, out var jsonElement))
        {
            return defaultValue;
        }

        try
        {
            if (jsonElement.ValueKind == JsonValueKind.Null)
            {
                return defaultValue;
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)jsonElement.GetString()!;
            }

            if (typeof(T) == typeof(int) || typeof(T) == typeof(int?))
            {
                return (T)(object)jsonElement.GetInt32();
            }

            if (typeof(T) == typeof(bool) || typeof(T) == typeof(bool?))
            {
                return (T)(object)jsonElement.GetBoolean();
            }

            if (typeof(T) == typeof(double) || typeof(T) == typeof(double?))
            {
                return (T)(object)jsonElement.GetDouble();
            }

            if (typeof(T) == typeof(decimal) || typeof(T) == typeof(decimal?))
            {
                return (T)(object)jsonElement.GetDecimal();
            }

            if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
            {
                return (T)(object)jsonElement.GetDateTime();
            }

            return JsonSerializer.Deserialize<T>(jsonElement.GetRawText(), _options) ?? defaultValue;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, Localization.Instance["JsonSettingsService.Exceptions.FailedToReadSettingsFile"]);
            return defaultValue;
        }
    }

    public void SetSetting<T>(string key, T value)
    {
        if (value == null)
        {
            _settings[key] = JsonDocument.Parse("null").RootElement;
        }
        else
        {
            var json = JsonSerializer.Serialize(value, _options);
            _settings[key] = JsonDocument.Parse(json).RootElement;
        }
        Save();
    }

    public void Save()
    {
        try
        {
            var serializableSettings = new Dictionary<string, object?>();
            foreach (var kvp in _settings)
            {
                serializableSettings[kvp.Key] = ConvertJsonElementToObject(kvp.Value);
            }

            var json = JsonSerializer.Serialize(serializableSettings, _options);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, Localization.Instance["JsonSettingsService.Exceptions.FailedToSaveSettingsFile"]);
            throw;
        }
    }

    public void Load()
    {
        if (!File.Exists(_filePath))
            return;

        try
        {
            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return;

            using var document = JsonDocument.Parse(json);
            _settings.Clear();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                _settings[property.Name] = property.Value.Clone();
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, Localization.Instance["JsonSettingsService.Exceptions.FailedToLoadSettingsFile"]);
        }
    }

    private static object? ConvertJsonElementToObject(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt32(out var intVal) ? intVal : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            JsonValueKind.Array => element.EnumerateArray().Select(ConvertJsonElementToObject).ToArray(),
            JsonValueKind.Object => element.EnumerateObject().ToDictionary(p => p.Name, p => ConvertJsonElementToObject(p.Value)),
            _ => element.GetRawText()
        };
    }
}