using ShipLockerEvent = EdAssistant.Models.ShipLocker.ShipLockerEvent;

namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.ShipLocker)]
public sealed partial class StorageViewModel : PageViewModel, IDisposable
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

    [ObservableProperty]
    private bool isLoading = false;

    [ObservableProperty]
    private bool hasData = false;

    public bool HasNoItems => !IsLoading && HasData && FilteredItems.Count == 0;
    public bool ShowEmptyState => !IsLoading && !HasData;
    public bool ShowContent => !IsLoading && HasData;

    public string ItemsText => string.Format(Localization.Instance["StorageWindow.Items"],
        HasData ? FilteredItems.Count(item => item.Category == ItemCategory.Items) : 0);
    public string ComponentsText => string.Format(Localization.Instance["StorageWindow.Components"],
        HasData ? FilteredItems.Count(item => item.Category == ItemCategory.Components) : 0);
    public string ConsumablesText => string.Format(Localization.Instance["StorageWindow.Consumables"],
        HasData ? FilteredItems.Count(item => item.Category == ItemCategory.Consumables) : 0);
    public string DataText => string.Format(Localization.Instance["StorageWindow.Data"],
        HasData ? FilteredItems.Count(item => item.Category == ItemCategory.Data) : 0);

    public StorageViewModel(IGameDataService gameDataService)
    {
        _gameDataService = gameDataService;
        _gameDataService.DataLoaded += OnGameDataLoaded;

        // Simple: check for existing data immediately
        CheckForExistingData();
    }

    private void CheckForExistingData()
    {
        var existingData = _gameDataService.GetData<ShipLockerEvent>();
        if (existingData != null)
        {
            ProcessShipLockerData(existingData);
        }
        else
        {
            IsLoading = true;
            // Check periodically for data to arrive
            _ = Task.Run(async () =>
            {
                for (int i = 0; i < 30; i++) // Check for 3 seconds
                {
                    await Task.Delay(100);
                    var data = _gameDataService.GetData<ShipLockerEvent>();
                    if (data != null)
                    {
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            ProcessShipLockerData(data);
                            IsLoading = false;
                        });
                        return;
                    }
                }

                // No data found after waiting
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    IsLoading = false;
                });
            });
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
            IsLoading = false;
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

        HasData = true;
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        if (!HasData) return;

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

        OnPropertyChanged(nameof(HasNoItems));
        OnPropertyChanged(nameof(ShowEmptyState));
        OnPropertyChanged(nameof(ShowContent));
        OnPropertyChanged(nameof(ItemsText));
        OnPropertyChanged(nameof(ComponentsText));
        OnPropertyChanged(nameof(ConsumablesText));
        OnPropertyChanged(nameof(DataText));
    }

    public override void Dispose()
    {
        _gameDataService.DataLoaded -= OnGameDataLoaded;
    }
}