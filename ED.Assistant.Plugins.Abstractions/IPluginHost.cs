namespace ED.Assistant.Plugins;

/// <summary>Host services a plugin can take from DI.</summary>
public interface IPluginHost
{
    /// <summary>The journal folder the host reads: the one from settings, or the platform default.</summary>
    Task<string> GetJournalFolderAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Raised on a background thread whenever the host has read journal lines
    /// (startup load, manual reload, or the auto-watcher). Fires whether or not a
    /// plugin page is on screen.
    /// </summary>
    event EventHandler? JournalChanged;
}
