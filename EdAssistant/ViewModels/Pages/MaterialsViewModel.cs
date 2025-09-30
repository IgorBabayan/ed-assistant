namespace EdAssistant.ViewModels.Pages;

public sealed partial class MaterialsViewModel(IJournalService journalService, ILogger<MaterialsViewModel> logger)
    : PageViewModel(logger)
{
    private readonly List<MaterialsInventoryItemDTO> _allItems = [];
    
    [ObservableProperty]
    private ObservableCollection<MaterialsInventoryItemDTO> _filteredItems = []; 
    
    [ObservableProperty]
    private bool _showRaw = true;

    [ObservableProperty]
    private bool _showManufactures = true;

    [ObservableProperty]
    private bool _showEncodes = true;

    [ObservableProperty]
    private string _searchText = string.Empty;
    
    public bool HasNoItems => FilteredItems.Count == 0;

    public string RawText => string.Format(Localization.Instance["MaterialsPage.Raw"], FilteredItems.Count(item => item.CategoryEnum == MaterialCategoryEnum.Raw));
    public string ManufacturesText => string.Format(Localization.Instance["MaterialsPage.Manufactured"], FilteredItems.Count(item => item.CategoryEnum == MaterialCategoryEnum.Manufactured));
    public string EncodesText => string.Format(Localization.Instance["MaterialsPage.Encoded"], FilteredItems.Count(item => item.CategoryEnum == MaterialCategoryEnum.Encoded));
    
    protected override async Task OnInitializeAsync()
    {
        logger.LogInformation(Localization.Instance["MaterialsPage.Initializing"]);
        WeakReferenceMessenger.Default.Send(new LoadingMessage(true));
        
        try
        {
            await LoadMaterialsDataAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["MaterialsPage.Exceptions.FailedToInitialize"]);
        }
        finally
        {
            WeakReferenceMessenger.Default.Send(new LoadingMessage(false));
        }
    }
    
    partial void OnShowRawChanged(bool value) => ApplyFilters();
    partial void OnShowManufacturesChanged(bool value) => ApplyFilters();
    partial void OnShowEncodesChanged(bool value) => ApplyFilters();
    partial void OnSearchTextChanged(string value) => ApplyFilters();

    private async Task LoadMaterialsDataAsync()
    {
        var materials = (await journalService.GetLatestJournalEntriesAsync<MaterialsEvent>()).LastOrDefault();
        if (materials is null)
            return;
        
        ProcessMaterials(materials);
    }
    
    private void ProcessMaterials(MaterialsEvent materialsEvent)
    {
        _allItems.Clear();
        _allItems.AddRange(materialsEvent.Raw.Select(m => new MaterialsInventoryItemDTO
        {
            Name = string.IsNullOrWhiteSpace(m.NameLocalised)
                ? $"{m.Name.Capitalize()} ({m.Count})"
                : $"{m.NameLocalised} ({m.Count})",
            CategoryEnum = MaterialCategoryEnum.Raw
        }));
        _allItems.AddRange(materialsEvent.Manufactured.Select(m => new MaterialsInventoryItemDTO
        {
            Name = string.IsNullOrWhiteSpace(m.NameLocalised)
                ? $"{m.Name.Capitalize()} ({m.Count})"
                : $"{m.NameLocalised} ({m.Count})",
            CategoryEnum = MaterialCategoryEnum.Manufactured
        }));
        _allItems.AddRange(materialsEvent.Encoded.Select(m => new MaterialsInventoryItemDTO
        {
            Name = string.IsNullOrWhiteSpace(m.NameLocalised)
                ? $"{m.Name.Capitalize()} ({m.Count})"
                : $"{m.NameLocalised} ({m.Count})",
            CategoryEnum = MaterialCategoryEnum.Encoded
        }));

        ApplyFilters();
    }
    
    private void ApplyFilters()
    {
        var filtered = _allItems.Where(item =>
        {
            var categoryMatch = item.CategoryEnum switch
            {
                MaterialCategoryEnum.Raw => ShowRaw,
                MaterialCategoryEnum.Manufactured => ShowManufactures,
                MaterialCategoryEnum.Encoded => ShowEncodes,
                _ => false
            };

            if (!categoryMatch)
                return false;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                return string.Equals(item.Name, SearchText, StringComparison.OrdinalIgnoreCase);
            }

            return true;
        }).OrderBy(item => item.Name).ThenBy(item => item.CategoryEnum);

        FilteredItems = new ObservableCollection<MaterialsInventoryItemDTO>(filtered);

        OnPropertyChanged(nameof(HasNoItems));
        OnPropertyChanged(nameof(RawText));
        OnPropertyChanged(nameof(ManufacturesText));
        OnPropertyChanged(nameof(EncodesText));
    }
}