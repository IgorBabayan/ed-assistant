using Avalonia.Threading;
using ED.Assistant.Domain.DTO;
using ED.Assistant.Domain.Enums;
using ED.Assistant.Extensions;
using ED.Assistant.Presentation.Helpers.Dashboard;

namespace ED.Assistant.Presentation.ViewModels.Dashboard;

public partial class DashboardViewModel : LoadableViewModel
{
	private const int RecentEventsDisplayLimit = 8;

	[ObservableProperty]
	public partial CommanderEvent? Commander { get; set; } = default;

	[ObservableProperty]
	public partial LoadGameEvent? LoadGame { get; set; } = default;

	[ObservableProperty]
	public partial ObservableCollection<RankDTO>? Ranks { get; set; } = new();

	[ObservableProperty]
	public partial FSDJumpEvent? CurrentSystem { get; set; } = default;

	public ObservableCollection<DashboardSignalViewModel> Signals { get; } = [];

	public ObservableCollection<RecentEventViewModel> RecentEvents { get; } = [];

	public bool HasSignals => Signals.Count > 0;

	public bool HasRecentEvents => RecentEvents.Count > 0;

	public DashboardViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

	protected override void UpdateFromState(JournalState state)
	{
		if (state is null)
			return;

		// Build display rows here: the journal watcher calls this right after applying
		// new lines, so the state is consistent now and may change later.
		var signals = DashboardSignalsBuilder.Build(state, DateTime.UtcNow);
		var recentEvents = RecentEventsBuilder.Build(state.RecentEvents, RecentEventsDisplayLimit, DateTime.Now);

		var commander = state.Commander;
		var ranks = state.Ranks;
		var loadGame = state.LoadGame;
		var currentSystem = state.FSDJump;

		// StateChanged is raised on the watcher thread; bound collections must change on the UI thread.
		RunOnUIThread(() =>
		{
			Commander = commander;
			ParseCommanderRanks(ranks);

			LoadGame = loadGame;
			CurrentSystem = currentSystem;

			Replace(Signals, signals);
			Replace(RecentEvents, recentEvents);

			OnPropertyChanged(nameof(HasSignals));
			OnPropertyChanged(nameof(HasRecentEvents));
		});
	}

	private static void RunOnUIThread(Action action)
	{
		if (Dispatcher.UIThread.CheckAccess())
			action();
		else
			Dispatcher.UIThread.Post(action);
	}

	private static void Replace<T>(ObservableCollection<T> target, IEnumerable<T> items)
	{
		target.Clear();

		foreach (var item in items)
			target.Add(item);
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