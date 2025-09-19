namespace EdAssistant.Services.FileWatcher;

public class JournalEventArgs(JournalEvent journalEvent, string rawJson, string fileName) : EventArgs
{
    public JournalEvent JournalEvent { get; } = journalEvent;
    public string RawJson { get; } = rawJson;
    public string FileName { get; } = fileName;
}