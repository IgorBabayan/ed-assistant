using Avalonia.Threading;

namespace ED.Assistant.Presentation.ViewModels.Journal;

public sealed partial class JournalViewModel : LoadableViewModel
{
	private static readonly TimeSpan SearchDelay = TimeSpan.FromMilliseconds(250);

	private readonly SemaphoreSlim _updateGate = new(1, 1);

	// UI thread only
	private readonly List<JournalEntryViewModel> _allEntries = [];
	private SearchFilter _filter = SearchFilter.Empty;
	private CancellationTokenSource? _searchCts;

	// _updateGate only
	private JournalLog? _currentLog;
	private long _lastSequence;

	public JournalViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

	/// <summary>Raised when the view should jump to the newest line.</summary>
	public event EventHandler? ScrollToEndRequested;

	/// <summary>Rows currently shown (after filtering). Replaced as a whole on reload / new search.</summary>
	[ObservableProperty]
	public partial ObservableCollection<JournalEntryViewModel> Entries { get; set; } = [];

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(HasSearchText))]
	public partial string? SearchText { get; set; }

	[ObservableProperty]
	public partial bool IsFollowing { get; set; } = true;

	[ObservableProperty]
	public partial string? FileName { get; set; }

	public bool HasSearchText => !string.IsNullOrEmpty(SearchText);

	public bool HasEntries => Entries.Count > 0;

	public string EmptyMessage => _allEntries.Count == 0
		? "No journal lines loaded yet. Load journals or start the game with auto watch on."
		: "No lines match your search.";

	public string Summary => _filter.IsEmpty
		? $"{_allEntries.Count:N0} lines"
		: $"{Entries.Count:N0} of {_allEntries.Count:N0} lines";

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		// StateChanged can fire from the watcher thread while a navigation update is running
		await _updateGate.WaitAsync(cancellationToken);
		try
		{
			var log = state.Log;

			// A full reload creates a new JournalState (and a new log) — start over
			var isReset = !ReferenceEquals(log, _currentLog);
			var batch = log.GetSince(isReset ? 0 : _lastSequence);

			if (!isReset && batch.Count == 0)
				return;

			// Header parsing happens here, off the UI thread
			var rows = batch.Select(entry => new JournalEntryViewModel(entry)).ToArray();

			_currentLog = log;
			if (batch.Count > 0)
				_lastSequence = batch[^1].Sequence;

			await Dispatcher.UIThread.InvokeAsync(() => Apply(rows, isReset, state.FileName, log.Capacity));
		}
		finally
		{
			_updateGate.Release();
		}
	}

	[RelayCommand]
	private void ClearSearch() => SearchText = string.Empty;

	partial void OnSearchTextChanged(string? value) => _ = ApplySearchDelayedAsync(value);

	partial void OnIsFollowingChanged(bool value)
	{
		if (value)
			ScrollToEndRequested?.Invoke(this, EventArgs.Empty);
	}

	partial void OnEntriesChanged(ObservableCollection<JournalEntryViewModel> value) => RaiseCounters();

	private void Apply(IReadOnlyList<JournalEntryViewModel> rows, bool isReset, string? fileName, int capacity)
	{
		FileName = fileName;

		if (isReset)
		{
			_allEntries.Clear();
			_allEntries.AddRange(rows);
			TrimAll(capacity);

			// one Reset notification instead of thousands of Add notifications
			Entries = new ObservableCollection<JournalEntryViewModel>(_allEntries.Where(_filter.IsMatch));
		}
		else
		{
			_allEntries.AddRange(rows);

			foreach (var row in rows)
			{
				if (_filter.IsMatch(row))
					Entries.Add(row);
			}

			TrimAll(capacity);
		}

		RaiseCounters();

		if (IsFollowing)
			ScrollToEndRequested?.Invoke(this, EventArgs.Empty);
	}

	private void TrimAll(int capacity)
	{
		var overflow = _allEntries.Count - capacity;
		if (overflow <= 0)
			return;

		var lastRemoved = _allEntries[overflow - 1].Sequence;
		_allEntries.RemoveRange(0, overflow);

		// Entries keep file order, so dropped rows are always at the front
		while (Entries.Count > 0 && Entries[0].Sequence <= lastRemoved)
			Entries.RemoveAt(0);
	}

	private async Task ApplySearchDelayedAsync(string? text)
	{
		_searchCts?.Cancel();
		_searchCts?.Dispose();

		var cts = _searchCts = new CancellationTokenSource();

		try
		{
			await Task.Delay(SearchDelay, cts.Token);
		}
		catch (OperationCanceledException)
		{
			return;
		}

		_filter = SearchFilter.Parse(text);
		Entries = new ObservableCollection<JournalEntryViewModel>(_allEntries.Where(_filter.IsMatch));

		if (IsFollowing)
			ScrollToEndRequested?.Invoke(this, EventArgs.Empty);
	}

	private void RaiseCounters()
	{
		OnPropertyChanged(nameof(HasEntries));
		OnPropertyChanged(nameof(Summary));
		OnPropertyChanged(nameof(EmptyMessage));
	}

	protected override void OnDispose()
	{
		_searchCts?.Cancel();
		_searchCts?.Dispose();
		_updateGate.Dispose();

		base.OnDispose();
	}

	/// <summary>
	/// Space-separated terms, all must appear in the raw line (case-insensitive).
	/// "Scan Earthlike" → lines containing both "Scan" and "Earthlike".
	/// </summary>
	private sealed class SearchFilter
	{
		public static readonly SearchFilter Empty = new([]);

		private readonly string[] _terms;

		private SearchFilter(string[] terms) => _terms = terms;

		public bool IsEmpty => _terms.Length == 0;

		public static SearchFilter Parse(string? text) => string.IsNullOrWhiteSpace(text)
			? Empty
			: new SearchFilter(text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

		public bool IsMatch(JournalEntryViewModel entry)
		{
			foreach (var term in _terms)
			{
				if (!entry.RawLine.Contains(term, StringComparison.OrdinalIgnoreCase))
					return false;
			}

			return true;
		}
	}
}