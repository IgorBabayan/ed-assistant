namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.Materials)]
public sealed partial class MaterialsViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;

    public MaterialsViewModel(IGameDataService gameDataService)
    {
        _gameDataService = gameDataService;
        _gameDataService.JournalEventLoaded += OnJournalEventLoaded;

        var existingData = _gameDataService.GetLatestJournalEvent<MaterialsEvent>();
        if (existingData is not null)
        {
            ProcessMaterials(existingData);
        }
    }

    private void OnJournalEventLoaded(object? sender, JournalEventLoadedEventArgs e)
    {
        if (e is { EventType: JournalEventType.Materials, Event: MaterialsEvent materialsEvent })
        {
            ProcessMaterials(materialsEvent);
        }
    }

    private void ProcessMaterials(MaterialsEvent materialsEvent)
    {

    }
}