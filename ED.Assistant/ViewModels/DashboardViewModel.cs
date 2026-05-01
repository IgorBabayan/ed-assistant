using ED.Assistant.Data.Models.Events;
using ED.Assistant.Data.Services.Events;
using ED.Assistant.Data.Services.Path;
using ED.Assistant.DTO;
using ED.Assistant.Services.Journal;
using System.Collections.ObjectModel;

namespace ED.Assistant.ViewModels;

public partial class DashboardViewModel : BaseViewModel, ILoadableViewModel
{
	private readonly ILogStorage _logStorage;
	private readonly IPathFinder _pathFinder;

	[ObservableProperty]
	private CommanderEvent? _commander = default;

	[ObservableProperty]
	private LoadGameEvent? _loadGame = default;

	[ObservableProperty]
	private ObservableCollection<RankDTO>? _ranks = new();

	public DashboardViewModel(ILogStorage logStorage, IPathFinder pathFinder,
		IJournalStateStore stateStore) : base(stateStore)
	{
		_logStorage = logStorage;
		_pathFinder = pathFinder;
	}

	[RelayCommand]
	private async Task Load(CancellationToken cancellationToken = default)
	{
		var folder = _pathFinder.GetPathToLogs();
		var state = await _logStorage.LoadLastLogsAsync(folder, cancellationToken);

		_stateStore.Update(state);
	}

	protected override void UpdateFromState(JournalState state)
	{
		Commander = state?.Commander;
		ParseCommanderRanks(state?.Ranks);

		LoadGame = state?.LoadGame;
	}

	private void ParseCommanderRanks(RankEvent? rank)
	{
		if (rank is null)
			return;

		
		Ranks!.Add(new()
		{
			Name = 
		})
	}
}
