using ED.Assistant.Services.Journal;

namespace ED.Assistant.ViewModels;

public interface INavigationAware
{
	Task OnNavigatedToAsync(CancellationToken cancellationToken = default);
}

public interface ILoadableViewModel
{
	IAsyncRelayCommand LoadCommand { get; }
}

public abstract class BaseViewModel : ObservableObject
{
	
}

public abstract partial class LoadableViewModel : BaseViewModel, INavigationAware, IDisposable
{
	private readonly IJournalLoaderService _journalLoader;
	private readonly IJournalStateStore _stateStore;
	private readonly IMemoryCache _memoryCache;

	private bool _disposed;
	private bool _isActivated;

	[ObservableProperty]
	private bool isActivating;
	
	protected LoadableViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache)
	{
		_journalLoader = journalLoader;
		_stateStore = stateStore;
		_memoryCache = memoryCache;

		_stateStore.StateChanged += OnStateChanged;
	}

	protected virtual bool ActivateOnNavigation => false;

	protected virtual void UpdateFromState(JournalState state) { }

	protected virtual Task UpdateFromStateAsync(JournalState state, CancellationToken cancellationToken = default)
	{
		UpdateFromState(state);
		return Task.CompletedTask;
	}

	protected async Task ActivateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		if (IsActivating)
			return;

		try
		{
			IsActivating = true;
			await UpdateFromStateAsync(state, cancellationToken);
		}
		finally
		{
			IsActivating = false;
		}
	}

	protected TViewModel GetOrCreateCachedViewModel<TModel, TViewModel>(string cacheKey, TModel model,
		Func<TModel, TViewModel> create, Action<TViewModel, TModel>? update = null)
		where TViewModel : class
	{
		if (_memoryCache.TryGetValue(cacheKey, out TViewModel? viewModel) &&
			viewModel is not null)
		{
			update?.Invoke(viewModel, model);
			return viewModel;
		}

		viewModel = create(model);

		_memoryCache.Set(
			cacheKey,
			viewModel,
			new MemoryCacheEntryOptions
			{
				SlidingExpiration = TimeSpan.FromMinutes(30)
			});

		return viewModel;
	}

	public async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
	{
		var state = _stateStore.CurrentState;

		if (state is null)
			return;

		if (!ActivateOnNavigation)
		{
			await UpdateFromStateAsync(state, cancellationToken);
			return;
		}

		if (_isActivated)
			return;

		_isActivated = true;
		await ActivateAsync(state, cancellationToken);
	}

	public void Dispose()
	{
		if (_disposed)
			return;

		_stateStore.StateChanged -= OnStateChanged;
		_disposed = true;
	}

	private async void OnStateChanged(object? sender, JournalState state)
	{
		if (ActivateOnNavigation)
			await ActivateAsync(state);
		else
			await UpdateFromStateAsync(state);
	}

	[RelayCommand]
	private async Task Load(CancellationToken cancellationToken = default) 
		=> await _journalLoader.LoadLastLogsAsync(cancellationToken);
}