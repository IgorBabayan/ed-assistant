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

        // First try to get from combined cache
        var journalFiles = GetJournalFilesForToday(_journalsFolder);
        if (journalFiles.Any())
        {
            var combinedKey = GetCombinedJournalCacheKey(journalFiles);
            var cachedEvents = cache.Get<Dictionary<Type, List<JournalEvent>>>(combinedKey);

            if (cachedEvents != null && cachedEvents.TryGetValue(typeof(T), out var events))
            {
                return events.OfType<T>()
                    .OrderByDescending(e => e.Timestamp)
                    .FirstOrDefault();
            }
        }

        // Fallback to original logic for single journal file
        var journal = GetLastJournal(_journalsFolder);
        if (journal is null)
            return null;

        var singleJournalKey = GetLastJournalCacheKey(journal.Value.FileName);
        var singleCachedEvents = cache.Get<Dictionary<Type, List<JournalEvent>>>(singleJournalKey);

        if (singleCachedEvents != null && singleCachedEvents.TryGetValue(typeof(T), out var singleEvents))
        {
            return singleEvents.OfType<T>()
                .OrderByDescending(e => e.Timestamp)
                .FirstOrDefault();
        }

        return null;
    }

    public IList<T> GetLatestJournals<T>() where T : JournalEvent
    {
        if (string.IsNullOrWhiteSpace(_journalsFolder))
            return [];

        // First try to get from combined cache
        var journalFiles = GetJournalFilesForToday(_journalsFolder);
        if (journalFiles.Any())
        {
            var combinedKey = GetCombinedJournalCacheKey(journalFiles);
            var cachedEvents = cache.Get<Dictionary<Type, List<JournalEvent>>>(combinedKey);
        
            if (cachedEvents is not null && cachedEvents.TryGetValue(typeof(T), out var events))
            {
                return events.OfType<T>().ToList();
            }
        }

        // Fallback to original logic for single journal file
        var journal = GetLastJournal(_journalsFolder);
        if (journal is null)
            return [];

        var singleJournalKey = GetLastJournalCacheKey(journal.Value.FileName);
        var singleCachedEvents = cache.Get<Dictionary<Type, List<JournalEvent>>>(singleJournalKey);
    
        if (singleCachedEvents is not null && singleCachedEvents.TryGetValue(typeof(T), out var singleEvents))
        {
            return singleEvents.OfType<T>().ToList();
        }

        return [];
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
    
    // Get all journal files from today (or specify date range as needed)
    var journalFiles = GetJournalFilesForToday(journalsFolder);
    
    if (!journalFiles.Any())
        return;

    // Create a combined cache key based on all files
    var combinedCacheKey = GetCombinedJournalCacheKey(journalFiles);
    var lastWriteKey = $"{combinedCacheKey}_LastWrite";
    var lastPositionsKey = $"{combinedCacheKey}_LastPositions";

    // Get the latest modification time from all files
    var latestWriteTime = journalFiles.Max(f => f.Info.LastWriteTime);
    var cachedLastWrite = cache.Get<DateTime?>(lastWriteKey);
    var cachedPositions = cache.Get<Dictionary<string, long>>(lastPositionsKey) ?? new Dictionary<string, long>();

    // Get existing cached events to preserve them
    var existingEventsByType = cache.Get<Dictionary<Type, List<JournalEvent>>>(combinedCacheKey) ?? new Dictionary<Type, List<JournalEvent>>();
    var existingAllEvents = cache.Get<List<JournalEvent>>($"{combinedCacheKey}_AllEvents") ?? new List<JournalEvent>();
    
    var needsReload = cachedLastWrite != latestWriteTime;
    if (!needsReload)
        return;

    var allNewEvents = new List<JournalEvent>(existingAllEvents);
    var allNewEventsByType = new Dictionary<Type, List<JournalEvent>>();
    
    // Copy existing events
    foreach (var kvp in existingEventsByType)
    {
        allNewEventsByType[kvp.Key] = new List<JournalEvent>(kvp.Value);
    }
    
    var updatedPositions = new Dictionary<string, long>(cachedPositions);

    // Process each journal file in chronological order
    foreach (var journal in journalFiles.OrderBy(j => j.DateTime))
    {
        var startPosition = updatedPositions.GetValueOrDefault(journal.FileName, 0);
        
        await using var fileStream = new FileStream(journal.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fileStream);

        // Skip to the last processed position if we've read this file before
        if (startPosition > 0 && fileStream.Length > startPosition)
        {
            fileStream.Seek(startPosition, SeekOrigin.Begin);
            // Don't skip a line here - we might miss data
        }

        var currentPosition = fileStream.Position;
        var hasNewEvents = false;

        while (await reader.ReadLineAsync() is { } line)
        {
            currentPosition = fileStream.Position;
            
            if (!string.IsNullOrWhiteSpace(line))
            {
                var journalEvent = journalFactory.CreateEvent(line);
                if (journalEvent is not null)
                {
                    // Check if this event is newer than what we already have
                    if (startPosition > 0)
                    {
                        var existingEvents = allNewEventsByType.GetValueOrDefault(journalEvent.GetType(), new List<JournalEvent>());
                        if (existingEvents.Any(e => e.Timestamp == journalEvent.Timestamp))
                            continue; // Skip duplicate
                    }
                    
                    allNewEvents.Add(journalEvent);
                    hasNewEvents = true;

                    var eventType = journalEvent.GetType();
                    if (!allNewEventsByType.ContainsKey(eventType))
                    {
                        allNewEventsByType[eventType] = new List<JournalEvent>();
                    }
                    allNewEventsByType[eventType].Add(journalEvent);

                    logger.LogInformation(Localization.Instance["Settings.SuccessfullyLoadFileData"], eventType.Name, journal.FileName);
                    JournalLoaded?.Invoke(this, new JournalEventLoadedEventArgs(journalEvent));
                }
            }
        }

        // Only update position if we successfully read the file
        if (hasNewEvents || startPosition == 0)
        {
            updatedPositions[journal.FileName] = currentPosition;
        }
    }

    // Sort all events by timestamp to ensure proper chronological order
    foreach (var eventList in allNewEventsByType.Values)
    {
        eventList.Sort((e1, e2) => e1.Timestamp.CompareTo(e2.Timestamp));
    }
    
    allNewEvents.Sort((e1, e2) => e1.Timestamp.CompareTo(e2.Timestamp));

    // Cache the combined results
    cache.Set(combinedCacheKey, allNewEventsByType, _expirationTime);
    cache.Set($"{combinedCacheKey}_AllEvents", allNewEvents, _expirationTime);
    cache.Set(lastWriteKey, latestWriteTime, _expirationTime);
    cache.Set(lastPositionsKey, updatedPositions, _expirationTime);
}

    private static List<(string FileName, FileInfo Info, DateTime? DateTime)> GetJournalFilesForToday(string journalsFolder)
    {
        var today = DateTime.Today;
    
        return Directory.GetFiles(journalsFolder, "Journal.*.log")
            .Select(file => new
            {
                FileName = file,
                Info = new FileInfo(file),
                DateTime = ExtractDateTime(Path.GetFileName(file))
            })
            .Where(x => x.DateTime.HasValue && x.DateTime.Value.Date == today)
            .OrderBy(x => x.DateTime!.Value)
            .Select(x => (x.FileName, x.Info, x.DateTime))
            .ToList();
    }

    private static string GetCombinedJournalCacheKey(IEnumerable<(string FileName, FileInfo Info, DateTime? DateTime)> journalFiles)
    {
        var fileNames = journalFiles.Select(f => Path.GetFileName(f.FileName)).OrderBy(name => name);
        var combined = string.Join("_", fileNames);
        return $"CombinedJournal_{combined.GetHashCode():X}";
    }

    private async Task LoadAllJournalData(string journalsFolder)
    {
        throw new NotImplementedException();
    }
}