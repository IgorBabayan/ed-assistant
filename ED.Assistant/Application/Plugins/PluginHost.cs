using System.Diagnostics;
using ED.Assistant.Application.Path;
using ED.Assistant.Application.Settings;
using ED.Assistant.Plugins;

namespace ED.Assistant.Application.Plugins;

internal sealed class PluginHost : IPluginHost, IDisposable
{
    private readonly ISettingsStorage _settingsStorage;
    private readonly IPathFinder _pathFinder;
    private readonly IJournalStateStore _stateStore;

    public event EventHandler? JournalChanged;

    public PluginHost(ISettingsStorage settingsStorage, IPathFinder pathFinder, IJournalStateStore stateStore)
    {
        _settingsStorage = settingsStorage;
        _pathFinder = pathFinder;
        _stateStore = stateStore;

        _stateStore.StateChanged += OnStateChanged;
    }

    public async Task<string> GetJournalFolderAsync(CancellationToken cancellationToken = default)
    {
        var settings = await _settingsStorage.LoadAsync(_pathFinder.GetConfigPath(), cancellationToken);
        return string.IsNullOrWhiteSpace(settings.LogFolder) ? _pathFinder.GetPathToLogs() : settings.LogFolder;
    }

    public void Dispose() => _stateStore.StateChanged -= OnStateChanged;

    private void OnStateChanged(object? sender, JournalState state)
    {
        if (JournalChanged is not { } handlers)
            return;

        // One throwing plugin must not break state publication or the other plugins
        foreach (var handler in handlers.GetInvocationList().Cast<EventHandler>())
        {
            try
            {
                handler(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Plugin JournalChanged handler failed: {ex}");
            }
        }
    }
}
