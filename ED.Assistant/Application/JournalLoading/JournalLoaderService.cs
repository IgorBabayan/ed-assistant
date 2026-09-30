using ED.Assistant.Application.Evaluation;
using ED.Assistant.Application.Path;
using ED.Assistant.Application.Settings;
using ED.Assistant.Application.Storage;

namespace ED.Assistant.Application.JournalLoading;

internal sealed class JournalLoaderService : IJournalLoaderService, IDisposable
{
	private readonly IPathFinder _pathFinder;
	private readonly IJournalWatchService _watcher;
	private readonly SemaphoreSlim _gate = new(1, 1);
	private readonly ILogStorage _logStorage;
	private readonly IJournalStateStore _stateStore;
	private readonly IEvaluatorSyncService _evaluatorSync;
	private readonly ISettingsStorage _settingsStorage;

	public JournalLoaderService(IPathFinder pathFinder, ILogStorage logStorage, IJournalStateStore stateStore,
		IEvaluatorSyncService evaluatorSync, ISettingsStorage settingsStorage, IJournalWatchService watcher)
	{
		_pathFinder = pathFinder;
		_watcher = watcher;
		_logStorage = logStorage;
		_stateStore = stateStore;
		_evaluatorSync = evaluatorSync;
		_settingsStorage = settingsStorage;
	}

	public async Task LoadLastLogsAsync(CancellationToken cancellationToken = default)
	{
        await _gate.WaitAsync(cancellationToken);
        try
        {
            var settings = await _settingsStorage.LoadAsync(_pathFinder.GetConfigPath(), cancellationToken);
            var folder = string.IsNullOrWhiteSpace(settings.LogFolder) ? _pathFinder.GetPathToLogs() : settings.LogFolder;
            var resumeWatch = _watcher.IsRunning;
            await _watcher.StopAsync();
            try
            {
                var state = await _logStorage.LoadLastLogsAsync(folder, settings.ReadLogsForDays, cancellationToken);
                await _evaluatorSync.SyncAsync(state, cancellationToken);
                _stateStore.Update(state);
            }
            finally
            {
                if (resumeWatch && !cancellationToken.IsCancellationRequested)
                    await _watcher.StartAsync(folder, cancellationToken);
            }
        }
        finally
        {
            _gate.Release();
        }
	}

    public void Dispose() => _gate.Dispose();
}
