namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.ShipLocker)]
public sealed partial class StorageViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;
    private readonly List<CategorizedInventoryItemDTO> _allItems = new();

    [ObservableProperty]
    private ObservableCollection<CategorizedInventoryItemDTO> filteredItems = new();

    [ObservableProperty]
    private bool showItems = true;

    [ObservableProperty]
    private bool showComponents = true;

    [ObservableProperty]
    private bool showConsumables = true;

    [ObservableProperty]
    private bool showData = true;

    [ObservableProperty]
    private string searchText = string.Empty;

    public string ItemsCheckboxText => string.Format(Localization.Instance["StorageWindow.Items"], _allItems.Count(item => item.Category == ItemCategory.Items));
    public string ComponentsCheckboxText => string.Format(Localization.Instance["StorageWindow.Components"], _allItems.Count(item => item.Category == ItemCategory.Components));
    public string ConsumablesCheckboxText => string.Format(Localization.Instance["StorageWindow.Consumables"], _allItems.Count(item => item.Category == ItemCategory.Consumables));
    public string DataCheckboxText => string.Format(Localization.Instance["StorageWindow.Data"], _allItems.Count(item => item.Category == ItemCategory.Data));

    public StorageViewModel(IGameDataService gameDataService)
    {
        _gameDataService = gameDataService;
        _gameDataService.DataLoaded += OnGameDataLoaded;

        var existingData = _gameDataService.GetData<ShipLockerEvent>();
        if (existingData is not null)
        {
            ProcessShipLockerData(existingData);
        }
    }

    partial void OnShowItemsChanged(bool value) => ApplyFilters();
    partial void OnShowComponentsChanged(bool value) => ApplyFilters();
    partial void OnShowConsumablesChanged(bool value) => ApplyFilters();
    partial void OnShowDataChanged(bool value) => ApplyFilters();
    partial void OnSearchTextChanged(string value) => ApplyFilters();

    private void OnGameDataLoaded(object? sender, GameDataLoadedEventArgs e)
    {
        if (e.DataType == typeof(ShipLockerEvent) && e.Data is ShipLockerEvent shipLocker)
        {
            ProcessShipLockerData(shipLocker);
        }
    }

    private void ProcessShipLockerData(ShipLockerEvent shipData)
    {
        _allItems.Clear();

        _allItems.AddRange(shipData.Items.Select(item => new CategorizedInventoryItemDTO
        {
            Name = item.Name,
            NameLocalised = string.IsNullOrWhiteSpace(item.NameLocalised)
                ? $"{item.Name.Capitalize()} ({item.Count})"
                : $"{item.NameLocalised} ({item.Count})",
            Category = ItemCategory.Items
        }));

        _allItems.AddRange(shipData.Components.Select(component => new CategorizedInventoryItemDTO
        {
            Name = component.Name,
            NameLocalised = string.IsNullOrWhiteSpace(component.NameLocalised)
                ? $"{component.Name.Capitalize()} ({component.Count})"
                : $"{component.NameLocalised} ({component.Count})",
            Category = ItemCategory.Components
        }));

        _allItems.AddRange(shipData.Consumables.Select(consumable => new CategorizedInventoryItemDTO
        {
            Name = consumable.Name,
            NameLocalised = string.IsNullOrWhiteSpace(consumable.NameLocalised)
                ? $"{consumable.Name.Capitalize()} ({consumable.Count})"
                : $"{consumable.NameLocalised} ({consumable.Count})",
            Category = ItemCategory.Consumables
        }));

        _allItems.AddRange(shipData.Data.Select(dataItem => new CategorizedInventoryItemDTO
        {
            Name = dataItem.Name,
            NameLocalised = string.IsNullOrWhiteSpace(dataItem.NameLocalised)
                ? $"{dataItem.Name.Capitalize()} ({dataItem.Count})"
                : $"{dataItem.NameLocalised} ({dataItem.Count})",
            Category = ItemCategory.Data
        }));

        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = _allItems.Where(item =>
        {
            var categoryMatch = item.Category switch
            {
                ItemCategory.Items => ShowItems,
                ItemCategory.Components => ShowComponents,
                ItemCategory.Consumables => ShowConsumables,
                ItemCategory.Data => ShowData,
                _ => false
            };

            if (!categoryMatch)
                return false;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLowerInvariant();
                return item.Name.ToLowerInvariant().Contains(searchLower) ||
                       item.NameLocalised.ToLowerInvariant().Contains(searchLower);
            }

            return true;
        }).OrderBy(item => item.NameLocalised).ThenBy(item => item.Category);

        FilteredItems = new ObservableCollection<CategorizedInventoryItemDTO>(filtered);
    }
}