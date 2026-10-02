using System.Diagnostics;
using System.Globalization;
using Avalonia.Threading;
using ED.Assistant.Application.Evaluation;
using ED.Assistant.Data.Repository;
using ED.Assistant.Extensions;
using Microsoft.Extensions.DependencyInjection;
using EvaluatorEntity = ED.Assistant.Data.Evaluator.Evaluator;

namespace ED.Assistant.Presentation.ViewModels.Evaluator;

public sealed partial class EvaluatorViewModel : LoadableViewModel
{
	private readonly IServiceScopeFactory _scopeFactory;
	private readonly IEvaluatorImportService _importService;
	private readonly IEvaluatorSyncService _syncService;

	// Only the newest samples are loaded up front; older ones are fetched as the list scrolls
	private const int PageSize = 100;

	// RefreshAsync is serialized by LoadableViewModel, but LoadMore comes from the view: this keeps them apart
	private readonly SemaphoreSlim _pageGate = new(1, 1);

	// Paging state, only touched while holding _pageGate.
	// Keyset cursor = last loaded row in (DateCreation desc, Id desc) order: unlike Skip(),
	// it doesn't shift or duplicate rows when new samples arrive between pages.
	private (DateTime DateCreation, int Id)? _cursor;
	private int _loadedCount;
	private volatile bool _hasMore;

	public ObservableCollection<EvaluatorItemViewModel> Items { get; } = [];

	[ObservableProperty]
	public partial int UnsoldCount { get; set; }

	[ObservableProperty]
	public partial string UnsoldValue { get; set; } = "—";

