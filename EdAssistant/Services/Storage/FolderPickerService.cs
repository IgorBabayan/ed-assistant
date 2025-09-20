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

    public bool TryGetDefaultJournalsPath(out string path)
    {
        path  = settingsService.GetSetting<string>("JournalFolderPath");
        if (!string.IsNullOrWhiteSpace(path) && CheckPath(path))
        {
            return true;
        }

        return TryGetJournalPath(out path);
    }

    private bool TryGetJournalPath(out string path)
    {
        var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (OperatingSystem.IsWindows())
        {
            path = Path.Combine(homeFolder, "Saved Games", "Frontier Developments", "Elite Dangerous");
        }
        else if (OperatingSystem.IsLinux())
        {
            path = Path.Combine(homeFolder, ".steam", "steam", "steamapps", "compatdata", "359320", "pfx", 
                "drive_c", "users", "steamuser", "Saved Games", "Frontier Developments", "Elite Dangerous");
        }
        else
        {
            logger.LogWarning(Localization.Instance["FolderService.Exceptions.OSNotSupported"]);
            path = string.Empty;
            return false;
        }
        
        if (!CheckPath(path))
        {
            logger.LogWarning(Localization.Instance["FolderService.Exceptions.NoJournalsPath"]);
            return false;
        }

        settingsService.SetSetting("JournalFolderPath", path);
        return true;
    }
    
    private bool CheckPath(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }

        logger.LogWarning(Localization.Instance["Exceptions.DirectoryNotFound"], path);
        return false;
    }
}