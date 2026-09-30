using ED.Assistant.Domain.DTO;
using ED.Assistant.Domain.Enums;
using ED.Assistant.Extensions;
using ED.Assistant.Presentation.Collections;
using ED.Assistant.Presentation.Helpers.Dashboard;

namespace ED.Assistant.Presentation.ViewModels.Dashboard;

public partial class DashboardViewModel : LoadableViewModel
{
	private const int RecentEventsDisplayLimit = 8;

	// Enum ranges never change: compute once instead of on every update
	private static readonly ushort MaxCombat = GetMaxRank<CombatRankEnum>();
	private static readonly ushort MaxTrade = GetMaxRank<TradeRankEnum>();
	private static readonly ushort MaxExplore = GetMaxRank<ExploreRankEnum>();
	private static readonly ushort MaxSoldier = GetMaxRank<SoldierRankEnum>();
	private static readonly ushort MaxExobiologist = GetMaxRank<ExobiologistRankEnum>();
	private static readonly ushort MaxCqc = GetMaxRank<CQCRankEnum>();
	private static readonly ushort MaxEmpire = GetMaxRank<EmpireRankEnum>();
	private static readonly ushort MaxFederation = GetMaxRank<FederationRankEnum>();

	// Last RankEvent turned into rows; the event object is replaced only when a new Rank line arrives
	private RankEvent? _lastRanks;

	[ObservableProperty]
	public partial CommanderEvent? Commander { get; private set; }

	[ObservableProperty]
	public partial LoadGameEvent? LoadGame { get; private set; }

	[ObservableProperty]
	public partial ObservableCollection<RankDTO>? Ranks { get; set; } = [];

	[ObservableProperty]
	public partial FSDJumpEvent? CurrentSystem { get; private set; }

	public BulkObservableCollection<DashboardSignalViewModel> Signals { get; } = [];

	public BulkObservableCollection<RecentEventViewModel> RecentEvents { get; } = [];

	public bool HasSignals => Signals.Count > 0;

	public bool HasRecentEvents => RecentEvents.Count > 0;

	public DashboardViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

	protected override void UpdateFromState(JournalState state)
	{
		// Build display rows here: the journal watcher calls this right after applying
		// new lines, so the state is consistent now and may change later.
		var signals = DashboardSignalsBuilder.Build(state, DateTime.UtcNow).ToList();
		var recentEvents = RecentEventsBuilder.Build(state.RecentEvents, RecentEventsDisplayLimit, DateTime.Now).ToList();

		var commander = state.Commander;
		var loadGame = state.LoadGame;
		var currentSystem = state.FSDJump;

		var ranks = state.Ranks;
		List<RankDTO>? rankRows = null;

		if (!ReferenceEquals(ranks, _lastRanks))
		{
			rankRows = ranks is null ? [] : BuildRanks(ranks);
			_lastRanks = ranks;
		}

		// StateChanged is raised on the watcher thread; bound collections must change on the UI thread.
		RunOnUIThread(() =>
		{
			Commander = commander;
			LoadGame = loadGame;
			CurrentSystem = currentSystem;

			if (rankRows is not null)
				Ranks = new ObservableCollection<RankDTO>(rankRows);

			Signals.ReplaceAll(signals);
			RecentEvents.ReplaceAll(recentEvents);

			OnPropertyChanged(nameof(HasSignals));
			OnPropertyChanged(nameof(HasRecentEvents));
		});
	}

	private static ushort GetMaxRank<TEnum>()
		where TEnum : struct, Enum => Enum.GetValues<TEnum>().Select(x => Convert.ToUInt16(x)).Max();

	private static List<RankDTO> BuildRanks(RankEvent rank) =>
	[
		new()
		{
			Name = "Combat",
			Value = rank.Combat,
			Maximum = MaxCombat,
			Level = ((CombatRankEnum)rank.Combat).GetDisplayName()
		},
		new()
		{
			Name = "Trade",
			Value = rank.Trade,
			Maximum = MaxTrade,
			Level = ((TradeRankEnum)rank.Trade).GetDisplayName()
		},
		new()
		{
			Name = "Explore",
			Value = rank.Explore,
			Maximum = MaxExplore,
			Level = ((ExploreRankEnum)rank.Explore).GetDisplayName()
		},
		new()
		{
			Name = "Soldier",
			Value = rank.Soldier,
			Maximum = MaxSoldier,
			Level = ((SoldierRankEnum)rank.Soldier).GetDisplayName()
		},
		new()
		{
			Name = "Exobiologist",
			Value = rank.Exobiologist,
			Maximum = MaxExobiologist,
			Level = ((ExobiologistRankEnum)rank.Exobiologist).GetDisplayName()
		},
		new()
		{
			Name = "CQC",
			Value = rank.CQC,
			Maximum = MaxCqc,
			Level = ((CQCRankEnum)rank.CQC).GetDisplayName()
		},
		new()
		{
			Name = "Empire",
			Value = rank.Empire,
			Maximum = MaxEmpire,
			Level = ((EmpireRankEnum)rank.Empire).GetDisplayName()
		},
		new()
		{
			Name = "Federation",
			Value = rank.Federation,
			Maximum = MaxFederation,
			Level = ((FederationRankEnum)rank.Federation).GetDisplayName()
		}
	];
}