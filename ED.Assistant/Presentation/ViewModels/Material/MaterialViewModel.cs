using Avalonia.Threading;
using ED.Assistant.Presentation.Collections;

namespace ED.Assistant.Presentation.ViewModels.Material;

public partial class MaterialViewModel : LoadableViewModel
{
	// The Materials event is only written on login, so most updates can be skipped
	private volatile MaterialsEvent? _lastMaterials;

	public BulkObservableCollection<MaterialItemViewModel> Materials { get; } = [];
	public BulkObservableCollection<MaterialItemViewModel> FilteredMaterials { get; } = [];
	public BulkObservableCollection<MaterialSummaryViewModel> MaterialSummaries { get; } = [];

	public IReadOnlyList<string> Categories { get; } =
	[
		Options.Category.All,
		Options.Category.Raw,
		Options.Category.Manufactured,
		Options.Category.Encoded
	];

	public IReadOnlyList<string> SortOptions { get; } =
	[
		Options.Sort.ByName,
		Options.Sort.ByCategory,
		Options.Sort.ByCount
	];

	[ObservableProperty]
	public partial string SearchText { get; set; } = string.Empty;

	[ObservableProperty]
	public partial string SelectedCategory { get; set; } = Options.Category.All;

	[ObservableProperty]
	public partial string SelectedSort { get; set; } = Options.Sort.ByName;

	private static class Options
	{
		internal static class Category
		{
			internal const string All = "All";
			internal const string Raw = "Raw";
			internal const string Manufactured = "Manufactured";
			internal const string Encoded = "Encoded";
		}

		internal static class Sort
		{
			internal const string ByName = "Name";
			internal const string ByCategory = "Category";
			internal const string ByCount = "Count";
		}
	}

	public MaterialViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		var materialsEvent = state.Materials;
		if (ReferenceEquals(materialsEvent, _lastMaterials))
			return;

		cancellationToken.ThrowIfCancellationRequested();

		// Cached item view models are bound and raise PropertyChanged when updated,
		// so they are created/updated on the UI thread (a few hundred items at most)
		await Dispatcher.UIThread.InvokeAsync(() =>
		{
			var materials = new List<MaterialItemViewModel>();

			AddMaterials(materials, materialsEvent?.Raw, Options.Category.Raw);
			AddMaterials(materials, materialsEvent?.Manufactured, Options.Category.Manufactured);
			AddMaterials(materials, materialsEvent?.Encoded, Options.Category.Encoded);

			Materials.ReplaceAll(materials.OrderBy(x => x.Name));

			BuildSummaries();
			ApplyFilters();

			_lastMaterials = materialsEvent;
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
			cacheKey: $"material:{category}:{material.Name}",
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
			query = query.Where(x => x.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

		if (SelectedCategory != Options.Category.All)
			query = query.Where(x => x.Category == SelectedCategory);

		query = SelectedSort switch
		{
			Options.Sort.ByCategory => query.OrderBy(x => x.Category).ThenBy(x => x.Name),
			Options.Sort.ByCount => query.OrderByDescending(x => x.Count),
			_ => query.OrderBy(x => x.Name)
		};

		FilteredMaterials.ReplaceAll(query);
	}

	private void BuildSummaries()
	{
		MaterialSummaries.ReplaceAll(
		[
			new MaterialSummaryViewModel
			{
				Title = Options.Category.Raw,
				Value = Materials.Where(x => x.Category == Options.Category.Raw).Sum(x => x.Count)
			},
			new MaterialSummaryViewModel
			{
				Title = Options.Category.Manufactured,
				Value = Materials.Where(x => x.Category == Options.Category.Manufactured).Sum(x => x.Count)
			},
			new MaterialSummaryViewModel
			{
				Title = Options.Category.Encoded,
				Value = Materials.Where(x => x.Category == Options.Category.Encoded).Sum(x => x.Count)
			}
		]);
	}
}

public sealed partial class MaterialItemViewModel : BaseViewModel
{
	// Instances are cached and reused between reloads, so changes must be observable
	[ObservableProperty]
	public partial string Name { get; set; } = string.Empty;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(MaxCapacity))]
	[NotifyPropertyChangedFor(nameof(StockText))]
	public partial string Category { get; set; } = string.Empty;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(StockText))]
	public partial int Count { get; set; }

	public int MaxCapacity => Category switch
	{
		"Raw" => 300,
		"Manufactured" => 250,
		"Encoded" => 250,
		_ => 300
	};

	public string StockText => $"{Count} / {MaxCapacity}";
}

public sealed class MaterialSummaryViewModel
{
	public string Title { get; init; } = string.Empty;

	public int Value { get; init; }

	public string? Subtitle { get; init; }
}