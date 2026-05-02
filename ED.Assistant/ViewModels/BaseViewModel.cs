using CommunityToolkit.Mvvm.ComponentModel;
using ED.Assistant.Data.Models.Events;
using ED.Assistant.Data.Services.Events;
using ED.Assistant.Data.Services.Path;
using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public interface INavigationAware
{
	void OnNavigatedTo();
}

public abstract class BaseViewModel : ObservableObject, INavigationAware
{
	protected readonly IJournalStateStore _stateStore;
	protected readonly ILogStorage _logStorage;
	protected readonly IPathFinder _pathFinder;

	protected BaseViewModel(ILogStorage logStorage, IPathFinder pathFinder,
		IJournalStateStore stateStore)
	{
		_logStorage = logStorage;
		_pathFinder = pathFinder;

		_stateStore = stateStore;
		_stateStore.StateChanged += OnStateChanged;
	}

	protected abstract void UpdateFromState(JournalState state);

	public void OnNavigatedTo()
	{
		if (_stateStore.CurrentState is not null)
			UpdateFromState(_stateStore.CurrentState);
	}

	private void OnStateChanged(object? sender, JournalState state) => UpdateFromState(state);
}
