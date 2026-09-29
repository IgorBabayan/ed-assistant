using ED.Assistant.Application.Evaluation;
using ED.Assistant.Application.Path;
using ED.Assistant.Application.Settings;
using ED.Assistant.Application.Storage;

namespace ED.Assistant.Application.JournalLoading;

class JournalLoaderService : IJournalLoaderService
{
	private readonly IPathFinder _pathFinder;
	private readonly ILogStorage _logStorage;
	private readonly IJournalStateStore _stateStore;
	private readonly IEvaluatorSyncService _evaluatorSync;
	private readonly ISettingsStorage _settingsStorage;

	public JournalLoaderService(IPathFinder pathFinder, ILogStorage logStorage, IJournalStateStore stateStore,
		IEvaluatorSyncService evaluatorSync, ISettingsStorage settingsStorage)
	{
		_pathFinder = pathFinder;
		_logStorage = logStorage;
		_stateStore = stateStore;
		_evaluatorSync = evaluatorSync;
		_settingsStorage = settingsStorage;
	}

	public async Task LoadLastLogsAsync(CancellationToken cancellationToken = default)
	{
		var folder = _pathFinder.GetPathToLogs();
		var settings = await _settingsStorage.LoadAsync(_pathFinder.GetConfigPath(), cancellationToken);

		var state = await _logStorage.LoadLastLogsAsync(folder, settings.ReadLogsForDays, cancellationToken);
		await _evaluatorSync.SyncAsync(state, cancellationToken);

		_stateStore.Update(state);
	}
}