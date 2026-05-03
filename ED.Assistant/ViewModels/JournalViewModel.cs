using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public partial class JournalViewModel : LoadableViewModel
{
	public JournalViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore)
		: base(journalLoader, stateStore) { }

	protected override void UpdateFromState(JournalState state)
	{
	}
}
