using Avalonia.Threading;
using ED.Assistant.Presentation.Collections;

namespace ED.Assistant.Presentation.ViewModels.Material;

public partial class MaterialViewModel : LoadableViewModel
{
	// The Materials event is only written on login, so most updates can be skipped
	private volatile MaterialsEvent? _lastMaterials;

	public BulkObservableCollection<MaterialItemViewModel> Materials { get; } = new();
	public BulkObservableCollection<MaterialItemViewModel> FilteredMaterials { get; } = new();
	public BulkObservableCollection<MaterialSummaryViewModel> MaterialSummaries { get; } = new();

	public IReadOnlyList<string> Categories { get; } =
	[
		Options.Category.All,
		Options.Category.Raw,
		Options.Category.Manufactured,
		Options.Category.Encoded
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
	public partial string SelectedCategory { get; set; } = Options.Category.All;

	[ObservableProperty]
	public partial string SelectedSort { get; set; } = Options.Sort.Name;

	private class Options
	{
		internal class Category
		{
			internal const string All = "All";
			internal const string Raw = "Raw";
			internal const string Manufactured = "Manufactured";
			internal const string Encoded = "Encoded";
		}

		internal class  Sort
		{
			internal const string Name = "Name";
			internal const string Category = "Category";
			internal const string Count = "Count";
		}
	}

	public MaterialViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		var materialsEvent = state.Materials;
		if (materialsEvent is null || ReferenceEquals(materialsEvent, _lastMaterials))
			return;

		var materials = await Task.Run(() =>
		{
			var result = new List<MaterialItemViewModel>();

			AddMaterials(result, materialsEvent.Raw, Options.Category.Raw);
			AddMaterials(result, materialsEvent.Manufactured, Options.Category.Manufactured);
			AddMaterials(result, materialsEvent.Encoded, Options.Category.Encoded);

			return result.OrderBy(x => x.Name).ToList();
		}, cancellationToken);

		// Bound collections: change them on the UI thread only
		await Dispatcher.UIThread.InvokeAsync(() =>
		{
			Materials.ReplaceAll(materials);

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

		foreach (var material in source)
		{
			var viewModel = GetOrCreateCachedViewModel(
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
				});

			target.Add(viewModel);
		}
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
			Options.Sort.Category => query.OrderBy(x => x.Category).ThenBy(x => x.Name),
			Options.Sort.Count => query.OrderByDescending(x => x.Count),
			_ => query.OrderBy(x => x.Name)
		};

		FilteredMaterials.ReplaceAll(query.ToList());
	}

	private void BuildSummaries()
	{
		MaterialSummaries.ReplaceAll(
		[
			new()
			{
				Title = Options.Category.Raw,
				Value = Materials.Where(x => x.Category == Options.Category.Raw).Sum(x => x.Count)
			},
			new()
			{
				Title = Options.Category.Manufactured,
				Value = Materials.Where(x => x.Category == Options.Category.Manufactured).Sum(x => x.Count)
			},
			new()
			{
				Title = Options.Category.Encoded,
				Value = Materials.Where(x => x.Category == Options.Category.Encoded).Sum(x => x.Count)
			}
		]);
	}
}

public sealed partial class MaterialItemViewModel : BaseViewModel
{
	public string Name { get; set; } = string.Empty;

	public string Category { get; set; } = string.Empty;

	public int Count { get; set; }

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