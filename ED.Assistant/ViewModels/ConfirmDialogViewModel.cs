using ED.Assistant.Data.Models.Events;
using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public partial class ConfirmDialogViewModel : BaseViewModel
{
	public ConfirmDialogViewModel(IJournalStateStore stateStore) : base(stateStore)
	{
	}

	protected override void UpdateFromState(JournalState state)
	{
		throw new NotImplementedException();
	}
}
