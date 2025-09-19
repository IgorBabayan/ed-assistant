namespace EdAssistant.Services.FileWatcher;

class JournalMonitorService(IEliteDangerousJournalWatcher watcher, ILogger<JournalMonitorService> logger) 
    : IJournalMonitorService
{
    private string _lastProcessedLine = string.Empty;
    private long _lastFilePosition = 0;
    
    public void Initialize(string journalPath)
    {
        watcher.SetupFolderPath(journalPath);
        watcher.FileChanged += OnJournalFileChanged;
        watcher.FileCreated += OnNewJournalFileCreated;
        watcher.StartWatching();
    }
    
    public void Dispose() => watcher?.Dispose();

    private async void OnJournalFileChanged(object sender, FileChangedEventArgs e)
    {
        logger.LogInformation(Localization.Instance["JournalWatcher.Information.JournalFileModified"], Path.GetFileName(e.FilePath));
        await ProcessNewJournalEntries(e.FilePath);
    }
    
    private async Task ProcessNewJournalEntries(string filePath)
    {
        try
        {
            await Task.Delay(100);

            await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream);

            // Seek to last known position
            fileStream.Seek(_lastFilePosition, SeekOrigin.Begin);

            while (await reader.ReadLineAsync() is { } line)
            {
                if (!string.IsNullOrWhiteSpace(line) && !string.Equals(line, _lastProcessedLine, StringComparison.OrdinalIgnoreCase))
                {
                    await ProcessJournalLine(line);
                    _lastProcessedLine = line;
                }
            }

            _lastFilePosition = fileStream.Position;
        }
        catch (IOException exception)
        {
            logger.LogError(exception, Localization.Instance["Exceptions.FileIOError"], exception.Message);
            await Task.Delay(500);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["JournalService.Exceptions.FailedToCreateJournalEvent"], exception.Message);
        }
    }
    
    private async Task ProcessJournalLine(string jsonLine)
    {
        try
        {
            //! TODO: process log
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["JournalService.Exceptions.ErrorGettingJournalFiles"], exception.Message);
        }
    }
    
    private async void OnNewJournalFileCreated(object sender, FileChangedEventArgs e)
    {
        logger.LogInformation(Localization.Instance["JournalWatcher.Information.NewJournalFileCreated"], Path.GetFileName(e.FilePath));
        _lastFilePosition = 0;
        await ProcessNewJournalEntries(e.FilePath);
    }
}