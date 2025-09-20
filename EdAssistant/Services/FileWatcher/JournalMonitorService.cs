namespace EdAssistant.Services.FileWatcher;

sealed class JournalMonitorService(IFolderPickerService folderPickerService, IJournalEventFactory journalEventFactory,
    ISettingsService settingsService, ILogger<JournalMonitorService> logger) : IJournalMonitorService
{
    private FileSystemWatcher? _fileWatcher;
    private readonly Dictionary<string, long> _filePositions = new();
    private readonly Lock _lock = new();
    private string? _journalPath;
    private bool _disposed;

    public event EventHandler<JournalEventArgs>? JournalEventReceived;
    public event EventHandler<JournalFileEventArgs>? NewJournalFileCreated;
    public event EventHandler<JournalPathChangedEventArgs>? JournalPathChanged;
    
    public bool IsMonitoring { get; private set; }

    public async Task StartAsync()
    {
        if (IsMonitoring)
        {
            logger.LogWarning("Journal monitoring is already running");
            return;
        }

        var journalPath = GetCurrentJournalPath();
        if (string.IsNullOrWhiteSpace(journalPath) || !Directory.Exists(journalPath))
        {
            logger.LogInformation("Journal folder not set");
            return;
        }
        await StartMonitoringInternalAsync(journalPath);
    }

    public async Task RestartAsync(string newJournalPath)
    {
        if (string.IsNullOrWhiteSpace(newJournalPath))
        {
            throw new ArgumentException("Journal path cannot be null or empty", nameof(newJournalPath));
        }

        if (!Directory.Exists(newJournalPath))
        {
            throw new DirectoryNotFoundException($"Journal directory not found: {newJournalPath}");
        }

        var oldPath = _journalPath;
        
        logger.LogInformation("Restarting journal monitoring. Old path: '{OldPath}', New path: '{NewPath}'", 
            oldPath ?? "none", newJournalPath);

        try
        {
            // Stop current monitoring
            StopMonitoringInternal();

            // Clear previous state
            lock (_lock)
            {
                _filePositions.Clear();
            }

            // Save new path to settings
            settingsService.SetSetting("JournalFolderPath", newJournalPath);

            // Start monitoring with new path
            await StartMonitoringInternalAsync(newJournalPath);
            
            logger.LogInformation("Successfully restarted journal monitoring with new path: {NewPath}", newJournalPath);
            
            // Notify path change
            JournalPathChanged?.Invoke(this, new JournalPathChangedEventArgs(oldPath ?? "", newJournalPath));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to restart monitoring with new path: {NewPath}", newJournalPath);
            
            // Try to restore previous monitoring if possible
            if (!string.IsNullOrEmpty(oldPath) && Directory.Exists(oldPath))
            {
                logger.LogInformation("Attempting to restore previous journal path: {OldPath}", oldPath);
                try
                {
                    await StartMonitoringInternalAsync(oldPath);
                }
                catch (Exception restoreEx)
                {
                    logger.LogError(restoreEx, "Failed to restore previous journal monitoring");
                }
            }
            
            throw;
        }
    }

    public void StopMonitoring()
    {
        if (!IsMonitoring)
            return;

        StopMonitoringInternal();
        logger.LogInformation("Journal monitoring stopped");
    }

    private string GetCurrentJournalPath()
    {
        // First try to get from settings
        var savedPath = settingsService.GetSetting<string>("JournalFolderPath");
        if (!string.IsNullOrEmpty(savedPath) && Directory.Exists(savedPath))
        {
            return savedPath;
        }

        if (folderPickerService.TryGetDefaultJournalsPath(out var path))
        {
            return path;
        }
        // Fall back to default path
        return string.Empty;
    }

    private async Task StartMonitoringInternalAsync(string journalPath)
    {
        if (string.IsNullOrEmpty(journalPath) || !Directory.Exists(journalPath))
        {
            throw new DirectoryNotFoundException($"Journal directory not found: {journalPath}");
        }

        _journalPath = journalPath;

        try
        {
            // Initialize file positions for existing files (start at end to only catch new events)
            await InitializeFilePositionsAsync();

            // Set up file system watcher
            _fileWatcher = new FileSystemWatcher(_journalPath)
            {
                Filter = "Journal.*.log",
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                EnableRaisingEvents = true
            };

            _fileWatcher.Changed += OnFileChanged;
            _fileWatcher.Created += OnFileCreated;
            _fileWatcher.Error += OnWatcherError;

            IsMonitoring = true;
            logger.LogInformation("Journal monitoring started for path: {Path}", _journalPath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to start journal monitoring for path: {Path}", journalPath);
            throw;
        }
    }

    private void StopMonitoringInternal()
    {
        IsMonitoring = false;
        
        if (_fileWatcher != null)
        {
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.Changed -= OnFileChanged;
            _fileWatcher.Created -= OnFileCreated;
            _fileWatcher.Error -= OnWatcherError;
            _fileWatcher.Dispose();
            _fileWatcher = null;
        }
    }

    private async Task InitializeFilePositionsAsync()
    {
        if (string.IsNullOrEmpty(_journalPath))
            return;

        var journalFiles = Directory.GetFiles(_journalPath, "Journal.*.log")
            .OrderBy(f => f)
            .ToArray();

        foreach (var file in journalFiles)
        {
            try
            {
                var fileInfo = new FileInfo(file);
                lock (_lock)
                {
                    // Start at end of existing files to only catch new events
                    _filePositions[file] = fileInfo.Length;
                }
                
                logger.LogDebug("Initialized position for {FileName}: {Position} bytes", 
                    Path.GetFileName(file), fileInfo.Length);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to initialize position for file {FileName}", 
                    Path.GetFileName(file));
            }
        }
    }

    private async void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        try
        {
            logger.LogInformation("New journal file created: {FileName}", Path.GetFileName(e.FullPath));
            
            // Initialize position for new file
            lock (_lock)
            {
                _filePositions[e.FullPath] = 0;
            }
            
            NewJournalFileCreated?.Invoke(this, new JournalFileEventArgs(e.FullPath));
            
            // Small delay to let the game write initial content
            await Task.Delay(100);
            await ProcessFileChangesAsync(e.FullPath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling file creation for {FileName}", Path.GetFileName(e.FullPath));
        }
    }

    private async void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        try
        {
            // Small delay to handle rapid file changes
            await Task.Delay(50);
            await ProcessFileChangesAsync(e.FullPath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling file change for {FileName}", Path.GetFileName(e.FullPath));
        }
    }

    private void OnWatcherError(object sender, ErrorEventArgs e)
    {
        logger.LogError(e.GetException(), "File system watcher error occurred");
        
        // Try to restart monitoring
        Task.Run(async () =>
        {
            try
            {
                await Task.Delay(1000); // Wait a bit before restarting
                if (!IsMonitoring && !string.IsNullOrEmpty(_journalPath))
                {
                    logger.LogInformation("Attempting to restart journal monitoring after error");
                    await StartMonitoringInternalAsync(_journalPath);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to restart monitoring after watcher error");
            }
        });
    }

    private async Task ProcessFileChangesAsync(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        try
        {
            var fileName = Path.GetFileName(filePath);
            long lastPosition;
            
            lock (_lock)
            {
                lastPosition = _filePositions.GetValueOrDefault(filePath, 0);
            }

            // Check if file has new content
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length <= lastPosition)
                return;

            logger.LogDebug("Processing changes in {FileName} from position {Position}", 
                fileName, lastPosition);

            var newEvents = await ReadNewEntriesAsync(filePath, lastPosition);
            
            foreach (var (rawJson, journalEvent) in newEvents)
            {
                try
                {
                    JournalEventReceived?.Invoke(this, new JournalEventArgs(journalEvent, rawJson, fileName));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error invoking journal event handler for event type: {EventType}", 
                        journalEvent?.GetType().Name ?? "Unknown");
                }
            }

            // Update file position
            lock (_lock)
            {
                _filePositions[filePath] = fileInfo.Length;
            }
            
            if (newEvents.Count > 0)
            {
                logger.LogDebug("Processed {Count} new journal events from {FileName}", 
                    newEvents.Count, fileName);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing file changes for {FileName}", Path.GetFileName(filePath));
        }
    }

    private async Task<List<(string RawJson, JournalEvent? Event)>> ReadNewEntriesAsync(string filePath, long startPosition)
    {
        var newEvents = new List<(string, JournalEvent?)>();

        try
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream, Encoding.UTF8);

            // Seek to last known position
            fileStream.Seek(startPosition, SeekOrigin.Begin);

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine))
                    continue;

                try
                {
                    var journalEvent = journalEventFactory.CreateEvent(trimmedLine);
                    newEvents.Add((trimmedLine, journalEvent));
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to parse journal line: {Line}", 
                        trimmedLine.Length > 100 ? $"{trimmedLine[..100]}..." : trimmedLine);
                    // Still include raw data for potential manual processing
                    newEvents.Add((trimmedLine, null));
                }
            }
        }
        catch (IOException ex) when (ex.Message.Contains("being used by another process"))
        {
            logger.LogDebug("File {FileName} is locked by Elite Dangerous, will retry later", 
                Path.GetFileName(filePath));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error reading journal file {FileName}", Path.GetFileName(filePath));
        }

        return newEvents;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        StopMonitoring();
        _disposed = true;
        
        logger.LogDebug("JournalMonitorService disposed");
    }
}