	[ObservableProperty]
	public partial int FirstFootStepCount { get; set; }
	
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(HasImportMessage))]
	public partial string? ImportMessage { get; set; }

	[ObservableProperty]
	public partial bool IsImportError { get; set; }

	public bool HasImportMessage => ImportMessage is not null;

	public bool HasItems => Items.Count > 0;

	protected override bool ReactsToJournalChanges => false;

	public EvaluatorViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache, IServiceScopeFactory scopeFactory, IEvaluatorImportService importService,
		IEvaluatorSyncService syncService)
		: base(journalLoader, stateStore, memoryCache)
	{
		_scopeFactory = scopeFactory;
		_importService = importService;
		_syncService = syncService;
		_syncService.DataChanged += OnDataChanged;

		// An import writes to the DB without touching the journal state, so StateChanged won't fire
		_importService.Imported += OnImported;
	}

	protected override void OnDispose()
	{
		_importService.Imported -= OnImported;
		_syncService.DataChanged -= OnDataChanged;
		_pageGate.Dispose();
		base.OnDispose();
	}

	// The evaluator lives in the DB; the journal state is only the trigger
	protected override Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default) => RefreshAsync(cancellationToken);
	
	[RelayCommand]
	private void DismissImportMessage() => ImportMessage = null;

	private void OnDataChanged(object? sender, EventArgs e) => Invalidate();

	private void OnImported(object? sender, EvaluatorImportResult result)
	{
		RunOnUIThread(() =>
		{
			IsImportError = !result.IsSuccess;
			ImportMessage = !result.IsSuccess
				? $"Import failed: {result.Error}"
				: result.Added > 0
					? $"Imported {result.Added} new unsold samples from {result.Folder}"
					: $"No new unsold samples in {result.Folder}";
		});

		if (result.IsSuccess)
			Invalidate();
	}

	private async Task RefreshAsync(CancellationToken cancellationToken = default)
	{
		await _pageGate.WaitAsync(cancellationToken);
		try
		{
			// Re-read as many rows as are already shown, so a new sample doesn't snap the list back to page 1
			var take = Math.Max(PageSize, _loadedCount);

			// Runs on the UI thread when navigating here, and Microsoft.Data.Sqlite executes
			// "async" queries synchronously: keep the DB work off the UI thread
			var (totals, firstFootSteps, rows) = await Task.Run(async () =>
			{
				using var scope = _scopeFactory.CreateScope();
				var repository = scope.ServiceProvider.GetRequiredService<IRepository<EvaluatorEntity>>();

				// The IsActive query filter already hides sold rows
				var active = repository.AsNoTracking();

				// SQLite can't SUM decimals server-side: fetch only the Total column and add it up in memory
				var allTotals = await active.Select(x => x.Total).ToListAsync(cancellationToken);
				var footSteps = await active.CountAsync(x => x.HasFirstFootStep, cancellationToken);
				var page = await NextPage(active, null, take + 1).ToListAsync(cancellationToken);

				return (allTotals, footSteps, page);
			}, cancellationToken);

			var hasMore = TrimPage(rows, take);
			var items = rows.Select(EvaluatorItemViewModel.From).ToArray();
			var total = totals.Sum();

			await Dispatcher.UIThread.InvokeAsync(() =>
			{
				Items.Clear();
				foreach (var item in items)
					Items.Add(item);

				UnsoldCount = totals.Count;
				UnsoldValue = total > 0 ? total.ToCompact() : "—";
				FirstFootStepCount = firstFootSteps;

				OnPropertyChanged(nameof(HasItems));
			});

			_loadedCount = items.Length;
			_cursor = rows.Count > 0 ? (rows[^1].DateCreation, rows[^1].Id) : null;
			_hasMore = hasMore;
		}
		finally
		{
			_pageGate.Release();
		}
	}

	/// <summary>Appends the next page of older samples. Called by the view when scrolled near the bottom.</summary>
	[RelayCommand]
	private async Task LoadMoreAsync()
	{
		if (!_hasMore)
			return;

		await _pageGate.WaitAsync();
		try
		{
			// A refresh may have run while we waited
			if (!_hasMore || _cursor is not { } cursor)
				return;

			var rows = await Task.Run(async () =>
			{
				using var scope = _scopeFactory.CreateScope();
				var repository = scope.ServiceProvider.GetRequiredService<IRepository<EvaluatorEntity>>();
				return await NextPage(repository.AsNoTracking(), cursor, PageSize + 1).ToListAsync();
			});

			var hasMore = TrimPage(rows, PageSize);
			var items = rows.Select(EvaluatorItemViewModel.From).ToArray();

			await Dispatcher.UIThread.InvokeAsync(() =>
			{
				foreach (var item in items)
					Items.Add(item);
			});

			_loadedCount += items.Length;
			if (rows.Count > 0)
				_cursor = (rows[^1].DateCreation, rows[^1].Id);
			_hasMore = hasMore;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"{nameof(EvaluatorViewModel)} load more failed: {ex}");
		}
		finally
		{
			_pageGate.Release();
		}
	}

	// Newest first; Id breaks ties between samples recorded at the same moment
	private static IQueryable<EvaluatorEntity> NextPage(IQueryable<EvaluatorEntity> active,
		(DateTime DateCreation, int Id)? after, int take)
	{
		if (after is { } c)
			active = active.Where(x => x.DateCreation < c.DateCreation
			                           || (x.DateCreation == c.DateCreation && x.Id < c.Id));

		return active
			.Include(x => x.Genus)
			.OrderByDescending(x => x.DateCreation)
			.ThenByDescending(x => x.Id)
			.Take(take);
	}

	// Pages are queried with one extra row to know whether more exist without a COUNT
	private static bool TrimPage(List<EvaluatorEntity> rows, int pageSize)
	{
		if (rows.Count <= pageSize)
			return false;

		rows.RemoveRange(pageSize, rows.Count - pageSize);
		return true;
	}
}

public sealed class EvaluatorItemViewModel
{
	public string Name { get; init; } = string.Empty;
	public string Sampled { get; init; } = string.Empty;
	public string BaseValue { get; init; } = "—";
	public string Multiplier { get; init; } = "×1";
	public string Total { get; init; } = "—";
	public bool HasFirstFootStep { get; init; }

	public static EvaluatorItemViewModel From(EvaluatorEntity entity) => new()
	{
		Name = entity.Genus.Name,
		// SQLite returns Kind=Unspecified; journal timestamps are UTC
		Sampled = DateTime.SpecifyKind(entity.DateCreation, DateTimeKind.Utc)
			.ToLocalTime()
			.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
		BaseValue = entity.Genus.Value.ToMillions(),
		Multiplier = entity.HasFirstFootStep ? "×5" : "×1",
		Total = entity.Total.ToMillions(),
		HasFirstFootStep = entity.HasFirstFootStep
	};
}