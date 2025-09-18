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

        // Find the latest journal group (most recent timestamp in filename)
        var latestGroup = GetLatestJournalGroup(journalFiles);
        if (latestGroup == null || latestGroup.Count == 0)
        {
            return [];
        }

        // Process only the latest journal group (handles split files automatically)
        var entries = await ProcessJournalGroupAsync(latestGroup);
        return entries.OfType<T>().OrderBy(e => e.Timestamp);
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

    private async Task EnsureCacheIsUpToDateAsync()
    {
        if (_isUpdating) return; // Prevent multiple simultaneous updates

        if (!Directory.Exists(_journalsPath))
        {
            throw new DirectoryNotFoundException(string.Format(Localization.Instance["Exceptions.DirectoryNotFound"],
                _journalsPath));
        }

        var journalFiles = GetJournalFiles();
        if (journalFiles.Length == 0) return;

        var needsUpdate = false;

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
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting journal files from {Path}", _journalsPath);
            return Array.Empty<FileInfo>();
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

    private string GetJournalBaseName(string fileName)
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

    private string GetGroupKey(FileInfo file) => GetJournalBaseName(file.Name);

    private List<FileInfo> GetLatestJournalGroup(FileInfo[] journalFiles)
    {
        if (journalFiles.Length == 0) return new List<FileInfo>();

        // Group all files by their base name
        var groupedFiles = GroupRelatedJournalFiles(journalFiles);
        if (groupedFiles.Count == 0) return new List<FileInfo>();

        // Find the group with the most recent base name (latest timestamp)
        // Journal files are named like: Journal.2024-01-15T123456.01.log
        var latestGroup = groupedFiles
            .OrderByDescending(group => ExtractTimestampFromFileName(group.First().Name))
            .First();

        logger.LogInformation("Found latest journal group with {FileCount} files: {Files}", 
            latestGroup.Count, 
            string.Join(", ", latestGroup.Select(f => f.Name)));

        return latestGroup;
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
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Could not extract timestamp from filename: {FileName}", fileName);
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
        var totalLines = lines.Count;
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
                if (entry != null)
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

    private JournalEvent ParseJournalEntry(JsonDocument jsonDocument, string rawJson)
    {
        var root = jsonDocument.RootElement;

        if (!root.TryGetProperty("timestamp", out var timestampElement) ||
            !root.TryGetProperty("event", out var eventElement))
        {
            return null;
        }

        if (!DateTime.TryParse(timestampElement.GetString(), out var timestamp))
        {
            return null;
        }

        try
        {
            return journalFactory.CreateEvent(rawJson);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to create journal event from: {Json}", rawJson[..Math.Min(100, rawJson.Length)]);
            return null;
        }
    }

    public void Dispose()
    {
        _cacheSemaphore?.Dispose();
    }
}