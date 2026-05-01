using ED.Assistant.Data.Models.Enums;
using ED.Assistant.Data.Models.Events;
using ED.Assistant.Data.Services.Events;
using ED.Assistant.Data.Services.Path;
using ED.Assistant.DTO;
using ED.Assistant.Extensions;
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

		Ranks!.Clear();
		Ranks.Add(new()
		{
			Name = "Combat",
			Progress = rank.Combat,
			Level = ((CombatRankEnum)rank.Combat).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "Trade",
			Progress = rank.Trade,
			Level = ((TradeRankEnum)rank.Trade).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "Explore",
			Progress = rank.Explore,
			Level = ((ExploreRankEnum)rank.Explore).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "Soldier",
			Progress = rank.Soldier,
			Level = ((SoldierRankEnum)rank.Soldier).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "Exobiologist",
			Progress = rank.Exobiologist,
			Level = ((ExobiologistRankEnum)rank.Exobiologist).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "CQC",
			Progress = rank.CQC,
			Level = ((CQCRankEnum)rank.CQC).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "Empire",
			Progress = rank.Empire,
			Level = ((EmpireRankEnum)rank.Empire).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "Federation",
			Progress = rank.Federation,
			Level = ((FederationRankEnum)rank.Federation).GetDisplayName()
		});
	}
}
