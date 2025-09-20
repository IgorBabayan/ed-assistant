namespace EdAssistant.Services.Storage;

public interface IFolderPickerService
{
    Task<IStorageFolder?> PickSingleFolderAsync(string? title = null, string? suggestedStartPath = null);
    bool TryGetDefaultJournalsPath(out string path);
}