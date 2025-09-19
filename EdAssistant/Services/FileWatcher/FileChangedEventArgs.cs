namespace EdAssistant.Services.FileWatcher;

public class FileChangedEventArgs(string filePath, FileChangeType changeType) : EventArgs
{
    public string FilePath { get; } = filePath;
    public FileChangeType ChangeType { get; } = changeType;
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}