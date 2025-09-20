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

    public void Start()
    {
        if (IsMonitoring)
        {
            logger.LogWarning(Localization.Instance["JournalWatcher.Information.JournalMonitoringIsAlreadyRunning"]);
            return;
        }

        var journalPath = GetCurrentJournalPath();
        if (string.IsNullOrWhiteSpace(journalPath) || !Directory.Exists(journalPath))
        {
            logger.LogInformation(Localization.Instance["JournalWatcher.Information.JournalFolderNotSet"]);
            return;
        }
        StartMonitoringInternal(journalPath);
    }

    public void Restart(string newJournalPath)
    {
        if (string.IsNullOrWhiteSpace(newJournalPath))
        {
            var message = Localization.Instance["JournalWatcher.Information.JournalPathIsNullOrEmpty"];
            logger.LogInformation(message);
            throw new ArgumentException(message, nameof(newJournalPath));
        }

        if (!Directory.Exists(newJournalPath))
        {
            var message = string.Format(Localization.Instance["Exceptions.DirectoryNotFound"], newJournalPath);
            logger.LogInformation(message);
            throw new DirectoryNotFoundException(message);
        }

        var oldPath = _journalPath;
        logger.LogInformation(Localization.Instance["JournalWatcher.Information.RestartingJournalMonitoring"], 
            oldPath ?? "-", newJournalPath);

        try
        {
            StopMonitoringInternal();
            lock (_lock)
            {
                _filePositions.Clear();
            }

            settingsService.SetSetting("JournalFolderPath", newJournalPath);

            StartMonitoringInternal(newJournalPath);
            
            logger.LogInformation(Localization.Instance["JournalWatcher.Information.SuccessfullyRestartedJournalMonitoring"], newJournalPath);
            JournalPathChanged?.Invoke(this, new JournalPathChangedEventArgs(oldPath ?? string.Empty, newJournalPath));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["JournalWatcher.Information.SuccessfullyRestartedJournalMonitoring"], 
                newJournalPath);
            
            if (!string.IsNullOrEmpty(oldPath) && Directory.Exists(oldPath))
            {
                logger.LogInformation(Localization.Instance["JournalWatcher.Information.AttemptingToRestorePreviousJournalPath"], oldPath);
                try
                {
                    StartMonitoringInternal(oldPath);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, Localization.Instance["JournalWatcher.Exceptions.FailedToRestorePreviousJournalMonitoring"]);
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
        logger.LogInformation(Localization.Instance["JournalWatcher.Information.JournalMonitoringStopped"]);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        StopMonitoring();
        _disposed = true;
        
        logger.LogDebug(Localization.Instance["JournalWatcher.Information.ServiceDisposed"]);
    }

    private string GetCurrentJournalPath()
    {
        var savedPath = settingsService.GetSetting<string>("JournalFolderPath");
        if (!string.IsNullOrEmpty(savedPath) && Directory.Exists(savedPath))
        {
            return savedPath;
        }

        return folderPickerService.TryGetDefaultJournalsPath(out var path)
            ? path
            : string.Empty;
    }

    private void StartMonitoringInternal(string journalPath)
    {
        if (string.IsNullOrEmpty(journalPath) || !Directory.Exists(journalPath))
        {
            var message = string.Format(Localization.Instance["Exceptions.DirectoryNotFound"], journalPath);
            logger.LogInformation(message);
            throw new DirectoryNotFoundException(message);
        }

        _journalPath = journalPath;
        try
        {
            InitializeFilePositions();

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
            logger.LogInformation(Localization.Instance["JournalWatcher.Information.StartedWatchingJournalFiles"], _journalPath);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["JournalWatcher.Exceptions.FailedToStartJournalMonitoring"], journalPath);
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

    private void InitializeFilePositions()
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
                    _filePositions[file] = fileInfo.Length;
                }
                
                logger.LogDebug(Localization.Instance["JournalWatcher.Information.InitializedPosition"], 
                    Path.GetFileName(file), fileInfo.Length);
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, Localization.Instance["JournalWatcher.Exceptions.FailedToInitializePosition"], 
                    Path.GetFileName(file));
            }
        }
    }

    private async void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        try
        {
            logger.LogInformation(Localization.Instance["JournalWatcher.Information.NewJournalFileCreated"], 
                Path.GetFileName(e.FullPath));
            
            lock (_lock)
            {
                _filePositions[e.FullPath] = 0;
            }
            
            NewJournalFileCreated?.Invoke(this, new JournalFileEventArgs(e.FullPath));
            
            await Task.Delay(100);
            await ProcessFileChangesAsync(e.FullPath);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["JournalWatcher.Information.ErrorHandlingFileCreation"], 
                Path.GetFileName(e.FullPath));
        }
    }

    private async void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        try
        {
            await Task.Delay(50);
            await ProcessFileChangesAsync(e.FullPath);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["JournalWatcher.Information.ErrorHandlingFileChange"], 
                Path.GetFileName(e.FullPath));
        }
    }

    private void OnWatcherError(object sender, ErrorEventArgs args)
    {
        logger.LogError(args.GetException(), Localization.Instance["FileSystemWatcher.Exceptions.ErrorOccurred"]);
        
        Task.Run(async () =>
        {
            try
            {
                await Task.Delay(1000);
                if (!IsMonitoring && !string.IsNullOrEmpty(_journalPath))
                {
                    logger.LogInformation(Localization.Instance["JournalWatcher.Information.RestartJournalMonitoringAfterError"]);
                    StartMonitoringInternal(_journalPath);
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception, Localization.Instance["JournalWatcher.Information.FailedToRestartMonitoringAfterWatcherError"]);
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

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length <= lastPosition)
                return;

            logger.LogDebug(Localization.Instance["JournalWatcher.Debug.ProcessingChanges"], fileName, lastPosition);
            var newEvents = await ReadNewEntriesAsync(filePath, lastPosition);
            foreach (var (rawJson, journalEvent) in newEvents)
            {
                try
                {
                    JournalEventReceived?.Invoke(this, new JournalEventArgs(journalEvent!, rawJson, fileName));
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, Localization.Instance["JournalWatcher.Exceptions.ErrorInvokingJournalEvent"], 
                        journalEvent?.GetType().Name ?? Localization.Instance["Common.Unknown"]);
                }
            }

            lock (_lock)
            {
                _filePositions[filePath] = fileInfo.Length;
            }
            
            if (newEvents.Count > 0)
            {
                logger.LogDebug(Localization.Instance["JournalWatcher.Debug.ProcessedNewJournalEvents"], 
                    newEvents.Count, fileName);
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["JournalWatcher.Exceptions.ErrorProcessingFileChanges"], 
                Path.GetFileName(filePath));
        }
    }

    private async Task<List<(string RawJson, JournalEvent? Event)>> ReadNewEntriesAsync(string filePath, long startPosition)
    {
        var newEvents = new List<(string, JournalEvent?)>();
        try
        {
            await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream, Encoding.UTF8);

            fileStream.Seek(startPosition, SeekOrigin.Begin);
            while (await reader.ReadLineAsync() is { } line)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine))
                    continue;

                try
                {
                    var journalEvent = journalEventFactory.CreateEvent(trimmedLine);
                    newEvents.Add((trimmedLine, journalEvent));
                }
                catch (Exception exception)
                {
                    logger.LogWarning(exception, Localization.Instance["JournalWatcher.Warning.FailedToParseJournalLine"], 
                        trimmedLine.Length > 100 ? $"{trimmedLine[..100]}..." : trimmedLine);
                    
                    newEvents.Add((trimmedLine, null));
                }
            }
        }
        catch (IOException exception) when (exception.Message.Contains("being used by another process"))
        {
            logger.LogDebug(Localization.Instance["Exceptions.FileIsLocked"], Path.GetFileName(filePath));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["JournalWatcher.Exceptions.ErrorReadingJournalFile"], Path.GetFileName(filePath));
        }

        return newEvents;
    }
}