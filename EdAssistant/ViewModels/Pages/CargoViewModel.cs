using CargoEvent = EdAssistant.Models.Cargo.CargoEvent;

namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.Cargo)]
public sealed partial class CargoViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;
    private readonly List<CargoInventoryItemDTO> _allItems = new();

    [ObservableProperty]
    private ObservableCollection<CargoInventoryItemDTO> filteredItems = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    public bool HasNoItems => FilteredItems.Count == 0;

    public CargoViewModel(IGameDataService gameDataService)
    {
        _gameDataService = gameDataService;
        _gameDataService.DataLoaded += OnGameDataLoaded;

        var existingData = _gameDataService.GetData<CargoEvent>();
        if (existingData is not null)
        {
            ProcessCargoData(existingData);
        }
    }

    public override void Dispose() => _gameDataService.DataLoaded -= OnGameDataLoaded;

    partial void OnSearchTextChanged(string value) => ApplyFilters();

    private void OnGameDataLoaded(object? sender, GameDataLoadedEventArgs e)
    {
        if (e.DataType == typeof(CargoEvent) && e.Data is CargoEvent cargoData)
        {
            ProcessCargoData(cargoData);
        }
    }

    private void ProcessCargoData(CargoEvent cargoData)
    {
        _allItems.Clear();
        _allItems.AddRange(cargoData.Inventory.Select(x => new CargoInventoryItemDTO
        {
            Name = $"{x.Name.Capitalize()} ({x.Count})",
            IsStolen = Convert.ToBoolean(x.Stolen)
        }));

        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = _allItems.Where(item =>
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLowerInvariant();
                return item.Name.ToLowerInvariant().Contains(searchLower);
            }

            return true;
        }).OrderBy(item => item.Name);

        FilteredItems = new ObservableCollection<CargoInventoryItemDTO>(filtered);
        OnPropertyChanged(nameof(HasNoItems));
    }
}