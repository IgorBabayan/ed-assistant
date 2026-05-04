namespace ED.Assistant.ViewModels;

public interface INavigationAware
{
	Task OnNavigatedToAsync(CancellationToken cancellationToken = default);
}

public interface ILoadableViewModel
{
	IAsyncRelayCommand LoadCommand { get; }
}

public abstract class BaseViewModel : ObservableObject, IDisposable
{
	private bool _disposed;

	protected virtual void OnDispose() { }

	public void Dispose()
	{
		if (_disposed)
			return;

		OnDispose();
		_disposed = true;
	}
}

public abstract partial class LoadableViewModel : BaseViewModel, INavigationAware
{
	protected readonly IJournalLoaderService _journalLoader;

	private readonly IJournalStateStore _stateStore;
	private readonly IMemoryCache _memoryCache;

	private bool _isActivated;

	[ObservableProperty]
	public partial bool IsActivating { get; set; }

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

	protected override void OnDispose() => _stateStore.StateChanged -= OnStateChanged;

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