namespace EdAssistant.Services.Journal;

class JournalService : IJournalService
{
    private readonly ILogger<JournalService> _logger;
    private readonly IJournalEventFactory _journalFactory;
    private readonly string _journalsPath;
    private readonly ConcurrentDictionary<string, List<JournalEvent>> _cache = new();
    private readonly SemaphoreSlim _cacheSemaphore = new(1, 1);
    
    private DateTime _lastCacheUpdate = DateTime.MinValue;
    private volatile bool _isUpdating;

    public JournalService(IFolderPickerService folderPickerService, ILogger<JournalService> logger,
        IJournalEventFactory journalFactory)
    {
        _logger = logger;
        _journalFactory = journalFactory;
        if (folderPickerService.TryGetDefaultJournalsPath(out var path))
        {
            _journalsPath = path;
        }
    }

    public async Task<IEnumerable<T>> GetAllJournalEntriesAsync<T>() where T : JournalEvent
    {
        await EnsureCacheIsUpToDateAsync();
        return _cache.Values.SelectMany(entries => entries).OfType<T>().OrderBy(e => e.Timestamp);
    }

    public async Task<IEnumerable<T>> GetJournalEntriesAsync<T>(DateTime fromDate, DateTime? toDate = null) 
        where T : JournalEvent
    {
        var allEntries = await GetAllJournalEntriesAsync<T>();
        var filtered = allEntries.Where(e => e.Timestamp >= fromDate);
        if (toDate.HasValue)
        {
            filtered = filtered.Where(e => e.Timestamp <= toDate.Value);
        }

        return filtered;
    }

    public async Task<IEnumerable<T>> GetLatestJournalEntriesAsync<T>() where T : JournalEvent
    {
        if (!Directory.Exists(_journalsPath))
        {
            return [];
        }

        var journalFiles = GetJournalFiles();
        if (journalFiles.Length == 0) 
        {
            return [];
        }

        if (typeof(T) == typeof(ScanEvent) || 
            typeof(T) == typeof(SAAScanCompleteEvent) ||
            typeof(T) == typeof(ScanBaryCentreEvent) ||
            typeof(T) == typeof(FSSSignalDiscoveredEvent))
        {
            var recentResults = (await GetEntriesFromRecentDaysAsync<T>(journalFiles, maxDaysBack: 7)).ToList();
            if (recentResults.Any())
            {
                _logger.LogInformation(Localization.Instance["JournalService.Information.FoundEntriesFromRecentSessions"], 
                    recentResults.Count(), typeof(T).Name);
                return recentResults;
            }
        }

        var todayGroups = GetTodaysJournalGroups(journalFiles);
        if (todayGroups.Count > 0)
        {
            var allTodayEntries = new List<JournalEvent>();
            foreach (var group in todayGroups)
            {
                var groupEntries = await ProcessJournalGroupAsync(group);
                allTodayEntries.AddRange(groupEntries);
            }

            var todayResults = allTodayEntries.OfType<T>().OrderBy(e => e.Timestamp).ToList();
            if (todayResults.Count > 0)
            {
                _logger.LogInformation(Localization.Instance["JournalService.Information.FoundTodayEntity"], 
                    todayResults.Count, typeof(T).Name);
                return todayResults;
            }

            _logger.LogInformation(Localization.Instance["JournalService.Information.NoTodayEntries"], typeof(T).Name);
        }

        var fallbackResults = (await GetEntriesFromRecentDaysAsync<T>(journalFiles, maxDaysBack: 7)).ToList();
        if (fallbackResults.Any())
        {
            _logger.LogInformation(Localization.Instance["JournalService.Information.FoundEntriesFromRecentDays"], 
                fallbackResults.Count(), typeof(T).Name);
            return fallbackResults;
        }

        var latestGroup = GetLatestJournalGroup(journalFiles);
        if (latestGroup.Count == 0)
        {
            return [];
        }

        var latestEntries = await ProcessJournalGroupAsync(latestGroup);
        var latestResults = latestEntries.OfType<T>().OrderBy(e => e.Timestamp).ToList();
        
        _logger.LogInformation(Localization.Instance["JournalService.Information.FoundEntries"], 
            latestResults.Count, typeof(T).Name);
        
        return latestResults;
    }

