using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EdAssistant.Services.Storage;
using EdAssistant.Translations;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using EdAssistant.Models.Enums;
using Path = System.IO.Path;

namespace EdAssistant.ViewModels.Pages;

public sealed partial class SettingsViewModel : PageViewModel
{
    private readonly IFolderPickerService _picker;
    private readonly ILogger<SettingsViewModel> _logger;

    [ObservableProperty]
    private bool materials;

    [ObservableProperty]
    private bool storage;

    [ObservableProperty]
    private bool system;

    [ObservableProperty]
    private bool planet;

    [ObservableProperty]
    private bool marketConnector;

    [ObservableProperty]
    private bool log;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ReadAllCommand))]
    private string? journalsFolderPath;

    public SettingsViewModel(IFolderPickerService picker, ILogger<SettingsViewModel> logger)
    {
        _picker = picker;
        _logger = logger;

        Materials = true;
        Storage = true;
        System = true;
        Planet = true;
        MarketConnector = true;
        Log = true;

        PrepareDefaultJournalsPath();
    }

    private void PrepareDefaultJournalsPath()
    {
        var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string path;
        if (OperatingSystem.IsWindows())
        {
            path = Path.Combine(homeFolder, "Saved Games", "Frontier Developments", "Elite Dangerous");
            TrySetupJournalsPath(path);
        }
        else if (OperatingSystem.IsLinux())
        {
            //! TODO: check on Linux to correct path
            path = Path.Combine(homeFolder, ".steam", "Saved Games", "Frontier Developments", "Elite Dangerous");
            TrySetupJournalsPath(path);
        }
        else
        {
            throw new NotImplementedException(Localization.Instance["Exceptions.OSNotSupported"]);
        }
    }

    private void TrySetupJournalsPath(string path)
    {
        if (Directory.Exists(path))
        {
            JournalsFolderPath = path;
        }
        else
        {
            throw new DirectoryNotFoundException(string.Format(Localization.Instance["Exceptions.DirectoryNotFound"], path));
        }
    }

    [RelayCommand]
    private async Task BrowseFolderAsync()
    {
        var folder = await _picker.PickSingleFolderAsync();
        if (folder is null)
            return;

        string? path = null;
        try
        {
            path = folder.TryGetLocalPath();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,
                Localization.Instance["Exceptions.FailedGettingFolder"],
                folder!.Name, folder!.GetType().Name);
        }

        JournalsFolderPath = path ?? folder!.Name;
    }

    [RelayCommand(CanExecute = nameof(CanReadAll))]
    private async Task ReadAllAsync()
    {

    }

    private bool CanReadAll() => !string.IsNullOrWhiteSpace(JournalsFolderPath) && Directory.Exists(JournalsFolderPath);

    partial void OnMaterialsChanged(bool oldValue, bool newValue) => WeakReferenceMessenger.Default.Send(new DockVisibilityChanged(DockEnum.Materials, newValue));
    partial void OnStorageChanged(bool oldValue, bool newValue) => WeakReferenceMessenger.Default.Send(new DockVisibilityChanged(DockEnum.Storage, newValue));
    partial void OnSystemChanged(bool oldValue, bool newValue) => WeakReferenceMessenger.Default.Send(new DockVisibilityChanged(DockEnum.System, newValue));
    partial void OnPlanetChanged(bool oldValue, bool newValue) => WeakReferenceMessenger.Default.Send(new DockVisibilityChanged(DockEnum.Planet, newValue));
    partial void OnMarketConnectorChanged(bool oldValue, bool newValue) => WeakReferenceMessenger.Default.Send(new DockVisibilityChanged(DockEnum.MarketConnector, newValue));
    partial void OnLogChanged(bool oldValue, bool newValue) => WeakReferenceMessenger.Default.Send(new DockVisibilityChanged(DockEnum.Log, newValue));
}