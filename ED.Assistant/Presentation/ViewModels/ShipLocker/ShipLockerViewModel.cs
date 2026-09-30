using Avalonia.Threading;
using ED.Assistant.Presentation.Collections;
using ED.Assistant.Presentation.ViewModels.Material;

namespace ED.Assistant.Presentation.ViewModels.ShipLocker;

public partial class ShipLockerViewModel : LoadableViewModel
{
	// Replaced only when a new ShipLocker line arrives; skip rebuilds for every other line
	private volatile ShipLockerEvent? _lastShipLocker;

	public BulkObservableCollection<MaterialItemViewModel> Materials { get; } = [];
	public BulkObservableCollection<MaterialItemViewModel> FilteredMaterials { get; } = [];
	public BulkObservableCollection<MaterialSummaryViewModel> MaterialSummaries { get; } = [];

	public IReadOnlyList<string> Categories { get; } =
	[
		Options.Categories.All,
		Options.Categories.Items,
		Options.Categories.Components,
		Options.Categories.Consumables,
		Options.Categories.Data
	];

	public IReadOnlyList<string> SortOptions { get; } =
	[
		Options.Sort.Name,
		Options.Sort.Category,
		Options.Sort.Count
	];

	[ObservableProperty]
	public partial string SearchText { get; set; } = string.Empty;

	[ObservableProperty]
	public partial string SelectedCategory { get; set; } = Options.Categories.All;

	[ObservableProperty]
	public partial string SelectedSort { get; set; } = Options.Sort.Name;

	private static class Options
	{
		internal static class Categories
		{
			internal const string All = "All";
			internal const string Items = "Items";
			internal const string Components = "Components";
			internal const string Consumables = "Consumables";
			internal const string Data = "Data";
		}

		internal static class Sort
		{
			internal const string Name = "Name";
			internal const string Category = "Category";
			internal const string Count = "Count";
		}
	}

	public ShipLockerViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		var shipLocker = state.ShipLocker;
		if (ReferenceEquals(shipLocker, _lastShipLocker))
			return;

		cancellationToken.ThrowIfCancellationRequested();

		// Cached item view models are bound and raise PropertyChanged when updated,
		// so they are created/updated on the UI thread
		await Dispatcher.UIThread.InvokeAsync(() =>
		{
			var materials = new List<MaterialItemViewModel>();

			AddMaterials(materials, shipLocker?.Items, Options.Categories.Items);
			AddMaterials(materials, shipLocker?.Components, Options.Categories.Components);
			AddMaterials(materials, shipLocker?.Consumables, Options.Categories.Consumables);
			AddMaterials(materials, shipLocker?.Data, Options.Categories.Data);

			Materials.ReplaceAll(materials.OrderBy(x => x.Name));

			BuildSummaries();
			ApplyFilters();

			_lastShipLocker = shipLocker;
		});
	}

	partial void OnSearchTextChanged(string value) => ApplyFilters();

	partial void OnSelectedCategoryChanged(string value) => ApplyFilters();

	partial void OnSelectedSortChanged(string value) => ApplyFilters();

	private void AddMaterials(List<MaterialItemViewModel> target, IEnumerable<MaterialItem>? source,
		string category)
	{
		if (source is null)
			return;

		target.AddRange(source.Select(material => GetOrCreateCachedViewModel(
			cacheKey: $"ship-locker:{category}:{material.Name}",
			model: material,
			create: x => new MaterialItemViewModel
			{
				Name = x.FullName,
				Category = category,
				Count = x.Count
			},
			update: (vm, x) =>
			{
				vm.Name = x.FullName;
				vm.Category = category;
				vm.Count = x.Count;
			})));
	}

	private void ApplyFilters()
	{
		var query = Materials.AsEnumerable();

		if (!string.IsNullOrWhiteSpace(SearchText))
		{
			query = query.Where(x =>
				x.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
		}

		if (SelectedCategory != Options.Categories.All)
			query = query.Where(x => x.Category == SelectedCategory);

		query = SelectedSort switch
		{
			Options.Sort.Category => query.OrderBy(x => x.Category).ThenBy(x => x.Name),
			Options.Sort.Count => query.OrderByDescending(x => x.Count),
			_ => query.OrderBy(x => x.Name)
		};

		FilteredMaterials.ReplaceAll(query);
	}

	private void BuildSummaries()
	{
		var summaries = new List<MaterialSummaryViewModel>
		{
			CreateSummary(Options.Categories.Items),
			CreateSummary(Options.Categories.Components),
			CreateSummary(Options.Categories.Consumables),
			CreateSummary(Options.Categories.Data),
			new()
			{
				Title = "Low stock",
				Value = Materials.Count(x => x.MaxCapacity > 0 && x.Count < x.MaxCapacity * 0.25),
				Subtitle = "< 25%"
			}
		};

		MaterialSummaries.ReplaceAll(summaries);
	}

	private MaterialSummaryViewModel CreateSummary(string category) => new()
	{
		Title = category,
		Value = Materials
				.Where(x => x.Category == category)
				.Sum(x => x.Count)
	};
}