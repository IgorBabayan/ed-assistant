namespace EdAssistant.Services.Initialization;

class InitializationService : IInitializationService
{
    private readonly IGameDataService _gameDataService;
    private readonly IFolderPickerService _folderPickerService;

    public InitializationService(IGameDataService gameDataService, IFolderPickerService folderPickerService)
    {
        _gameDataService = gameDataService;
        _folderPickerService = folderPickerService;
    }

    public async Task InitializeAsync()
    {
        var journalsPath = _folderPickerService.GetDefaultJournalsPath();
        if (!string.IsNullOrWhiteSpace(journalsPath) && Directory.Exists(journalsPath))
        {
            await _gameDataService.LoadAllGameDataAsync(journalsPath);
        }
    }
}