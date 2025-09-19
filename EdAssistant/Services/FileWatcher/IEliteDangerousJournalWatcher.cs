namespace EdAssistant.Services.FileWatcher;

public interface IEliteDangerousJournalWatcher : IDisposable
{
    event EventHandler<FileChangedEventArgs>? FileChanged;
    event EventHandler<FileChangedEventArgs>? FileCreated;

    void SetupFolderPath(string folderPath);
    void StartWatching();
    void StopWatching();
    void Restart();
}