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
		IMemoryCache memoryCache, IServiceScopeFactory scopeFactory, IEvaluatorImportService importService)
		: base(journalLoader, stateStore, memoryCache)
	{
		_scopeFactory = scopeFactory;
		_importService = importService;

		// An import writes to the DB without touching the journal state, so StateChanged won't fire
		_importService.Imported += OnImported;
	}

	protected override void OnDispose()
	{
		_importService.Imported -= OnImported;
		base.OnDispose();
	}

	// The evaluator lives in the DB; the journal state is only the trigger
	protected override Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default) => RefreshAsync(cancellationToken);
	
	[RelayCommand]
	private void DismissImportMessage() => ImportMessage = null;

	private async void OnImported(object? sender, EvaluatorImportResult result)
	{
		try
		{
			await Dispatcher.UIThread.InvokeAsync(() =>
			{
				IsImportError = !result.IsSuccess;
				ImportMessage = !result.IsSuccess
					? $"Import failed: {result.Error}"
					: result.Added > 0
						? $"Imported {result.Added} new unsold samples from {result.Folder}"
						: $"No new unsold samples in {result.Folder}";
			});

			if (result.IsSuccess)
				await RefreshAsync();
		}
		catch (Exception)
		{
		}
	}

	private async Task RefreshAsync(CancellationToken cancellationToken = default)
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
			Items.Clear();
			foreach (var item in items)
				Items.Add(item);

			UnsoldCount = items.Length;
			UnsoldValue = total > 0 ? total.ToCompact() : "—";
			FirstFootStepCount = firstFootSteps;

			OnPropertyChanged(nameof(HasItems));
		});
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