    public async Task<Dictionary<Type, List<JournalEvent>>> GetLatestJournalEntriesBatchAsync(params Type[] eventTypes)
    {
        if (!Directory.Exists(_journalsPath))
        {
            return [];
        }

        var journalFiles = GetJournalFiles();
        if (journalFiles.Length == 0)
        {
            return eventTypes.ToDictionary(t => t, _ => new List<JournalEvent>());
        }

        var typeSet = new HashSet<Type>(eventTypes);
        var results = eventTypes.ToDictionary(t => t, _ => new List<JournalEvent>());

        var fallbackResults = await GetEntriesFromRecentDaysBatchAsync(journalFiles, typeSet, maxDaysBack: 7);
        foreach (var kvp in fallbackResults)
        {
            if (kvp.Value.Any())
            {
                results[kvp.Key] = kvp.Value;
                _logger.LogInformation(Localization.Instance["JournalService.Information.FoundEntriesFromRecentDays"],
                    kvp.Value.Count, kvp.Key.Name);
            }
        }

        return results;
    }

    private async Task<Dictionary<Type, List<JournalEvent>>> GetEntriesFromRecentDaysBatchAsync(
        FileInfo[] journalFiles, HashSet<Type> eventTypes, int maxDaysBack)
    {
        var groupedFiles = GroupRelatedJournalFiles(journalFiles);
        var today = DateTime.Today;
        var cutoffDate = today.AddDays(-maxDaysBack);
        var results = eventTypes.ToDictionary(t => t, _ => new List<JournalEvent>());

        var sortedGroups = groupedFiles
            .Where(group => ExtractTimestampFromFileName(group.First().Name).Date >= cutoffDate)
            .OrderByDescending(group => ExtractTimestampFromFileName(group.First().Name))
            .ToList();

        foreach (var group in sortedGroups)
        {
            var groupEntries = await ProcessJournalGroupAsync(group);
            var groupDate = ExtractTimestampFromFileName(group.First().Name).Date;

            foreach (var eventType in eventTypes)
            {
                var typedEntries = groupEntries.Where(e => e.GetType() == eventType).ToList();
                if (typedEntries.Any())
                {
                    results[eventType].AddRange(typedEntries);
                    _logger.LogInformation(Localization.Instance["JournalService.Information.FoundEntriesInJournalFrom"],
                        typedEntries.Count, eventType.Name, groupDate.ToShortDateString());
                }
            }
        }

        foreach (var kvp in results.ToList())
        {
            results[kvp.Key] = kvp.Value.OrderBy(e => e.Timestamp).ToList();
        }

        return results;
    }

    private async Task<IEnumerable<T>> GetEntriesFromRecentDaysAsync<T>(FileInfo[] journalFiles, int maxDaysBack)
        where T : JournalEvent
    {
        var groupedFiles = GroupRelatedJournalFiles(journalFiles);
        var today = DateTime.Today;
        var cutoffDate = today.AddDays(-maxDaysBack);
        var recentEntries = new List<T>();
        
        var sortedGroups = groupedFiles
            .Where(group => ExtractTimestampFromFileName(group.First().Name).Date >= cutoffDate)
            .OrderByDescending(group => ExtractTimestampFromFileName(group.First().Name))
            .ToList();

        foreach (var group in sortedGroups)
        {
            var groupEntries = await ProcessJournalGroupAsync(group);
            var typedEntries = groupEntries.OfType<T>().ToList();
            
            if (typedEntries.Any())
            {
                recentEntries.AddRange(typedEntries);
                var groupDate = ExtractTimestampFromFileName(group.First().Name).Date;
                _logger.LogInformation(Localization.Instance["JournalService.Information.FoundEntriesInJournalFrom"], 
                    typedEntries.Count, typeof(T).Name, groupDate.ToShortDateString());
            }
        }
        
        return recentEntries.OrderBy(e => e.Timestamp);
    }

