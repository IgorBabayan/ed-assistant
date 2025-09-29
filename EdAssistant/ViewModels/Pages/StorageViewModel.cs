namespace EdAssistant.ViewModels.Pages;

public sealed partial class StorageViewModel(IJournalService journalService, ILogger<StorageViewModel> logger)
    : PageViewModel(logger)
{
    private readonly List<StorageInventoryItemDTO> _allItems = new();

    [ObservableProperty]
    private ObservableCollection<StorageInventoryItemDTO> _filteredItems = new();

    [ObservableProperty]
    private bool _showItems = true;

    [ObservableProperty]
    private bool _showComponents = true;

    [ObservableProperty]
    private bool _showConsumables = true;

    [ObservableProperty]
    private bool _showData = true;

    [ObservableProperty]
    private string _searchText = string.Empty;
    
    [ObservableProperty]
    private bool _isLoadingStorage;

    public bool HasNoItems => FilteredItems.Count == 0;

    public string ItemsText => string.Format(Localization.Instance["StorageWindow.Items"], FilteredItems.Count(item => item.CategoryEnum == ItemCategoryEnum.Items));
    public string ComponentsText => string.Format(Localization.Instance["StorageWindow.Components"], FilteredItems.Count(item => item.CategoryEnum == ItemCategoryEnum.Components));
    public string ConsumablesText => string.Format(Localization.Instance["StorageWindow.Consumables"], FilteredItems.Count(item => item.CategoryEnum == ItemCategoryEnum.Consumables));
    public string DataText => string.Format(Localization.Instance["StorageWindow.Data"], FilteredItems.Count(item => item.CategoryEnum == ItemCategoryEnum.Data));
    
    partial void OnShowItemsChanged(bool value) => ApplyFilters();
    partial void OnShowComponentsChanged(bool value) => ApplyFilters();
    partial void OnShowConsumablesChanged(bool value) => ApplyFilters();
    partial void OnShowDataChanged(bool value) => ApplyFilters();
    partial void OnSearchTextChanged(string value) => ApplyFilters();
    
    protected override async Task OnInitializeAsync()
    {
        logger.LogInformation(Localization.Instance["StoragePage.Initializing"]);
        IsLoadingStorage = true;
        try
        {
            await LoadStorageDataAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["StoragePage.Exceptions.FailedToInitialize"]);
        }
        finally
        {
            IsLoadingStorage = false;
        }
    }

    private async Task LoadStorageDataAsync()
    {
        var storage = (await journalService.GetLatestJournalEntriesAsync<ShipLockerEvent>()).LastOrDefault();
        if (storage is null)
            return;
        
        ProcessShipLockerData(storage);
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
            CategoryEnum = ItemCategoryEnum.Items
        }));

        _allItems.AddRange(shipData.Components.Select(component => new StorageInventoryItemDTO
        {
            Name = component.Name,
            NameLocalised = string.IsNullOrWhiteSpace(component.NameLocalised)
                ? $"{component.Name.Capitalize()} ({component.Count})"
                : $"{component.NameLocalised} ({component.Count})",
            CategoryEnum = ItemCategoryEnum.Components
        }));

        _allItems.AddRange(shipData.Consumables.Select(consumable => new StorageInventoryItemDTO
        {
            Name = consumable.Name,
            NameLocalised = string.IsNullOrWhiteSpace(consumable.NameLocalised)
                ? $"{consumable.Name.Capitalize()} ({consumable.Count})"
                : $"{consumable.NameLocalised} ({consumable.Count})",
            CategoryEnum = ItemCategoryEnum.Consumables
        }));

        _allItems.AddRange(shipData.Data.Select(dataItem => new StorageInventoryItemDTO
        {
            Name = dataItem.Name,
            NameLocalised = string.IsNullOrWhiteSpace(dataItem.NameLocalised)
                ? $"{dataItem.Name.Capitalize()} ({dataItem.Count})"
                : $"{dataItem.NameLocalised} ({dataItem.Count})",
            CategoryEnum = ItemCategoryEnum.Data
        }));

        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = _allItems.Where(item =>
        {
            var categoryMatch = item.CategoryEnum switch
            {
                ItemCategoryEnum.Items => ShowItems,
                ItemCategoryEnum.Components => ShowComponents,
                ItemCategoryEnum.Consumables => ShowConsumables,
                ItemCategoryEnum.Data => ShowData,
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
        }).OrderBy(item => item.NameLocalised).ThenBy(item => item.CategoryEnum);

        FilteredItems = new ObservableCollection<StorageInventoryItemDTO>(filtered);

        OnPropertyChanged(nameof(HasNoItems));
        OnPropertyChanged(nameof(ItemsText));
        OnPropertyChanged(nameof(ComponentsText));
        OnPropertyChanged(nameof(ConsumablesText));
        OnPropertyChanged(nameof(DataText));
    }
}