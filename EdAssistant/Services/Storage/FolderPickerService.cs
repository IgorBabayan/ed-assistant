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
        return result?.FirstOrDefault();
    }
}