    public async Task RefreshCacheAsync()
    {
        await _cacheSemaphore.WaitAsync();
        try
        {
            _cache.Clear();
            _lastCacheUpdate = DateTime.MinValue;
            _isUpdating = false;
        }
        finally
        {
            _cacheSemaphore.Release();
        }

        await EnsureCacheIsUpToDateAsync();
    }

    public void ClearCache()
    {
        _cacheSemaphore.Wait();
        try
        {
            _cache.Clear();
            _lastCacheUpdate = DateTime.MinValue;
            _isUpdating = false;
        }
        finally
        {
            _cacheSemaphore.Release();
        }
    }

    public void Dispose() => _cacheSemaphore?.Dispose();

    private static string GetJournalBaseName(string fileName)
    {
        var parts = fileName.Split('.');
        if (parts.Length >= 4) 
        {
            return string.Join(".", parts.Take(parts.Length - 2));
        }

        return Path.GetFileNameWithoutExtension(fileName);
    }

    private static string GetGroupKey(FileInfo file) => GetJournalBaseName(file.Name);

    private async Task EnsureCacheIsUpToDateAsync()
    {
        if (_isUpdating)
            return;

        if (!Directory.Exists(_journalsPath))
        {
            throw new DirectoryNotFoundException(string.Format(Localization.Instance["Exceptions.DirectoryNotFound"],
                _journalsPath));
        }

        var journalFiles = GetJournalFiles();
        if (journalFiles.Length == 0) return;

        bool needsUpdate;
        await _cacheSemaphore.WaitAsync();
        try
        {
            if (_cache.IsEmpty)
            {
                needsUpdate = true;
            }
            else
            {
                var newestFileTime = journalFiles.Max(f => File.GetLastWriteTime(f.FullName));
                needsUpdate = newestFileTime > _lastCacheUpdate;
            }

            if (needsUpdate)
            {
                _isUpdating = true;
            }
        }
        finally
        {
            _cacheSemaphore.Release();
        }

        if (needsUpdate)
        {
            try
            {
                await LoadJournalFilesAsync(journalFiles);
                await _cacheSemaphore.WaitAsync();
                
                try
                {
                    _lastCacheUpdate = DateTime.Now;
                    _isUpdating = false;
                }
                finally
                {
                    _cacheSemaphore.Release();
                }
            }
            catch
            {
                _isUpdating = false;
                throw;
            }
        }
    }

