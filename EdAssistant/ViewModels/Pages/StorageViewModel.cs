using ShipLockerEvent = EdAssistant.Models.ShipLocker.ShipLockerEvent;

namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.ShipLocker)]
public sealed partial class StorageViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;
    private readonly List<StorageInventoryItemDTO> _allItems = new();

    [ObservableProperty]
    private ObservableCollection<StorageInventoryItemDTO> filteredItems = new();

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

    public bool HasNoItems => FilteredItems.Count == 0;

    public string ItemsText => string.Format(Localization.Instance["StorageWindow.Items"], FilteredItems.Count(item => item.Category == ItemCategory.Items));
    public string ComponentsText => string.Format(Localization.Instance["StorageWindow.Components"], FilteredItems.Count(item => item.Category == ItemCategory.Components));
    public string ConsumablesText => string.Format(Localization.Instance["StorageWindow.Consumables"], FilteredItems.Count(item => item.Category == ItemCategory.Consumables));
    public string DataText => string.Format(Localization.Instance["StorageWindow.Data"], FilteredItems.Count(item => item.Category == ItemCategory.Data));

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

        _allItems.AddRange(shipData.Items.Select(item => new StorageInventoryItemDTO
        {
            Name = item.Name,
            NameLocalised = string.IsNullOrWhiteSpace(item.NameLocalised)
                ? $"{item.Name.Capitalize()} ({item.Count})"
                : $"{item.NameLocalised} ({item.Count})",
            Category = ItemCategory.Items
        }));

        _allItems.AddRange(shipData.Components.Select(component => new StorageInventoryItemDTO
        {
            Name = component.Name,
            NameLocalised = string.IsNullOrWhiteSpace(component.NameLocalised)
                ? $"{component.Name.Capitalize()} ({component.Count})"
                : $"{component.NameLocalised} ({component.Count})",
            Category = ItemCategory.Components
        }));

        _allItems.AddRange(shipData.Consumables.Select(consumable => new StorageInventoryItemDTO
        {
            Name = consumable.Name,
            NameLocalised = string.IsNullOrWhiteSpace(consumable.NameLocalised)
                ? $"{consumable.Name.Capitalize()} ({consumable.Count})"
                : $"{consumable.NameLocalised} ({consumable.Count})",
            Category = ItemCategory.Consumables
        }));

        _allItems.AddRange(shipData.Data.Select(dataItem => new StorageInventoryItemDTO
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
                return string.Equals(item.Name, SearchText, StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(item.NameLocalised, SearchText, StringComparison.OrdinalIgnoreCase);
            }

            return true;
        }).OrderBy(item => item.NameLocalised).ThenBy(item => item.Category);

        FilteredItems = new ObservableCollection<StorageInventoryItemDTO>(filtered);

        OnPropertyChanged(nameof(HasNoItems));
        OnPropertyChanged(nameof(ItemsText));
        OnPropertyChanged(nameof(ComponentsText));
        OnPropertyChanged(nameof(ConsumablesText));
        OnPropertyChanged(nameof(DataText));
    }
}