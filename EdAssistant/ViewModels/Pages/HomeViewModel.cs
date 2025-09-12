namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.Home)]
public sealed partial class HomeViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;

    [ObservableProperty]
    private ObservableCollection<RankDTO> ranks = [];

    public HomeViewModel(IGameDataService gameDataService)
    {
        _gameDataService = gameDataService;
        _gameDataService.JournalLoaded += OnJournalLoaded;

        ProcessRankEvent();
        ProcessProgressEvent();
    }

    public override void Dispose() => _gameDataService.JournalLoaded -= OnJournalLoaded;

    private void OnJournalLoaded(object? sender, JournalEventLoadedEventArgs e)
    {
        switch (e)
        {
            case { EventType: JournalEventType.Rank, Event: RankEvent rank }:
                ProcessRank(rank);
                break;

            case { EventType: JournalEventType.Progress, Event: ProgressEvent progress }:
                ProcessProgress(progress);
                break;
        }
    }

    private void ProcessRankEvent()
    {
        var rankEvent = _gameDataService.GetLatestJournal<RankEvent>();
        if (rankEvent is not null)
        {
            ProcessRank(rankEvent);
        }
    }

    private void ProcessProgressEvent()
    {
        var progressEvent = _gameDataService.GetLatestJournal<ProgressEvent>();
        if (progressEvent is not null)
        {
            ProcessProgress(progressEvent);
        }
    }

    private void ProcessRank(RankEvent rank)
    {
        Ranks.Clear();

        Ranks.Add(new()
        {
            Rank = RankEnum.Combat,
            Title = RankEnum.Combat.GetRankTitle(rank.Combat),
            Progress = 0
        });

        Ranks.Add(new()
        {
            Rank = RankEnum.Trade,
            Title = RankEnum.Trade.GetRankTitle(rank.Trade),
            Progress = 0
        });

        Ranks.Add(new()
        {
            Rank = RankEnum.Explore,
            Title = RankEnum.Explore.GetRankTitle(rank.Explore),
            Progress = 0
        });

        Ranks.Add(new()
        {
            Rank = RankEnum.Soldier,
            Title = RankEnum.Soldier.GetRankTitle(rank.Soldier),
            Progress = 0
        });

        Ranks.Add(new()
        {
            Rank = RankEnum.Exobiologist,
            Title = RankEnum.Exobiologist.GetRankTitle(rank.Exobiologist),
            Progress = 0
        });

        Ranks.Add(new()
        {
            Rank = RankEnum.Empire,
            Title = RankEnum.Empire.GetRankTitle(rank.Empire),
            Progress = 0
        });

        Ranks.Add(new()
        {
            Rank = RankEnum.Federation,
            Title = RankEnum.Federation.GetRankTitle(rank.Federation),
            Progress = 0
        });

        Ranks.Add(new()
        {
            Rank = RankEnum.CQC,
            Title = RankEnum.CQC.GetRankTitle(rank.CQC),
            Progress = 0
        });
    }

    private void ProcessProgress(ProgressEvent progress)
    {
        if (Ranks.Count == 0)
            return;

        Ranks.First(x => x.Rank == RankEnum.Combat).Progress = progress.Combat;
        Ranks.First(x => x.Rank == RankEnum.Trade).Progress = progress.Trade;
        Ranks.First(x => x.Rank == RankEnum.Explore).Progress = progress.Explore;
        Ranks.First(x => x.Rank == RankEnum.Soldier).Progress = progress.Soldier;
        Ranks.First(x => x.Rank == RankEnum.Exobiologist).Progress = progress.Exobiologist;
        Ranks.First(x => x.Rank == RankEnum.Empire).Progress = progress.Empire;
        Ranks.First(x => x.Rank == RankEnum.Federation).Progress = progress.Federation;
        Ranks.First(x => x.Rank == RankEnum.CQC).Progress = progress.CQC;
    }
}