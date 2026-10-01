namespace ED.Assistant.Plugins;

/// <summary>
/// Shows a system notification when the main window isn't active.
/// The host registers a platform default; a plugin can replace it by registering its own
/// implementation in <see cref="IPlugin.ConfigureServices"/> (plugins are registered after the host,
/// so the last registration wins).
/// </summary>
public interface IDesktopNotifier
{
    /// <summary>Called on the UI thread: must not block (start the work and return).</summary>
    void Show(string title, string message);
}
