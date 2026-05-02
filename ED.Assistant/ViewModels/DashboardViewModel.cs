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
	[ObservableProperty]
	private CommanderEvent? _commander = default;

	[ObservableProperty]
	private LoadGameEvent? _loadGame = default;

	[ObservableProperty]
	private ObservableCollection<RankDTO>? _ranks = new();

	[ObservableProperty]
	private FSDJumpEvent? _currentSystem = default;

	public DashboardViewModel(ILogStorage logStorage, IPathFinder pathFinder,
		IJournalStateStore stateStore) : base(logStorage, pathFinder, stateStore) { }

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
		CurrentSystem = state?.FSDJump;
	}

	private static ushort GetMaxRank<TEnum>()
		where TEnum : struct, Enum => Enum.GetValues<TEnum>().Select(x => Convert.ToUInt16(x)).Max();

	private void ParseCommanderRanks(RankEvent? rank)
	{
		if (rank is null)
			return;

		Ranks!.Clear();
		Ranks.Add(new()
		{
			Name = "Combat",
			Value = rank.Combat,
			Maximum = GetMaxRank<CombatRankEnum>(),
			Level = ((CombatRankEnum)rank.Combat).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "Trade",
			Value = rank.Trade,
			Maximum = GetMaxRank<TradeRankEnum>(),
			Level = ((TradeRankEnum)rank.Trade).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "Explore",
			Value = rank.Explore,
			Maximum = GetMaxRank<ExploreRankEnum>(),
			Level = ((ExploreRankEnum)rank.Explore).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "Soldier",
			Value = rank.Soldier,
			Maximum = GetMaxRank<SoldierRankEnum>(),
			Level = ((SoldierRankEnum)rank.Soldier).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "Exobiologist",
			Value = rank.Exobiologist,
			Maximum = GetMaxRank<ExobiologistRankEnum>(),
			Level = ((ExobiologistRankEnum)rank.Exobiologist).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "CQC",
			Value = rank.CQC,
			Maximum = GetMaxRank<CQCRankEnum>(),
			Level = ((CQCRankEnum)rank.CQC).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "Empire",
			Value = rank.Empire,
			Maximum = GetMaxRank<EmpireRankEnum>(),
			Level = ((EmpireRankEnum)rank.Empire).GetDisplayName()
		});
		Ranks.Add(new()
		{
			Name = "Federation",
			Value = rank.Federation,
			Maximum = GetMaxRank<FederationRankEnum>(),
			Level = ((FederationRankEnum)rank.Federation).GetDisplayName()
		});
	}
}
