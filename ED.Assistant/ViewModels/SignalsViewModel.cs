using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public partial class SignalsViewModel : LoadableViewModel
{
	public SignalsViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore)
		: base(journalLoader, stateStore) { }

	protected override void UpdateFromState(JournalState state)
	{
	}
}
