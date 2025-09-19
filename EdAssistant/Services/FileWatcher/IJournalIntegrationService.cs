namespace EdAssistant.Services.FileWatcher;

public interface IJournalIntegrationService : IDisposable
{
    Task InitializeAsync();
}