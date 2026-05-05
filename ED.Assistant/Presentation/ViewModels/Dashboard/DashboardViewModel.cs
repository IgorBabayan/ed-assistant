using ED.Assistant.Domain.DTO;
using ED.Assistant.Domain.Enums;
using ED.Assistant.Extensions;

namespace ED.Assistant.Presentation.ViewModels.Dashboard;

public partial class DashboardViewModel : LoadableViewModel
{
	[ObservableProperty]
	public partial CommanderEvent? Commander { get; set; } = default;
	
	[ObservableProperty]
	public partial LoadGameEvent? LoadGame { get; set; } = default;
	
	[ObservableProperty]
	public partial ObservableCollection<RankDTO>? Ranks { get; set; } = new();
	
	[ObservableProperty]
	public partial FSDJumpEvent? CurrentSystem { get; set; } = default;

	public DashboardViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

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
