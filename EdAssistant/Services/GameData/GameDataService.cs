using CargoEvent = EdAssistant.Models.Cargo.CargoEvent;
using ShipLockerEvent = EdAssistant.Models.ShipLocker.ShipLockerEvent;

namespace EdAssistant.Services.GameData;

class GameDataService(ILogger<GameDataService> logger, IMemoryCache cache, IJournalEventFactory journalFactory) : IGameDataService
{
    private const int MAX_CACHED_EVENTS_PER_TYPE = 1000;
    private const int LATEST_EVENTS_TO_KEEP = 100;

    private readonly ConcurrentDictionary<Type, JournalEvent> _latestEvents = new();
    private readonly ConcurrentDictionary<Type, List<JournalEvent>> _recentEvents = new();
    private readonly ReaderWriterLockSlim _eventsLock = new();

    private readonly Dictionary<Type, string> _fileMapping = new()
    {
        { typeof(ShipLockerEvent), "ShipLocker.json" },
        { typeof(CargoEvent), "Cargo.json" },
        { typeof(MarketData), "Market.json" },
    };

    private static string GetCacheKey<T>() => $"GameData_{typeof(T).Name}";
    private static string GetCacheKey(Type type) => $"GameData_{type.Name}";
    private static string GetJournalCacheKey(string fileName) => $"Journal_{Path.GetFileName(fileName)}";
    private static string GetJournalEventCacheKey<T>() where T : JournalEvent => $"JournalEvents_{typeof(T).Name}";
    private static string GetJournalEventCacheKey(Type type) => $"JournalEvents_{type.Name}";

    public event EventHandler<GameDataLoadedEventArgs>? DataLoaded;
    public event EventHandler<JournalEventLoadedEventArgs>? JournalEventLoaded;

    public void Dispose() => _eventsLock.Dispose();

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

    public async Task LoadAllGameDataAsync(string journalsFolder)
    {
        var tasks = new[]
        {
            LoadAllJournalsAsync(journalsFolder),
            LoadAllShipDataAsync(journalsFolder)
        };
        await Task.WhenAll(tasks);
    }

    public List<T> GetJournalEvents<T>() where T : JournalEvent
    {
        var eventType = typeof(T);
        if (_recentEvents.TryGetValue(eventType, out var recentEvents))
        {
            return recentEvents.OfType<T>().ToList();
        }

        var cacheKey = GetJournalEventCacheKey<T>();
        var cachedEvents = cache.Get<List<JournalEvent>>(cacheKey);

        if (cachedEvents == null)
            return [];

        return cachedEvents.OfType<T>().ToList();
    }

    public List<JournalEvent> GetJournalEvents(Type eventType)
    {
        if (_recentEvents.TryGetValue(eventType, out var recentEvents))
        {
            return recentEvents;
        }

        var cacheKey = GetJournalEventCacheKey(eventType);
        return cache.Get<List<JournalEvent>>(cacheKey) ?? [];
    }

    public T? GetLatestJournalEvent<T>() where T : JournalEvent
    {
        var eventType = typeof(T);
        if (_latestEvents.TryGetValue(eventType, out var latestEvent) && latestEvent is T typedEvent)
        {
            return typedEvent;
        }

        if (_recentEvents.TryGetValue(eventType, out var recentEvents) && recentEvents.Count > 0)
        {
            return recentEvents[0] as T;
        }

        var events = GetJournalEvents<T>();
        return events.OrderByDescending(e => e.Timestamp).FirstOrDefault();
    }

    public void ClearOldEvents()
    {
        var cutoffTime = DateTime.UtcNow.AddHours(-24);
        _eventsLock.EnterWriteLock();
        try
        {
            var keysToUpdate = new List<string>();
            foreach (var eventType in _fileMapping.Keys.Concat([typeof(MaterialsEvent)]))
            {
                var cacheKey = GetJournalEventCacheKey(eventType);
                var events = cache.Get<List<JournalEvent>>(cacheKey);

                if (events != null)
                {
                    var filteredEvents = events
                        .Where(e => e.Timestamp > cutoffTime)
                        .ToList();

                    if (filteredEvents.Count != events.Count)
                    {
                        cache.Set(cacheKey, filteredEvents, TimeSpan.FromMinutes(30));
                        keysToUpdate.Add(cacheKey);
                    }
                }
            }

            logger.LogInformation(string.Format(Localization.Instance["Common.CleanUpCache"], keysToUpdate.Count));
        }
        finally
        {
            _eventsLock.ExitWriteLock();
        }
    }

