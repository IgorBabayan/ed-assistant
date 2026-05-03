using ED.Assistant.Services.Journal;
using System.Collections.ObjectModel;

namespace ED.Assistant.ViewModels;

public partial class MaterialViewModel : LoadableViewModel
{
	public ObservableCollection<MaterialItemViewModel> Materials { get; } = [];

	public ObservableCollection<MaterialItemViewModel> FilteredMaterials { get; } = [];

	public ObservableCollection<MaterialSummaryViewModel> MaterialSummaries { get; } = [];

	public IReadOnlyList<string> Categories { get; } =
	[
		"All",
		"Raw",
		"Manufactured",
		"Encoded"
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

	public MaterialViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore)
		: base(journalLoader, stateStore) { }

	protected override void UpdateFromState(JournalState state)
	{
		if (state?.Materials is null)
			return;

		Materials.Clear();

		AddMaterials(state.Materials.Raw, "Raw");
		AddMaterials(state.Materials.Manufactured, "Manufactured");
		AddMaterials(state.Materials.Encoded, "Encoded");

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

		var raw = Materials
			.Where(x => x.Category == "Raw")
			.Sum(x => x.Count);

		var manufactured = Materials
			.Where(x => x.Category == "Manufactured")
			.Sum(x => x.Count);

		var encoded = Materials
			.Where(x => x.Category == "Encoded")
			.Sum(x => x.Count);

		var lowStock = Materials
			.Count(x => x.Count < x.MaxCapacity * 0.25);

		MaterialSummaries.Add(new()
		{
			Title = "Raw",
			Value = raw
		});

		MaterialSummaries.Add(new()
		{
			Title = "Manufactured",
			Value = manufactured
		});

		MaterialSummaries.Add(new()
		{
			Title = "Encoded",
			Value = encoded
		});

		MaterialSummaries.Add(new()
		{
			Title = "Low stock",
			Value = lowStock,
			Subtitle = "< 25%"
		});
	}
}

public sealed partial class MaterialItemViewModel : ObservableObject
{
	public string Name { get; init; } = string.Empty;

	public string Category { get; init; } = string.Empty;

	public int Count { get; init; }

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