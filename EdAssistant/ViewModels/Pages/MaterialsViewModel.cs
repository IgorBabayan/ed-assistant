using EdAssistant.Models.ShipLocker;

namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.Materials)]
public sealed partial class MaterialsViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;
    private readonly List<MaterialsInventoryItemDTO> _allItems = new();

    [ObservableProperty]
    private ObservableCollection<MaterialsInventoryItemDTO> filteredItems = new(); 
    
    [ObservableProperty]
    private bool showRaw = true;

    [ObservableProperty]
    private bool showManufactures = true;

    [ObservableProperty]
    private bool showEncodes = true;

    [ObservableProperty]
    private string searchText = string.Empty;

    public bool HasNoItems => FilteredItems.Count == 0;

    public string RawText => string.Format(Localization.Instance["MaterialsWindow.Raw"], FilteredItems.Count(item => item.Category == MaterialCategory.Raw));
    public string ManufacturesText => string.Format(Localization.Instance["MaterialsWindow.Manufactured"], FilteredItems.Count(item => item.Category == MaterialCategory.Manufactured));
    public string EncodesText => string.Format(Localization.Instance["MaterialsWindow.Encoded"], FilteredItems.Count(item => item.Category == MaterialCategory.Encoded));

    public MaterialsViewModel(IGameDataService gameDataService)
    {
        _gameDataService = gameDataService;
        _gameDataService.JournalLoaded += OnJournalEventLoaded;

        var existingData = _gameDataService.GetLatestJournal<MaterialsEvent>();
        if (existingData is not null)
        {
            ProcessMaterials(existingData);
        }
    }

    public override void Dispose() => _gameDataService.JournalLoaded -= OnJournalEventLoaded;

    partial void OnShowRawChanged(bool value) => ApplyFilters();
    partial void OnShowManufacturesChanged(bool value) => ApplyFilters();
    partial void OnShowEncodesChanged(bool value) => ApplyFilters();
    partial void OnSearchTextChanged(string value) => ApplyFilters();

    private void OnJournalEventLoaded(object? sender, JournalEventLoadedEventArgs e)
    {
        if (e is { EventType: JournalEventType.Materials, Event: MaterialsEvent materialsEvent })
        {
            ProcessMaterials(materialsEvent);
        }
    }

    private void ProcessMaterials(MaterialsEvent materialsEvent)
    {
        _allItems.Clear();
        _allItems.AddRange(materialsEvent.Raw.Select(m => new MaterialsInventoryItemDTO
        {
            Name = string.IsNullOrWhiteSpace(m.NameLocalised)
                ? $"{m.Name.Capitalize()} ({m.Count})"
                : $"{m.NameLocalised} ({m.Count})",
            Category = MaterialCategory.Raw
        }));
        _allItems.AddRange(materialsEvent.Manufactured.Select(m => new MaterialsInventoryItemDTO
        {
            Name = string.IsNullOrWhiteSpace(m.NameLocalised)
                ? $"{m.Name.Capitalize()} ({m.Count})"
                : $"{m.NameLocalised} ({m.Count})",
            Category = MaterialCategory.Manufactured
        }));
        _allItems.AddRange(materialsEvent.Encoded.Select(m => new MaterialsInventoryItemDTO
        {
            Name = string.IsNullOrWhiteSpace(m.NameLocalised)
                ? $"{m.Name.Capitalize()} ({m.Count})"
                : $"{m.NameLocalised} ({m.Count})",
            Category = MaterialCategory.Encoded
        }));

        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = _allItems.Where(item =>
        {
            var categoryMatch = item.Category switch
            {
                MaterialCategory.Raw => ShowRaw,
                MaterialCategory.Manufactured => ShowManufactures,
                MaterialCategory.Encoded => ShowEncodes,
                _ => false
            };

            if (!categoryMatch)
                return false;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLowerInvariant();
                return item.Name.ToLowerInvariant().Contains(searchLower);
            }

            return true;
        }).OrderBy(item => item.Name).ThenBy(item => item.Category);

        FilteredItems = new ObservableCollection<MaterialsInventoryItemDTO>(filtered);

        OnPropertyChanged(nameof(HasNoItems));
        OnPropertyChanged(nameof(RawText));
        OnPropertyChanged(nameof(ManufacturesText));
        OnPropertyChanged(nameof(EncodesText));
    }
}