    private async Task LoadAllShipDataAsync(string journalsFolder)
    {
        var files = Directory.GetFiles(journalsFolder, "*.json");
        foreach (var (type, fileName) in _fileMapping)
        {
            var file = files.FirstOrDefault(x => Path.GetFileName(x).Equals(fileName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(file))
            {
                var fileInfo = new FileInfo(file);
                var cacheKey = GetCacheKey(type);
                var lastWriteKey = $"{cacheKey}_LastWrite";

                var cachedLastWrite = cache.Get<DateTime?>(lastWriteKey);
                var needsReload = !cache.TryGetValue(cacheKey, out _) || cachedLastWrite != fileInfo.LastWriteTime;

                if (needsReload)
                {
                    var data = await LoadDataAsync(type, file);
                    if (data is not null)
                    {
                        cache.Set(cacheKey, data, TimeSpan.FromHours(1));
                        cache.Set(lastWriteKey, fileInfo.LastWriteTime, TimeSpan.FromHours(1));

                        logger.LogInformation(Localization.Instance["Settings.SuccessfullyLoadFileData"], type.Name, file);
                        DataLoaded?.Invoke(this, new GameDataLoadedEventArgs(type, data));
                    }
                }
            }
        }
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
                logger.LogInformation(Localization.Instance["Settings.SuccessfullyLoadFileData"], type.Name, filePath);
                DataLoaded?.Invoke(this, new GameDataLoadedEventArgs(type, data));
            }

            return data;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["Exceptions.FailedToLoadFileData"], filePath);
            return null;
        }
    }

    private async Task LoadAllJournalsAsync(string journalsFolder)
    {
        var journalFiles = Directory.GetFiles(journalsFolder, "Journal.*.log")
            .OrderBy(f => f)
            .ToList();

        foreach (var file in journalFiles)
        {
            await LoadJournalFileAsync(file);
        }
    }

    private async Task LoadJournalFileAsync(string filePath)
    {
        var fileInfo = new FileInfo(filePath);
        var cacheKey = GetJournalCacheKey(filePath);
        var lastWriteKey = $"{cacheKey}_LastWrite";
        var lastPositionKey = $"{cacheKey}_LastPosition";

        var cachedLastWrite = cache.Get<DateTime?>(lastWriteKey);
        var cachedLastPosition = cache.Get<long>(lastPositionKey);

        var needsReload = cachedLastWrite != fileInfo.LastWriteTime;
        if (!needsReload)
        {
            return;
        }

        try
        {
            var newEventsByType = new Dictionary<Type, List<JournalEvent>>();
            long startPosition = 0;

            if (cachedLastWrite.HasValue && fileInfo.LastWriteTime > cachedLastWrite.Value)
            {
                startPosition = cachedLastPosition;
            }

            await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream);

            if (startPosition > 0 && fileStream.Length > startPosition)
            {
                fileStream.Seek(startPosition, SeekOrigin.Begin);
                await reader.ReadLineAsync();
            }

            var currentPosition = fileStream.Position;
            while (await reader.ReadLineAsync() is { } line)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var journalEvent = journalFactory.CreateEvent(line);
                    if (journalEvent != null)
                    {
                        var eventType = journalEvent.GetType();
                        if (!newEventsByType.ContainsKey(eventType))
                        {
                            newEventsByType[eventType] = [];
                        }
                        newEventsByType[eventType].Add(journalEvent);

                        UpdateLatestEvent(eventType, journalEvent);

                        logger.LogInformation(Localization.Instance["Settings.SuccessfullyLoadFileData"], eventType.Name, filePath);
                        JournalEventLoaded?.Invoke(this, new JournalEventLoadedEventArgs(journalEvent));
                    }
                }
                currentPosition = fileStream.Position;
            }

            foreach (var (eventType, newEvents) in newEventsByType)
            {
                UpdateEventCache(eventType, newEvents);
            }

            cache.Set(lastWriteKey, fileInfo.LastWriteTime, TimeSpan.FromMinutes(30));
            cache.Set(lastPositionKey, currentPosition, TimeSpan.FromMinutes(30));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, string.Format(Localization.Instance["Exceptions.FailedToLoadFileData"], filePath));
        }
    }

    private void UpdateLatestEvent(Type eventType, JournalEvent journalEvent) =>
        _latestEvents.AddOrUpdate(eventType, journalEvent, (key, existing) =>
            journalEvent.Timestamp > existing.Timestamp ? journalEvent : existing);

    private void UpdateEventCache(Type eventType, List<JournalEvent> newEvents)
    {
        var eventCacheKey = GetJournalEventCacheKey(eventType);
        _eventsLock.EnterWriteLock();
        try
        {
            var existingEvents = cache.Get<List<JournalEvent>>(eventCacheKey) ?? new List<JournalEvent>();
            existingEvents.AddRange(newEvents);

            if (existingEvents.Count > MAX_CACHED_EVENTS_PER_TYPE)
            {
                existingEvents = existingEvents
                    .OrderByDescending(e => e.Timestamp)
                    .Take(MAX_CACHED_EVENTS_PER_TYPE)
                    .ToList();
            }

            cache.Set(eventCacheKey, existingEvents, TimeSpan.FromMinutes(30));
            _recentEvents[eventType] = existingEvents
                .OrderByDescending(e => e.Timestamp)
                .Take(LATEST_EVENTS_TO_KEEP)
                .ToList();
        }
        finally
        {
            _eventsLock.ExitWriteLock();
        }
    }
}