namespace EdAssistant.Services.FileWatcher;

public class JournalFileEventArgs(string filePath) : EventArgs
{
    public string FilePath { get; } = filePath;
    public string FileName { get; } = Path.GetFileName(filePath);
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}