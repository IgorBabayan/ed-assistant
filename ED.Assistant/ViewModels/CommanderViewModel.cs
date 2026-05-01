using ED.Assistant.Data.Models.Events;
using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public partial class CommanderViewModel : BaseViewModel
{
	public CommanderViewModel(IJournalStateStore stateStore) : base(stateStore)
	{
	}

	protected override void UpdateFromState(JournalState state)
	{
		throw new NotImplementedException();
	}
}
