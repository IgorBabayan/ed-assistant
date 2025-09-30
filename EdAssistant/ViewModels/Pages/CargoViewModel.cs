namespace EdAssistant.ViewModels.Pages;

public sealed partial class CargoViewModel(IJournalService journalService, ILogger<CargoViewModel> logger)
    : PageViewModel(logger)
{
    private readonly List<CargoInventoryItemDTO> _allItems = [];

    [ObservableProperty]
    private ObservableCollection<CargoInventoryItemDTO> _filteredItems = [];

    [ObservableProperty]
    private string _searchText = string.Empty;

    public bool HasNoItems => FilteredItems.Count == 0;
    
    protected override async Task OnInitializeAsync()
    {
        logger.LogInformation(Localization.Instance["CargoPage.Initializing"]);
        WeakReferenceMessenger.Default.Send(new LoadingMessage(true));
        
        try
        {
            await LoadCargoDataAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["CargoPage.Exceptions.FailedToInitialize"]);
        }
        finally
        {
            WeakReferenceMessenger.Default.Send(new LoadingMessage(false));
        }
    }
    
    partial void OnSearchTextChanged(string value) => ApplyFilters();

    private async Task LoadCargoDataAsync()
    {
        var cargoEvent = (await journalService.GetLatestJournalEntriesAsync<CargoEvent>()).LastOrDefault();
        if (cargoEvent is null)
            return;
        
        _allItems.Clear();
        _allItems.AddRange(cargoEvent.Inventory.Select(x => new CargoInventoryItemDTO
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
                return string.Equals(item.Name, SearchText, StringComparison.OrdinalIgnoreCase);
            }

            return true;
        }).OrderBy(item => item.Name);

        FilteredItems = new ObservableCollection<CargoInventoryItemDTO>(filtered);
        OnPropertyChanged(nameof(HasNoItems));
    }
}