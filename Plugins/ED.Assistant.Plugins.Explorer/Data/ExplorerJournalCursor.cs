namespace ED.Assistant.Plugins.Explorer.Data;

/// <summary>How far the journals have been read. A single row.</summary>
public sealed class ExplorerJournalCursor
{
    public const int SingletonId = 1;

    public int Id { get; set; } = SingletonId;
    public string FileName { get; set; } = string.Empty;
    /// <summary>Byte offset just after the last complete line read.</summary>
    public long Position { get; set; }
    public DateTime DateUpdated { get; set; }
}
