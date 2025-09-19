namespace EdAssistant.Services.FileWatcher;

public class JournalPathChangedEventArgs(string oldPath, string newPath) : EventArgs
{
    public string OldPath { get; } = oldPath;
    public string NewPath { get; } = newPath;
    public DateTime ChangedAt { get; } = DateTime.UtcNow;
}