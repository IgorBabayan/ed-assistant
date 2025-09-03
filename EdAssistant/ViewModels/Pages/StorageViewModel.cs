using CommunityToolkit.Mvvm.ComponentModel;
using EdAssistant.Helpers.Attributes;
using EdAssistant.Models.Enums;
using EdAssistant.Models.ShipLocker;
using EdAssistant.Services.GameData;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EdAssistant.Helpers.Extensions;
using EdAssistant.Translations;

namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.Storage)]
public sealed partial class StorageViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;
    private readonly List<CategorizedInventoryItem> _allItems = new();

    [ObservableProperty]
    private ObservableCollection<CategorizedInventoryItem> filteredItems = new();

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

        _allItems.AddRange(shipData.Items.Select(item => new CategorizedInventoryItem
        {
            Name = item.Name,
            NameLocalised = string.IsNullOrWhiteSpace(item.NameLocalised)
                ? $"{item.Name.Capitalize()} ({item.Count})"
                : $"{item.NameLocalised} ({item.Count})",
            OwnerID = item.OwnerID,
            MissionID = item.MissionID,
            Count = item.Count,
            Category = ItemCategory.Items
        }));

        _allItems.AddRange(shipData.Components.Select(component => new CategorizedInventoryItem
        {
            Name = component.Name,
            NameLocalised = string.IsNullOrWhiteSpace(component.NameLocalised)
                ? $"{component.Name.Capitalize()} ({component.Count})"
                : $"{component.NameLocalised} ({component.Count})",
            OwnerID = component.OwnerID,
            MissionID = component.MissionID,
            Count = component.Count,
            Category = ItemCategory.Components
        }));

        _allItems.AddRange(shipData.Consumables.Select(consumable => new CategorizedInventoryItem
        {
            Name = consumable.Name,
            NameLocalised = string.IsNullOrWhiteSpace(consumable.NameLocalised)
                ? $"{consumable.Name.Capitalize()} ({consumable.Count})"
                : $"{consumable.NameLocalised} ({consumable.Count})",
            OwnerID = consumable.OwnerID,
            MissionID = consumable.MissionID,
            Count = consumable.Count,
            Category = ItemCategory.Consumables
        }));

        _allItems.AddRange(shipData.Data.Select(dataItem => new CategorizedInventoryItem
        {
            Name = dataItem.Name,
            NameLocalised = string.IsNullOrWhiteSpace(dataItem.NameLocalised)
                ? $"{dataItem.Name.Capitalize()} ({dataItem.Count})"
                : $"{dataItem.NameLocalised} ({dataItem.Count})",
            OwnerID = dataItem.OwnerID,
            MissionID = dataItem.MissionID,
            Count = dataItem.Count,
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
        }).OrderBy(item => item.Category).ThenBy(item => item.NameLocalised);

        FilteredItems = new ObservableCollection<CategorizedInventoryItem>(filtered);
    }
}