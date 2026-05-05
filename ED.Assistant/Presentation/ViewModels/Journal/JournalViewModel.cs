namespace ED.Assistant.Presentation.ViewModels.Journal;

public partial class JournalViewModel : LoadableViewModel
{
	public JournalViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
	}
}
