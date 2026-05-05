using ED.Assistant.Presentation.ViewModels.Material;

namespace ED.Assistant.Presentation.ViewModels.ShipLocker;

public partial class ShipLockerViewModel : LoadableViewModel
{
	public ObservableCollection<MaterialItemViewModel> Materials { get; } = [];
	public ObservableCollection<MaterialItemViewModel> FilteredMaterials { get; } = [];
	public ObservableCollection<MaterialSummaryViewModel> MaterialSummaries { get; } = [];

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

	private class Options
	{
		internal class Categories
		{
			internal const string All = "All";
			internal const string Items = "Items";
			internal const string Components = "Components";
			internal const string Consumables = "Consumables";
			internal const string Data = "Data";
		}

		internal class Sort
		{
			internal const string Name = "Name";
			internal const string Category = "Category";
			internal const string Count = "Count";
		}
	}

	protected override bool ActivateOnNavigation => true;

	public ShipLockerViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

	protected override async Task UpdateFromStateAsync(JournalState state, 
		CancellationToken cancellationToken = default)
	{
		if (state.ShipLocker is null)
			return;

		var materials = await Task.Run(() =>
		{
			var result = new List<MaterialItemViewModel>();

			AddMaterials(result, state.ShipLocker.Items, Options.Categories.Items);
			AddMaterials(result, state.ShipLocker.Components, Options.Categories.Components);
			AddMaterials(result, state.ShipLocker.Consumables, Options.Categories.Consumables);
			AddMaterials(result, state.ShipLocker.Data, Options.Categories.Data);

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
				});

			target.Add(viewModel);
		}
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

		FilteredMaterials.Clear();

		foreach (var material in query)
			FilteredMaterials.Add(material);
	}

	private void BuildSummaries()
	{
		MaterialSummaries.Clear();

		AddSummary(Options.Categories.Items);
		AddSummary(Options.Categories.Components);
		AddSummary(Options.Categories.Consumables);
		AddSummary(Options.Categories.Data);

		MaterialSummaries.Add(new()
		{
			Title = "Low stock",
			Value = Materials.Count(x => x.MaxCapacity > 0 && x.Count < x.MaxCapacity * 0.25),
			Subtitle = "< 25%"
		});
	}

	private void AddSummary(string category) => MaterialSummaries.Add(new()
	{
		Title = category,
		Value = Materials
				.Where(x => x.Category == category)
				.Sum(x => x.Count)
	});
}
