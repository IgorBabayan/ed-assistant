namespace EdAssistant.Services.GameData;

public interface IGameDataService
{
    Task LoadAllGameDataAsync(string journalsFolder);
    T? GetData<T>() where T : class;
    object? GetData(Type type);
    T? GetJournal<T>() where T : JournalEvent;
    T? GetLatestJournal<T>() where T : JournalEvent;
    event EventHandler<GameDataLoadedEventArgs> DataLoaded;
    event EventHandler<JournalEventLoadedEventArgs> JournalLoaded;
}