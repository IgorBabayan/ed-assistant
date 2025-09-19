namespace EdAssistant.Services.FileWatcher;

public interface IJournalMonitorService : IDisposable
{
    void Initialize(string journalPath);
}