using ED.Assistant.Data.Repository;

namespace ED.Assistant.Presentation.ViewModels.Evaluator;

public partial class EvaluatorViewModel : LoadableViewModel
{
    private readonly IRepository<Data.Evaluator.Evaluator> _evaluatorRepository;
    
    public EvaluatorViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore, IMemoryCache memoryCache,
        IRepository<Data.Evaluator.Evaluator> evaluatorRepository) : base(journalLoader, stateStore, memoryCache)
        => _evaluatorRepository = evaluatorRepository;

    protected override bool ActivateOnNavigation => true;

    protected override async Task UpdateFromStateAsync(JournalState state, CancellationToken cancellationToken = default)
    {
        await base.UpdateFromStateAsync(state, cancellationToken);
    }
}