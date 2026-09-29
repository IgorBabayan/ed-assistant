using System.Globalization;
using Avalonia.Threading;
using ED.Assistant.Application.Evaluation;
using ED.Assistant.Data.Repository;
using ED.Assistant.Extensions;
using ED.Assistant.Presentation.Collections;
using Microsoft.Extensions.DependencyInjection;
using EvaluatorEntity = ED.Assistant.Data.Evaluator.Evaluator;

namespace ED.Assistant.Presentation.ViewModels.Evaluator;

public sealed partial class EvaluatorViewModel : LoadableViewModel
{
	private readonly IServiceScopeFactory _scopeFactory;
	private readonly IEvaluatorSyncService _evaluatorSync;

	public BulkObservableCollection<EvaluatorItemViewModel> Items { get; } = new();

	[ObservableProperty]
	public partial int UnsoldCount { get; set; }

	[ObservableProperty]
	public partial string UnsoldValue { get; set; } = "—";

	[ObservableProperty]
	public partial int FirstFootStepCount { get; set; }

	public bool HasItems => Items.Count > 0;

	// The data lives in the database and only changes when the sync service writes to it,
	// so ordinary journal lines must not trigger a reload.
	protected override bool ReactsToJournalChanges => false;

	public EvaluatorViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache, IServiceScopeFactory scopeFactory, IEvaluatorSyncService evaluatorSync)
		: base(journalLoader, stateStore, memoryCache)
	{
		_scopeFactory = scopeFactory;
		_evaluatorSync = evaluatorSync;

		_evaluatorSync.DataChanged += OnEvaluatorDataChanged;
	}

	protected override void OnDispose()
	{
		_evaluatorSync.DataChanged -= OnEvaluatorDataChanged;
		base.OnDispose();
	}

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		List<EvaluatorEntity> rows;

		using (var scope = _scopeFactory.CreateScope())
		{
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<EvaluatorEntity>>();

			// The IsActive query filter already hides sold rows
			rows = await repository.AsNoTracking()
				.Include(x => x.Genus)
				.OrderByDescending(x => x.DateCreation)
				.ToListAsync(cancellationToken);
		}

		// SQLite can't SUM decimals server-side, so aggregate in memory
		var items = rows.Select(EvaluatorItemViewModel.From).ToArray();
		var total = rows.Sum(x => x.Total);
		var firstFootSteps = rows.Count(x => x.HasFirstFootStep);

		await Dispatcher.UIThread.InvokeAsync(() =>
		{
			Items.ReplaceAll(items);

			UnsoldCount = items.Length;
			UnsoldValue = total > 0 ? total.ToMillions() : "—";
			FirstFootStepCount = firstFootSteps;

			OnPropertyChanged(nameof(HasItems));
		});
	}

	// Reload now if visible, otherwise on the next visit
	private void OnEvaluatorDataChanged(object? sender, EventArgs e) => Invalidate();
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