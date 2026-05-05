namespace ED.Assistant.Presentation.ViewModels.Material;

public partial class MaterialViewModel : LoadableViewModel
{
	public ObservableCollection<MaterialItemViewModel> Materials { get; } = [];
	public ObservableCollection<MaterialItemViewModel> FilteredMaterials { get; } = [];
	public ObservableCollection<MaterialSummaryViewModel> MaterialSummaries { get; } = [];

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

	protected override bool ActivateOnNavigation => true;

	public MaterialViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		if (state.Materials is null)
			return;

		var materials = await Task.Run(() =>
		{
			var result = new List<MaterialItemViewModel>();

			AddMaterials(result, state.Materials.Raw, Options.Category.Raw);
			AddMaterials(result, state.Materials.Manufactured, Options.Category.Manufactured);
			AddMaterials(result, state.Materials.Encoded, Options.Category.Encoded);

			return result.OrderBy(x => x.Name).ToList();
		}, cancellationToken);

		Materials.Clear();

		foreach (var material in materials)
			Materials.Add(material);

		BuildSummaries();
		ApplyFilters();
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

		if (SelectedCategory != "All")
			query = query.Where(x => x.Category == SelectedCategory);

		query = SelectedSort switch
		{
			"Category" => query.OrderBy(x => x.Category).ThenBy(x => x.Name),
			"Count" => query.OrderByDescending(x => x.Count),
			_ => query.OrderBy(x => x.Name)
		};

		FilteredMaterials.Clear();
		foreach (var material in query)
			FilteredMaterials.Add(material);
	}

	private void BuildSummaries()
	{
		MaterialSummaries.Clear();

		MaterialSummaries.Add(new()
		{
			Title = Options.Category.Raw,
			Value = Materials.Where(x => x.Category == Options.Category.Raw).Sum(x => x.Count)
		});

		MaterialSummaries.Add(new()
		{
			Title = Options.Category.Manufactured,
			Value = Materials.Where(x => x.Category == Options.Category.Manufactured).Sum(x => x.Count)
		});

		MaterialSummaries.Add(new()
		{
			Title = Options.Category.Encoded,
			Value = Materials.Where(x => x.Category == Options.Category.Encoded).Sum(x => x.Count)
		});
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