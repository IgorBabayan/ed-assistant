namespace EdAssistant.Services.Journal;

class JournalService(IFolderPickerService folderPickerService, ILogger<JournalService> logger,
    IJournalEventFactory journalFactory) : IJournalService
{
    private readonly string _journalsPath = folderPickerService.GetDefaultJournalsPath();
    private readonly ConcurrentDictionary<string, List<JournalEvent>> _cache = new();
    private readonly SemaphoreSlim _cacheSemaphore = new(1, 1);
    private DateTime _lastCacheUpdate = DateTime.MinValue;
    private volatile bool _isUpdating;

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
            throw new DirectoryNotFoundException(string.Format(Localization.Instance["Exceptions.DirectoryNotFound"],
                _journalsPath));
        }

        var journalFiles = GetJournalFiles();
        if (journalFiles.Length == 0) 
        {
            return [];
        }

        // Strategy 1: Try to get entries from all of today's journal files
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
                logger.LogInformation(Localization.Instance["JournalService.Information.FoundTodayEntity"], 
                    todayResults.Count, typeof(T).Name);
                return todayResults;
            }

            logger.LogInformation(Localization.Instance["JournalService.Information.NoTodayEntries"], typeof(T).Name);
        }

        // Strategy 2: Fallback to latest journal session if today's files have no matching entries
        var latestGroup = GetLatestJournalGroup(journalFiles);
        if (latestGroup.Count == 0)
        {
            return [];
        }

        var latestEntries = await ProcessJournalGroupAsync(latestGroup);
        var latestResults = latestEntries.OfType<T>().OrderBy(e => e.Timestamp).ToList();
        
        logger.LogInformation(Localization.Instance["JournalService.Information.FoundEntries"], 
            latestResults.Count, typeof(T).Name);
        
        return latestResults;
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
        // Extract base name from files like "Journal.2024-01-15T123456.01.log"
        // Returns "Journal.2024-01-15T123456" without the part number
        var parts = fileName.Split('.');
        if (parts.Length >= 4) // Journal, date, part, log
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
                // Check if any files are newer than last cache update
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
            logger.LogError(exception, Localization.Instance["JournalService.Exceptions.ErrorGettingJournalFiles"], _journalsPath);
            return [];
        }
    }

    private async Task LoadJournalFilesAsync(FileInfo[] journalFiles)
    {
        var groupedFiles = GroupRelatedJournalFiles(journalFiles);
        
        // Process groups with controlled concurrency to prevent overwhelming the system
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
                    _cache.AddOrUpdate(groupKey, entries, (key, oldValue) => entries);
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

        // Group all files by their base name
        var groupedFiles = GroupRelatedJournalFiles(journalFiles);
        if (groupedFiles.Count == 0) 
            return [];

        // Find the group with the most recent base name (latest timestamp)
        // Journal files are named like: Journal.2024-01-15T123456.01.log
        var latestGroup = groupedFiles
            .OrderByDescending(group => ExtractTimestampFromFileName(group.First().Name))
            .First();

        logger.LogInformation(Localization.Instance["JournalService.Information.FoundLatestJournal"], latestGroup.Count, 
            string.Join(", ", latestGroup.Select(f => f.Name)));

        return latestGroup;
    }

    private List<List<FileInfo>> GetTodaysJournalGroups(FileInfo[] journalFiles)
    {
        if (journalFiles.Length == 0) 
            return [];

        // Group all files by their base name
        var groupedFiles = GroupRelatedJournalFiles(journalFiles);
        if (groupedFiles.Count == 0) 
            return [];

        var today = DateTime.Today;
        var todayGroups = new List<List<FileInfo>>();

        // Filter groups to only include those from today
        foreach (var group in groupedFiles)
        {
            var groupTimestamp = ExtractTimestampFromFileName(group.First().Name);
            if (groupTimestamp.Date == today)
            {
                todayGroups.Add(group);
            }
        }

        // Sort by timestamp to process in chronological order
        todayGroups = todayGroups
            .OrderBy(group => ExtractTimestampFromFileName(group.First().Name))
            .ToList();

        logger.LogInformation(Localization.Instance["JournalService.Information.FoundJournalGroups"], 
            todayGroups.Count, 
            todayGroups.Sum(g => g.Count));

        return todayGroups;
    }

    private DateTime ExtractTimestampFromFileName(string fileName)
    {
        try
        {
            // Extract timestamp from "Journal.2024-01-15T123456.01.log" format
            var parts = fileName.Split('.');
            if (parts.Length >= 3)
            {
                var timestampPart = parts[1]; // "2024-01-15T123456"
                
                // Parse the timestamp - Elite Dangerous uses this format in filenames
                if (DateTime.TryParseExact(timestampPart, "yyyy-MM-ddTHHmmss", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestamp))
                {
                    return timestamp;
                }
            }
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, Localization.Instance["JournalService.Exceptions.CouldNotExtractTimestamp"], fileName);
        }

        // Fallback to file creation time if timestamp parsing fails
        return DateTime.MinValue;
    }

    private async Task<List<JournalEvent>> ProcessJournalGroupAsync(List<FileInfo> fileGroup)
    {
        var allLines = new List<string>();
        
        // Read files individually and collect lines - FIXED: Don't append extra newlines
        foreach (var file in fileGroup.OrderBy(f => f.Name))
        {
            try
            {
                await using var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                // Use StringReader for most robust cross-platform line splitting
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
                logger.LogWarning(exception, Localization.Instance["Warnings.CouldNotReadFile"],
                    file.Name, exception.Message);
            }
        }

        return await ParseJournalEntries(allLines);
    }

    private async Task<List<JournalEvent>> ParseJournalEntries(List<string> lines)
    {
        var entries = new List<JournalEvent>();
        
        // Process lines with progress tracking for large files
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
                logger.LogWarning(exception, Localization.Instance["Warnings.Filtering"],
                    trimmedLine[..Math.Min(50, trimmedLine.Length)], exception.Message);
            }

            // Yield control periodically for large files to prevent UI freezing
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
            return journalFactory.CreateEvent(rawJson);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, Localization.Instance["JournalService.Exceptions.FailedToCreateJournalEvent"], rawJson[..Math.Min(100, rawJson.Length)]);
            return null;
        }
    }
}