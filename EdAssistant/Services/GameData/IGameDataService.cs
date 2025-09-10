namespace EdAssistant.Services.GameData;

public interface IGameDataService : IDisposable
{
    T? GetData<T>() where T : class;
    object? GetData(Type type);
    Task LoadAllGameDataAsync(string journalsFolder);
    List<T> GetJournalEvents<T>() where T : JournalEvent;
    List<JournalEvent> GetJournalEvents(Type eventType);
    T? GetLatestJournalEvent<T>() where T : JournalEvent;
    void ClearOldEvents();

    event EventHandler<GameDataLoadedEventArgs>? DataLoaded;
    event EventHandler<JournalEventLoadedEventArgs>? JournalEventLoaded;
}