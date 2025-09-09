namespace EdAssistant.Services.Storage;

class FolderPickerService : IFolderPickerService
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
        var sp = GetStorageProvider() ?? throw new InvalidOperationException(Localization.Instance["Exceptions.NoStorageProvider"]);

        var options = new FolderPickerOpenOptions
        {
            AllowMultiple = false,
            Title = title ?? Localization.Instance["Common.SelectFolderTitle"],
            SuggestedStartLocation = string.IsNullOrWhiteSpace(suggestedStartPath)
                ? null
                : await sp.TryGetFolderFromPathAsync(suggestedStartPath)
        };

        var result = await sp.OpenFolderPickerAsync(options);
        return result.FirstOrDefault();
    }

    public string GetDefaultJournalsPath()
    {
        var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string path;
        if (OperatingSystem.IsWindows())
        {
            path = Path.Combine(homeFolder, "Saved Games", "Frontier Developments", "Elite Dangerous");
            return TrySetupJournalsPath(path);
        }

        if (OperatingSystem.IsLinux())
        {
            //! TODO: check on Linux to correct path
            path = Path.Combine(homeFolder, ".steam", "Saved Games", "Frontier Developments", "Elite Dangerous");
            return TrySetupJournalsPath(path);
        }
        else
        {
            throw new NotImplementedException(Localization.Instance["Exceptions.OSNotSupported"]);
        }
    }

    private string TrySetupJournalsPath(string path)
    {
        if (Directory.Exists(path))
        {
            return path;
        }

        throw new DirectoryNotFoundException(string.Format(Localization.Instance["Exceptions.DirectoryNotFound"], path));
    }
}