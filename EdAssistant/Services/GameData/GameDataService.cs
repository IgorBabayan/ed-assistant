using ShipLockerEvent = EdAssistant.Models.ShipLocker.ShipLockerEvent;

namespace EdAssistant.Services.GameData;

public class GameDataService(ILogger<GameDataService> logger, IMemoryCache cache) : IGameDataService
{
    private readonly Dictionary<Type, string> _fileMapping = new()
    {
        { typeof(ShipLockerEvent), "ShipLocker.json" },
        { typeof(CargoEvent), "Cargo.json" },
        { typeof(MarketData), "Market.json" },
    };

    private static string GetCacheKey<T>() => $"GameData_{typeof(T).Name}";
    private static string GetCacheKey(Type type) => $"GameData_{type.Name}";

    public event EventHandler<GameDataLoadedEventArgs>? DataLoaded;

    public async Task<object?> LoadDataAsync(Type type, string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                logger.LogWarning(Localization.Instance["Exceptions.FileNotFound"], filePath);
                return null;
            }

            var json = await File.ReadAllTextAsync(filePath);
            var data = JsonSerializer.Deserialize(json, type);
            
            if (data is not null)
            {
                DataLoaded?.Invoke(this, new GameDataLoadedEventArgs(type, data));
                logger.LogInformation(Localization.Instance["Settings.SuccessfullyLoadFileData"], type.Name, filePath);
            }

            return data;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["Exceptions.FailedToLoadFileData"], filePath);
            return null;
        }
    }

    public async Task LoadAllGameDataAsync(string journalsFolder)
    {
        var files = Directory.GetFiles(journalsFolder, "*.json");

        foreach (var (type, fileName) in _fileMapping)
        {
            var file = files.FirstOrDefault(x =>
                Path.GetFileName(x).Equals(fileName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(file))
            {
                var fileInfo = new FileInfo(file);
                var cacheKey = GetCacheKey(type);
                var lastWriteKey = $"{cacheKey}_LastWrite";

                var cachedLastWrite = cache.Get<DateTime?>(lastWriteKey);
                var needsReload = !cache.TryGetValue(cacheKey, out _) ||
                                  cachedLastWrite != fileInfo.LastWriteTime;

                if (needsReload)
                {
                    var data = await LoadDataAsync(type, file);
                    if (data is not null)
                    {
                        cache.Set(cacheKey, data, TimeSpan.FromHours(1));
                        cache.Set(lastWriteKey, fileInfo.LastWriteTime, TimeSpan.FromHours(1));

                        DataLoaded?.Invoke(this, new GameDataLoadedEventArgs(type, data));
                    }
                }
            }
        }
    }

    public T? GetData<T>() where T : class
    {
        var cacheKey = GetCacheKey<T>();
        return cache.Get<T>(cacheKey);
    }

    public object? GetData(Type type)
    {
        var cacheKey = GetCacheKey(type);
        return cache.Get(cacheKey);
    }
}