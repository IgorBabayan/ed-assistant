namespace EdAssistant.Services.Storage;

class FolderPickerService(ISettingsService settingsService, ILogger<FolderPickerService> logger) : IFolderPickerService
{
    private static IStorageProvider? GetStorageProvider()
    {
        var app = Application.Current?.ApplicationLifetime;

        if (app is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var active = desktop.Windows.FirstOrDefault(w => w.IsActive) ?? desktop.MainWindow;
            return active?.StorageProvider;
        }

        if (app is ISingleViewApplicationLifetime single)
        {
            var top = TopLevel.GetTopLevel(single.MainView);
            return top?.StorageProvider;
        }

        return null;
    }

    public async Task<IStorageFolder?> PickSingleFolderAsync(string? title = null, string? suggestedStartPath = null)
    {
        var sp = GetStorageProvider() ?? 
                 throw new InvalidOperationException(Localization.Instance["FolderService.Exceptions.NoStorageProvider"]);

        var options = new FolderPickerOpenOptions
        {
            AllowMultiple = false,
            Title = title ?? Localization.Instance["FolderService.SelectFolderTitle"],
            SuggestedStartLocation = string.IsNullOrWhiteSpace(suggestedStartPath)
                ? null
                : await sp.TryGetFolderFromPathAsync(suggestedStartPath)
        };

        var result = await sp.OpenFolderPickerAsync(options);
        return result.FirstOrDefault();
    }

    public string GetDefaultJournalsPath()
    {
        var path  = settingsService.GetSetting<string>("JournalFolderPath");
        if (!string.IsNullOrWhiteSpace(path) && TrySetupJournalsPath(ref path))
        {
            return path;
        }
        
        var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (OperatingSystem.IsWindows())
        {
            path = Path.Combine(homeFolder, "Saved Games", "Frontier Developments", "Elite Dangerous");
            if (!TrySetupJournalsPath(ref path))
            {
                logger.LogWarning(Localization.Instance["FolderService.Exceptions.NoJournalsPath"]);
                return string.Empty;
            }
            return path;
        }

        if (OperatingSystem.IsLinux())
        {
            //! TODO: check on Linux to correct path
            path = Path.Combine(homeFolder, ".steam", "Saved Games", "Frontier Developments", "Elite Dangerous");
            if (!TrySetupJournalsPath(ref path))
            {
                logger.LogWarning(Localization.Instance["FolderService.Exceptions.NoJournalsPath"]);
                return string.Empty;
            }
            return path;
        }
        
        throw new NotImplementedException(Localization.Instance["FolderService.Exceptions.OSNotSupported"]);
    }

    private bool TrySetupJournalsPath(ref string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }

        logger.LogWarning(Localization.Instance["Exceptions.DirectoryNotFound"], path);
        return false;
    }
}