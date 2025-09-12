namespace EdAssistant.Services.Initialization;

class InitializationService(IGameDataService gameDataService, IFolderPickerService folderPickerService)
    : IInitializationService
{
    public async Task InitializeAsync()
    {
        var journalsPath = folderPickerService.GetDefaultJournalsPath();
        if (!string.IsNullOrWhiteSpace(journalsPath) && Directory.Exists(journalsPath))
        {
            await gameDataService.LoadAllGameDataAsync(journalsPath);
        }
    }
}