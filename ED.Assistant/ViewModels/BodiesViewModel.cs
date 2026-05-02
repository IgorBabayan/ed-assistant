using ED.Assistant.Data.Models.Events;
using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public partial class BodiesViewModel : BaseViewModel, ILoadableViewModel
{
	public BodiesViewModel(IJournalStateStore stateStore) : base(stateStore)
	{
	}

	public IAsyncRelayCommand LoadCommand => throw new NotImplementedException();

	protected override void UpdateFromState(JournalState state)
	{
	}
}
