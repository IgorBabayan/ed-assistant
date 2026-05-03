using ED.Assistant.Data.Services.Events;
using ED.Assistant.Data.Services.Path;

namespace ED.Assistant.Services.Journal;

public interface IJournalLoaderService
{
	Task LoadLastLogsAsync(CancellationToken cancellationToken = default);
}

class JournalLoaderService : IJournalLoaderService
{
	private readonly IPathFinder _pathFinder;
	private readonly ILogStorage _logStorage;
	private readonly IJournalStateStore _stateStore;

	public JournalLoaderService(IPathFinder pathFinder, ILogStorage logStorage, IJournalStateStore stateStore)
	{
		_pathFinder = pathFinder;
		_logStorage = logStorage;
		_stateStore = stateStore;
	}

	public async Task LoadLastLogsAsync(CancellationToken cancellationToken = default)
	{
		var folder = _pathFinder.GetPathToLogs();
		var state = await _logStorage.LoadLastLogsAsync(folder, cancellationToken);

		_stateStore.Update(state);
	}
}