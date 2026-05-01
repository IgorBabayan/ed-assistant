using CommunityToolkit.Mvvm.ComponentModel;
using ED.Assistant.Data.Models.Events;
using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
	protected readonly IJournalStateStore _stateStore;

	protected BaseViewModel(IJournalStateStore stateStore)
	{
		_stateStore = stateStore;
		_stateStore.StateChanged += OnStateChanged;

		UpdateFromState(_stateStore.State);
	}

	protected abstract void UpdateFromState(JournalState state);

	private void OnStateChanged(object? sender, JournalState state) => UpdateFromState(state);
}
