namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.Materials)]
public sealed partial class MaterialsViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;

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