    private FileInfo[] GetJournalFiles()
    {
        try
        {
            var directory = new DirectoryInfo(_journalsPath);
            return directory.GetFiles("Journal.*.log")
                .OrderBy(f => f.Name)
                .ToArray();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, Localization.Instance["JournalService.Exceptions.ErrorGettingJournalFiles"], _journalsPath);
            return [];
        }
    }

    private async Task LoadJournalFilesAsync(FileInfo[] journalFiles)
    {
        var groupedFiles = GroupRelatedJournalFiles(journalFiles);
        var semaphore = new SemaphoreSlim(Environment.ProcessorCount, Environment.ProcessorCount);
        var tasks = groupedFiles.Select(async group =>
        {
            await semaphore.WaitAsync();
            try
            {
                var entries = await ProcessJournalGroupAsync(group);
                var groupKey = GetGroupKey(group.First());

                await _cacheSemaphore.WaitAsync();
                try
                {
                    _cache.AddOrUpdate(groupKey, entries, (_, _) => entries);
                }
                finally
                {
                    _cacheSemaphore.Release();
                }
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
    }

    private List<List<FileInfo>> GroupRelatedJournalFiles(FileInfo[] files)
    {
        var groups = new List<List<FileInfo>>();
        var currentGroup = new List<FileInfo>();

        foreach (var file in files)
        {
            var baseName = GetJournalBaseName(file.Name);
            if (currentGroup.Count == 0 || GetJournalBaseName(currentGroup.First().Name) == baseName)
            {
                currentGroup.Add(file);
            }
            else
            {
                groups.Add(currentGroup);
                currentGroup = [file];
            }
        }

        if (currentGroup.Count > 0)
        {
            groups.Add(currentGroup);
        }

        return groups;
    }

    private List<FileInfo> GetLatestJournalGroup(FileInfo[] journalFiles)
    {
        if (journalFiles.Length == 0) 
            return [];

        var groupedFiles = GroupRelatedJournalFiles(journalFiles);
        if (groupedFiles.Count == 0) 
            return [];

        var latestGroup = groupedFiles
            .OrderByDescending(group => ExtractTimestampFromFileName(group.First().Name))
            .First();

        _logger.LogInformation(Localization.Instance["JournalService.Information.FoundLatestJournal"], latestGroup.Count, 
            string.Join(", ", latestGroup.Select(f => f.Name)));

        return latestGroup;
    }

    private List<List<FileInfo>> GetTodaysJournalGroups(FileInfo[] journalFiles)
    {
        if (journalFiles.Length == 0) 
            return [];

        var groupedFiles = GroupRelatedJournalFiles(journalFiles);
        if (groupedFiles.Count == 0) 
            return [];

        var today = DateTime.Today;
        var todayGroups = new List<List<FileInfo>>();

        foreach (var group in groupedFiles)
        {
            var groupTimestamp = ExtractTimestampFromFileName(group.First().Name);
            if (groupTimestamp.Date == today)
            {
                todayGroups.Add(group);
            }
        }

        todayGroups = todayGroups
            .OrderBy(group => ExtractTimestampFromFileName(group.First().Name))
            .ToList();

        _logger.LogInformation(Localization.Instance["JournalService.Information.FoundJournalGroups"], 
            todayGroups.Count, 
            todayGroups.Sum(g => g.Count));

        return todayGroups;
    }

    private DateTime ExtractTimestampFromFileName(string fileName)
    {
        try
        {
            var parts = fileName.Split('.');
            if (parts.Length >= 3)
            {
                var timestampPart = parts[1];
                
                if (DateTime.TryParseExact(timestampPart, "yyyy-MM-ddTHHmmss", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestamp))
                {
                    return timestamp;
                }
            }
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, Localization.Instance["JournalService.Exceptions.CouldNotExtractTimestamp"], fileName);
        }

        return DateTime.MinValue;
    }

    private async Task<List<JournalEvent>> ProcessJournalGroupAsync(List<FileInfo> fileGroup)
    {
        var allLines = new List<string>();
        foreach (var file in fileGroup.OrderBy(f => f.Name))
        {
            try
            {
                await using var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(fileStream);
                while (await reader.ReadLineAsync() is { } line)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        allLines.Add(line);
                    }
                }
            }
            catch (IOException exception)
            {
                _logger.LogWarning(exception, Localization.Instance["Warnings.CouldNotReadFile"],
                    file.Name, exception.Message);
            }
        }

        return await ParseJournalEntries(allLines);
    }

    private async Task<List<JournalEvent>> ParseJournalEntries(List<string> lines)
    {
        var entries = new List<JournalEvent>();
        var processedLines = 0;

        foreach (var line in lines)
        {
            processedLines++;
            var trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine))
                continue;

            try
            {
                using var jsonDocument = JsonDocument.Parse(trimmedLine);
                var entry = ParseJournalEntry(jsonDocument, trimmedLine);
                if (entry is not null)
                {
                    entries.Add(entry);
                }
            }
            catch (JsonException exception)
            {
                _logger.LogWarning(exception, Localization.Instance["Warnings.Filtering"],
                    trimmedLine[..Math.Min(50, trimmedLine.Length)], exception.Message);
            }

            if (processedLines % 1000 == 0)
            {
                await Task.Yield();
            }
        }

        return entries.OrderBy(e => e.Timestamp).ToList();
    }

    private JournalEvent? ParseJournalEntry(JsonDocument jsonDocument, string rawJson)
    {
        var root = jsonDocument.RootElement;

        if (!root.TryGetProperty("timestamp", out var timestampElement) ||
            !root.TryGetProperty("event", out _))
        {
            return null;
        }

        if (!DateTime.TryParse(timestampElement.GetString(), out _))
        {
            return null;
        }

        try
        {
            return _journalFactory.CreateEvent(rawJson);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, Localization.Instance["JournalService.Exceptions.FailedToCreateJournalEvent"], rawJson[..Math.Min(100, rawJson.Length)]);
            return null;
        }
    }
}