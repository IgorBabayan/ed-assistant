using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EdAssistant.Services.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using EdAssistant.Translations;
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
    private string? journalsFolderPath;

    public SettingsViewModel(IFolderPickerService picker, ILogger<SettingsViewModel> logger)
    {
        _picker = picker;
        _logger = logger;

        var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        JournalsFolderPath = OperatingSystem.IsWindows()
            ? Path.Combine(homeFolder, "Saved Games", "Frontier Developments", "Elite Dangerous")
            : OperatingSystem.IsLinux()
                ? Path.Combine(homeFolder, ".steam", "Saved Games", "Frontier Developments", "Elite Dangerous")
                : throw new NotImplementedException(Localization.Instance["Exceptions.OSNotSupported"]);
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

    [RelayCommand]
    private async Task ReadAllAsync()
    {

    }
}