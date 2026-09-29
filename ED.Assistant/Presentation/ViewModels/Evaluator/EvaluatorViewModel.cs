namespace ED.Assistant.Presentation.ViewModels.Evaluator;

public partial class EvaluatorViewModel : LoadableViewModel
{
    public EvaluatorViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore, IMemoryCache memoryCache) 
        : base(journalLoader, stateStore, memoryCache) { }
}