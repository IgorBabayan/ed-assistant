using System.Globalization;
using Avalonia.Threading;
using ED.Assistant.Data.Repository;
using ED.Assistant.Extensions;
using Microsoft.Extensions.DependencyInjection;
using EvaluatorEntity = ED.Assistant.Data.Evaluator.Evaluator;

namespace ED.Assistant.Presentation.ViewModels.Evaluator;

public sealed partial class EvaluatorViewModel : LoadableViewModel
{
	private readonly IServiceScopeFactory _scopeFactory;

	public ObservableCollection<EvaluatorItemViewModel> Items { get; } = [];

	[ObservableProperty]
	public partial int UnsoldCount { get; set; }

	[ObservableProperty]
	public partial string UnsoldValue { get; set; } = "—";

	[ObservableProperty]
	public partial int FirstFootStepCount { get; set; }

	public bool HasItems => Items.Count > 0;

	protected override bool ActivateOnNavigation => true;

	public EvaluatorViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache, IServiceScopeFactory scopeFactory)
		: base(journalLoader, stateStore, memoryCache) => _scopeFactory = scopeFactory;

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
			Items.Clear();
			foreach (var item in items)
				Items.Add(item);

			UnsoldCount = items.Length;
			UnsoldValue = total > 0 ? total.ToMillions() : "—";
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