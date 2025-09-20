namespace EdAssistant.ViewModels.Pages;

public sealed partial class HomeViewModel(IJournalService journalService, ILogger<HomeViewModel> logger)
    : PageViewModel(logger)
{
    [ObservableProperty]
    private ObservableCollection<RankDTO> _ranks = new();
    
    [ObservableProperty]
    private bool _isLoadingRanks;

    protected override async Task OnInitializeAsync()
    {
        logger.LogInformation(Localization.Instance["HomePage.Initializing"]);
        IsLoadingRanks = true;
        try
        {
            await LoadCommanderDataAsync();
            await LoadRankDataAsync();
            await LoadProgressDataAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["HomePage.Exceptions.FailedToInitialize"]);
        }
        finally
        {
            IsLoadingRanks = false;
        }
    }

    private async Task LoadCommanderDataAsync()
    {
        var commander = (await journalService.GetLatestJournalEntriesAsync<CommanderEvent>()).LastOrDefault();
        var title = commander is not null
            ? commander.Name
            : "Commander";
        
        WeakReferenceMessenger.Default.Send(new CommanderMessage(title));
    }

    private async Task LoadRankDataAsync()
    {
        try
        {
            var rankEvent = (await journalService.GetLatestJournalEntriesAsync<RankEvent>()).LastOrDefault();
            if (rankEvent is not null)
            {
                ProcessRank(rankEvent);
            }
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Failed to load progress data");
        }
    }
    
    private async Task LoadProgressDataAsync()
    {
        try
        {
            var progressEvent = (await journalService.GetLatestJournalEntriesAsync<ProgressEvent>()).LastOrDefault();
            if (progressEvent is not null)
            {
                ProcessProgress(progressEvent);
            }
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Failed to load progress data");
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