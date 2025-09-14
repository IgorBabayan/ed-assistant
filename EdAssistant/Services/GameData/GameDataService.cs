using CargoEvent = EdAssistant.Models.Cargo.CargoEvent;

namespace EdAssistant.Services.GameData;

public class GameDataService(ILogger<GameDataService> logger, IMemoryCache cache, IJournalEventFactory journalFactory) : IGameDataService
{
    private readonly Dictionary<Type, string> _fileMapping = new()
    {
        { typeof(ShipLockerEvent), "ShipLocker.json" },
        { typeof(CargoEvent), "Cargo.json" },
        { typeof(MarketData), "Market.json" },
    };
    private readonly TimeSpan _expirationTime = TimeSpan.FromMinutes(30);

    private string? _journalsFolder;

    public event EventHandler<GameDataLoadedEventArgs>? DataLoaded;
    public event EventHandler<JournalEventLoadedEventArgs>? JournalLoaded;

    public async Task LoadLast(string journalsFolder)
    {
        if (string.IsNullOrWhiteSpace(journalsFolder) || !Directory.Exists(journalsFolder))
            return;

        Task[] tasks =
        [
            LoadShipData(journalsFolder),
            LoadLastJournalData(journalsFolder)
        ];
        await Task.WhenAll(tasks);
    }

    public async Task LoadAll(string journalsFolder)
    {
        if (string.IsNullOrWhiteSpace(journalsFolder) || !Directory.Exists(journalsFolder))
            return;

        Task[] tasks =
        [
            LoadShipData(journalsFolder),
            LoadAllJournalData(journalsFolder)
        ];
        await Task.WhenAll(tasks);
    }

    public T? GetData<T>() where T : class
    {
        var cacheKey = GetDataCacheKey<T>();
        return cache.Get<T>(cacheKey);
    }

    public T? GetJournal<T>() where T : JournalEvent
    {
        return null;
    }

    public T? GetLatestJournal<T>() where T : JournalEvent
    {
        if (string.IsNullOrWhiteSpace(_journalsFolder))
            return null;

        var journal = GetLastJournal(_journalsFolder);
        if (journal is null)
            return null;

        var lastJournalKey = GetLastJournalCacheKey(journal.Value.FileName);
        var cachedEvents = cache.Get<Dictionary<Type, List<JournalEvent>>>(lastJournalKey);

        if (cachedEvents != null && cachedEvents.TryGetValue(typeof(T), out var events))
        {
            return events.OfType<T>()
                .OrderByDescending(e => e.Timestamp)
                .FirstOrDefault();
        }

        return null;
    }

    private static string GetDataCacheKey<T>() => $"GameData_{typeof(T).Name}";
    private static string GetDataCacheKey(Type type) => $"GameData_{type.Name}";
    private static string GetJournalCacheKey(string fileName) => $"Journal_{Path.GetFileName(fileName)}";
    private static string GetLastJournalCacheKey(string fileName) => $"Last_{GetJournalCacheKey(fileName)}";

    private static (string FileName, FileInfo Info, DateTime? DateTime)? GetLastJournal(string journalsFolder)
    {
        var journal = Directory.GetFiles(journalsFolder, "Journal.*.log")
            .Select(file => new
            {
                FileName = file,
                Info = new FileInfo(file),
                DateTime = ExtractDateTime(Path.GetFileName(file))
            })
            .Where(x => x.DateTime.HasValue)
            .OrderByDescending(x => x.DateTime!.Value)
            .FirstOrDefault();

        if (journal is null)
            return null;
        return (journal.FileName, journal.Info, journal.DateTime);
    }

    private static DateTime? ExtractDateTime(string filename)
    {
        var match = Regex.Match(filename, @"(\d{4}-\d{2}-\d{2}T\d{6})");
        if (match.Success)
        {
            var dateTimeStr = match.Groups[1].Value;
            if (DateTime.TryParseExact(dateTimeStr, "yyyy-MM-ddTHHmmss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
        }
        return null;
    }

    private async Task<object?> LoadDataAsync(Type type, string filePath)
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

    private async Task LoadShipData(string journalsFolder)
    {
        var files = Directory.GetFiles(journalsFolder, "*.json");
        foreach (var (type, fileName) in _fileMapping)
        {
            var file = files.FirstOrDefault(x =>
                Path.GetFileName(x).Equals(fileName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(file))
            {
                var fileInfo = new FileInfo(file);
                var cacheKey = GetDataCacheKey(type);
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

    private async Task LoadLastJournalData(string journalsFolder)
    {
        _journalsFolder = journalsFolder;
        var journal = GetLastJournal(_journalsFolder);

        if (journal is null)
            return;

        var cacheKey = GetJournalCacheKey(journal.Value.FileName);
        var lastWriteKey = $"{cacheKey}_LastWrite";
        var lastPositionKey = $"{cacheKey}_LastPosition";

        var cachedLastWrite = cache.Get<DateTime?>(lastWriteKey);
        var cachedLastPosition = cache.Get<long>(lastPositionKey);

        var needsReload = cachedLastWrite != journal.Value.Info.LastWriteTime;
        if (!needsReload)
            return;

        await using var fileStream = new FileStream(journal.Value.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fileStream);

        long startPosition = 0;
        if (cachedLastWrite.HasValue && journal.Value.Info.LastWriteTime > cachedLastWrite.Value)
        {
            startPosition = cachedLastPosition;
        }

        if (startPosition > 0 && fileStream.Length > startPosition)
        {
            fileStream.Seek(startPosition, SeekOrigin.Begin);
            await reader.ReadLineAsync();
        }

        var newEvents = new List<JournalEvent>();
        var newEventsByType = new Dictionary<Type, List<JournalEvent>>();
        var currentPosition = fileStream.Position;

        while (await reader.ReadLineAsync() is { } line)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                var journalEvent = journalFactory.CreateEvent(line);
                if (journalEvent is not null)
                {
                    newEvents.Add(journalEvent);

                    var eventType = journalEvent.GetType();
                    if (!newEventsByType.ContainsKey(eventType))
                    {
                        newEventsByType[eventType] = [];
                    }
                    newEventsByType[eventType].Add(journalEvent);

                    logger.LogInformation(Localization.Instance["Settings.SuccessfullyLoadFileData"], eventType.Name, journal.Value.FileName);
                    JournalLoaded?.Invoke(this, new JournalEventLoadedEventArgs(journalEvent));
                }
            }
            currentPosition = fileStream.Position;
        }

        var lastJournalKey = GetLastJournalCacheKey(journal.Value.FileName);
        var allEventsKey = $"{lastJournalKey}_AllEvents";

        cache.Set(lastJournalKey, newEventsByType, _expirationTime);
        cache.Set(allEventsKey, newEvents, _expirationTime);
        cache.Set(lastWriteKey, journal.Value.Info.LastWriteTime, _expirationTime);
        cache.Set(lastPositionKey, currentPosition, _expirationTime);
    }

    private async Task LoadAllJournalData(string journalsFolder)
    {
        throw new NotImplementedException();
    }
}