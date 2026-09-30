using System.Diagnostics;
using Avalonia.Threading;

namespace ED.Assistant.Presentation.ViewModels;

public interface INavigationAware
{
	Task OnNavigatedToAsync(CancellationToken cancellationToken = default);

	void OnNavigatedFrom();
}

public abstract class BaseViewModel : ObservableObject, IDisposable
{
	private bool _disposed;

	protected virtual void OnDispose() { }

	public void Dispose()
	{
		if (_disposed)
			return;

		_disposed = true;
		OnDispose();
		GC.SuppressFinalize(this);
	}
}

/// <summary>
/// Base for view models fed by <see cref="IJournalStateStore"/>.
/// <para>
/// Only the view model on screen is updated when the journal state changes; the others
/// just remember that they're stale and refresh once when they're navigated to.
/// Updates are coalesced: while one runs, only the newest pending state is kept.
/// </para>
/// </summary>
public abstract partial class LoadableViewModel : BaseViewModel, INavigationAware
{
	protected readonly IJournalLoaderService _journalLoader;

	private readonly IJournalStateStore _stateStore;
	private readonly IMemoryCache _memoryCache;

	private readonly object _activationLock = new();
	private JournalState? _pendingState;
	private bool _isRunning;

	private volatile bool _isCurrent;
	private volatile bool _isDirty = true;

	/// <summary>True while an update is running. Always changed on the UI thread.</summary>
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

	/// <summary>
	/// View models that are always on screen (the shell) and never navigated to
	/// override this to keep updating on every state change.
	/// </summary>
	protected virtual bool IsAlwaysVisible => false;

	/// <summary>
	/// View models whose data doesn't come from the journal state (e.g. the database)
	/// return false and call <see cref="Invalidate"/> when their own source changes.
	/// </summary>
	protected virtual bool ReactsToJournalChanges => true;

	/// <summary>True while this view model is the navigation target.</summary>
	protected bool IsCurrent => _isCurrent;

	protected virtual void UpdateFromState(JournalState state) { }

	/// <summary>
	/// Called on the thread that raised the change (usually the journal watcher),
	/// or on the UI thread during navigation. Bound properties/collections must be
	/// changed on the UI thread.
	/// </summary>
	protected virtual Task UpdateFromStateAsync(JournalState state, CancellationToken cancellationToken = default)
	{
		UpdateFromState(state);
		return Task.CompletedTask;
	}

	public async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
	{
		_isCurrent = true;

		if (!_isDirty)
			return;

		_isDirty = false;

		var state = _stateStore.CurrentState;
		if (state is null)
			return;

		try
		{
			await ActivateAsync(state, cancellationToken);
		}
		catch
		{
			// Try again on the next navigation
			_isDirty = true;
			throw;
		}
	}

	public void OnNavigatedFrom() => _isCurrent = false;

	/// <summary>Refreshes now if visible, otherwise on the next navigation.</summary>
	protected void Invalidate() => RequestUpdate(_stateStore.CurrentState);

	protected async Task ActivateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		lock (_activationLock)
		{
			if (_isRunning)
			{
				// Coalesce: only the newest state matters
				_pendingState = state;
				return;
			}

			_isRunning = true;
			SetActivating(true);
		}

		var currentState = state;

		try
		{
			while (true)
			{
				await UpdateFromStateAsync(currentState, cancellationToken);

				lock (_activationLock)
				{
					if (_pendingState is null)
					{
						_isRunning = false;
						SetActivating(false);
						return;
					}

					currentState = _pendingState;
					_pendingState = null;
				}
			}
		}
		catch
		{
			lock (_activationLock)
			{
				_pendingState = null;
				_isRunning = false;
				SetActivating(false);
			}

			throw;
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

	/// <summary>Runs <paramref name="action"/> on the UI thread (inline if already there).</summary>
	protected static void RunOnUIThread(Action action)
	{
		if (Dispatcher.UIThread.CheckAccess())
			action();
		else
			Dispatcher.UIThread.Post(action);
	}

	private void OnStateChanged(object? sender, JournalState state)
	{
		if (ReactsToJournalChanges)
			RequestUpdate(state);
	}

	// async void is required for event-style fire-and-forget; it must never throw
	private async void RequestUpdate(JournalState? state)
	{
		if (state is null)
			return;

		if (!_isCurrent && !IsAlwaysVisible)
		{
			_isDirty = true;
			return;
		}

		try
		{
			await ActivateAsync(state);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex)
		{
			_isDirty = true;
			Debug.WriteLine($"{GetType().Name} update failed: {ex}");
		}
	}

	// Called inside _activationLock so true/false posts can't be reordered
	private void SetActivating(bool value) => RunOnUIThread(() => IsActivating = value);

	[RelayCommand]
	private async Task Load(CancellationToken cancellationToken = default) 
		=> await _journalLoader.LoadLastLogsAsync(cancellationToken);
}