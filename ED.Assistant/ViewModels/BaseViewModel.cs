using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public interface INavigationAware
{
	void OnNavigatedTo();
}

public abstract class BaseViewModel : ObservableObject
{
	
}

public abstract partial class LoadableViewModel : BaseViewModel, INavigationAware, IDisposable
{
	private readonly IJournalLoaderService _journalLoader;
	private readonly IJournalStateStore _stateStore;

	private bool _disposed;

	protected LoadableViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore)
	{
		_journalLoader = journalLoader;

		_stateStore = stateStore;
		_stateStore.StateChanged += OnStateChanged;
	}

	protected abstract void UpdateFromState(JournalState state);

	public void OnNavigatedTo()
	{
		if (_stateStore.CurrentState is not null)
			UpdateFromState(_stateStore.CurrentState);
	}

	public void Dispose()
	{
		if (_disposed)
			return;

		_stateStore.StateChanged -= OnStateChanged;
		_disposed = true;
	}

	private void OnStateChanged(object? sender, JournalState state) => UpdateFromState(state);

	[RelayCommand]
	private Task Load(CancellationToken cancellationToken = default)
		=> _journalLoader.LoadLastLogsAsync(cancellationToken);
}