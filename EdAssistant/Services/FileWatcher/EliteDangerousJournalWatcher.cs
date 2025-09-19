namespace EdAssistant.Services.FileWatcher;

class EliteDangerousJournalWatcher(ILogger<EliteDangerousJournalWatcher> logger) : IEliteDangerousJournalWatcher
{
    private string? _journalPath;
    private FileSystemWatcher? _fileWatcher;
    private bool _disposed;

    public event EventHandler<FileChangedEventArgs>? FileChanged;
    public event EventHandler<FileChangedEventArgs>? FileCreated;
    
    public void SetupFolderPath(string folderPath)
    {
        if (string.IsNullOrWhiteSpace(folderPath))
            throw new ArgumentNullException(nameof(folderPath));
        
        if (!Directory.Exists(folderPath))
            throw new DirectoryNotFoundException(string.Format(Localization.Instance["Exceptions.DirectoryNotFound"], folderPath));
        
        _journalPath = folderPath;
    }
    
    public void StartWatching()
    {
        if (_fileWatcher is null)
        {
            StopWatching();
        }

        _fileWatcher = new (_journalPath!)
        {
            Filter = "Journal.*.log",
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size,
            EnableRaisingEvents = true,
            IncludeSubdirectories = false
        };

        _fileWatcher.Changed += OnFileChanged;
        _fileWatcher.Created += OnFileCreated;

        logger.LogInformation(Localization.Instance["JournalWatcher.Information.StartedWatchingJournalFiles"], _journalPath);
    }
    
    public void StopWatching()
    {
        if (_fileWatcher is not null)
        {
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.Changed -= OnFileChanged;
            _fileWatcher.Created -= OnFileCreated;
            _fileWatcher.Dispose();
            _fileWatcher = null;
            
            logger.LogInformation(Localization.Instance["JournalWatcher.Information.StoppedWatchingJournalFiles"]);
        }
    }

    public void Restart()
    {
        StopWatching();
        StartWatching();
    }
    
    public void Dispose()
    {
        if (!_disposed)
        {
            StopWatching();
            _disposed = true;
        }
    }
    
    private void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        // Debounce rapid file changes
        Task.Delay(100).ContinueWith(_ =>
        {
            try
            {
                var args = new FileChangedEventArgs(e.FullPath, FileChangeType.Modified);
                FileChanged?.Invoke(this, args);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, Localization.Instance["JournalWatcher.Exceptions.ErrorHandlingFileChange"],
                    exception.Message);
            }
        });
    }
    
    private void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        try
        {
            var args = new FileChangedEventArgs(e.FullPath, FileChangeType.Created);
            FileCreated?.Invoke(this, args);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["JournalWatcher.Exceptions.ErrorHandlingFileChange"], 
                exception.Message);
        }
    }
}