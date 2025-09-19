namespace EdAssistant.Services.FileWatcher;

public interface IJournalMonitorService : IDisposable
{
    event EventHandler<JournalEventArgs> JournalEventReceived;
    event EventHandler<JournalFileEventArgs> NewJournalFileCreated;
    event EventHandler<JournalPathChangedEventArgs> JournalPathChanged;
    Task StartAsync();
    Task RestartAsync(string newJournalPath);
    void StopMonitoring();
    bool IsMonitoring { get; }
}