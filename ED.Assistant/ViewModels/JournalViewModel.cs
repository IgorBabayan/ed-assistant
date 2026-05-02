using ED.Assistant.Data.Models.Events;
using ED.Assistant.Data.Services.Events;
using ED.Assistant.Data.Services.Path;
using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public partial class JournalViewModel : BaseViewModel, ILoadableViewModel
{
	public JournalViewModel(ILogStorage logStorage, IPathFinder pathFinder, IJournalStateStore stateStore)
		: base(logStorage, pathFinder, stateStore) { }

	public IAsyncRelayCommand LoadCommand => throw new NotImplementedException();

	protected override void UpdateFromState(JournalState state)
	{
	}
}
