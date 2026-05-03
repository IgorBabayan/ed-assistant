using ED.Assistant.Services.Journal;
using System.Collections.ObjectModel;

namespace ED.Assistant.ViewModels;

public partial class ShipLockerViewModel : LoadableViewModel
{
	public ObservableCollection<MaterialItemViewModel> Materials { get; } = [];

	public ObservableCollection<MaterialItemViewModel> FilteredMaterials { get; } = [];

	public ObservableCollection<MaterialSummaryViewModel> MaterialSummaries { get; } = [];

	public IReadOnlyList<string> Categories { get; } =
	[
		"All",
		"Items",
		"Components",
		"Consumables",
		"Data"
	];

	public IReadOnlyList<string> SortOptions { get; } =
	[
		"Name",
		"Category",
		"Count"
	];

	[ObservableProperty]
	private string searchText = string.Empty;

	[ObservableProperty]
	private string selectedCategory = "All";

	[ObservableProperty]
	private string selectedSort = "Name";

	protected override bool ActivateOnNavigation => true;

	public ShipLockerViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		if (state?.ShipLocker is null)
			return;

		AddMaterials(state.ShipLocker.Items, "Items");
		AddMaterials(state.ShipLocker.Components, "Components");
		AddMaterials(state.ShipLocker.Consumables, "Consumables");
		AddMaterials(state.ShipLocker.Data, "Data");

		BuildSummaries();
		ApplyFilters();
	}

	partial void OnSearchTextChanged(string value) => ApplyFilters();

	partial void OnSelectedCategoryChanged(string value) => ApplyFilters();

	partial void OnSelectedSortChanged(string value) => ApplyFilters();

	private void AddMaterials(IEnumerable<MaterialItem>? source, string category)
	{
		if (source is null)
			return;

		foreach (var material in source)
		{
			Materials.Add(new MaterialItemViewModel
			{
				Name = material.FullName,
				Category = category,
				Count = material.Count
			});
		}
	}

	private void ApplyFilters()
	{
		var query = Materials.AsEnumerable();
		if (!string.IsNullOrWhiteSpace(SearchText))
		{
			query = query.Where(x => x.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
		}

		if (SelectedCategory != "All")
		{
			query = query.Where(x => x.Category == SelectedCategory);
		}

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

		var items = Materials
			.Where(x => x.Category == "Items")
			.Sum(x => x.Count);

		var components = Materials
			.Where(x => x.Category == "Components")
			.Sum(x => x.Count);

		var consumables = Materials
			.Where(x => x.Category == "Consumables")
			.Sum(x => x.Count);

		var data = Materials
			.Where(x => x.Category == "Data")
			.Sum(x => x.Count);

		var lowStock = Materials
			.Count(x => x.Count < x.MaxCapacity * 0.25);

		MaterialSummaries.Add(new()
		{
			Title = "Items",
			Value = items
		});

		MaterialSummaries.Add(new()
		{
			Title = "Components",
			Value = components
		});

		MaterialSummaries.Add(new()
		{
			Title = "Consumables",
			Value = consumables
		});

		MaterialSummaries.Add(new()
		{
			Title = "Data",
			Value = data
		});

		MaterialSummaries.Add(new()
		{
			Title = "Low stock",
			Value = lowStock,
			Subtitle = "< 25%"
		});
	}
}
