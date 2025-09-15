namespace EdAssistant.Services.GameData;

public interface IGameDataService
{
    Task LoadAll(string journalsFolder);
    Task LoadLast(string journalsFolder);
    T? GetData<T>() where T : class;
    T? GetJournal<T>() where T : JournalEvent;
    T? GetLatestJournal<T>() where T : JournalEvent;
    IList<T> GetLatestJournals<T>() where T : JournalEvent;
    event EventHandler<GameDataLoadedEventArgs> DataLoaded;
    event EventHandler<JournalEventLoadedEventArgs> JournalLoaded;
}