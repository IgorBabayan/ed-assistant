using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public partial class SignalsViewModel : LoadableViewModel
{
	public SignalsViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
	}
}
