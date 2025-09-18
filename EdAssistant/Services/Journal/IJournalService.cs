namespace EdAssistant.Services.Journal;

public interface IJournalService: IDisposable
{
    Task<IEnumerable<T>> GetLatestJournalEntriesAsync<T>() where T : JournalEvent;
    Task<IEnumerable<T>> GetAllJournalEntriesAsync<T>() where T : JournalEvent;
    Task<IEnumerable<T>> GetJournalEntriesAsync<T>(DateTime fromDate, DateTime? toDate = null) where T : JournalEvent;
    Task RefreshCacheAsync();
    void ClearCache();
}