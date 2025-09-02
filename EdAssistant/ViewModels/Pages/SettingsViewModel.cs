using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EdAssistant.Helpers.Attributes;
using EdAssistant.Models.Enums;
using EdAssistant.Services.DockVisibility;
using EdAssistant.Services.Storage;
using EdAssistant.Translations;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using EdAssistant.Services.Settings;
using Path = System.IO.Path;

namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.Settings)]
public sealed partial class SettingsViewModel : PageViewModel
{
    private readonly IFolderPickerService _picker;
    private readonly ISettingsService _settingsService;
    private readonly IDockVisibilityService _dockVisibilityService;
    private readonly ILogger<SettingsViewModel> _logger;

    public bool Materials
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.Materials);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.Materials, value);
            _settingsService.SetSetting("Materials", value);
        }
    }

    public bool Storage
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.Storage);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.Storage, value);
            _settingsService.SetSetting("Storage", value);
        }
    }

    public bool System
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.System);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.System, value);
            _settingsService.SetSetting("System", value);
        }
    }

    public bool Planet
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.Planet);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.Planet, value);
            _settingsService.SetSetting("Planet", value);
        }
    }

    public bool MarketConnector
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.MarketConnector);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.MarketConnector, value);
            _settingsService.SetSetting("MarketConnector", value);
        }
    }

    public bool Log
    {
        get => _dockVisibilityService.GetVisibility(DockEnum.Log);
        set
        {
            _dockVisibilityService.SetVisibility(DockEnum.Log, value);
            _settingsService.SetSetting("Log", value);
        }
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ReadAllCommand))]
    private string? journalsFolderPath;

    public SettingsViewModel(IFolderPickerService picker, ILogger<SettingsViewModel> logger, IDockVisibilityService dockVisibilityService, ISettingsService settingsService)
    {
        _picker = picker;
        _logger = logger;
        _dockVisibilityService = dockVisibilityService;
        _settingsService = settingsService;

        PrepareDefaultJournalsPath();
        LoadSettings();

        _dockVisibilityService.VisibilityChanged += OnDockVisibilityChanged;
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

    private void LoadSettings()
    {
        _dockVisibilityService.SetVisibility(DockEnum.Materials,
            _settingsService.GetSetting("Materials", true));
        _dockVisibilityService.SetVisibility(DockEnum.Storage,
            _settingsService.GetSetting("Storage", true));
        _dockVisibilityService.SetVisibility(DockEnum.System,
            _settingsService.GetSetting("System", true));
        _dockVisibilityService.SetVisibility(DockEnum.Planet,
            _settingsService.GetSetting("Planet", true));
        _dockVisibilityService.SetVisibility(DockEnum.MarketConnector,
            _settingsService.GetSetting("MarketConnector", true));
        _dockVisibilityService.SetVisibility(DockEnum.Log,
            _settingsService.GetSetting("Log", true));
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

    private void OnDockVisibilityChanged(object? sender, DockVisibilityChangedEventArgs e) =>
        OnPropertyChanged(e.Dock switch
        {
            DockEnum.Materials => nameof(Materials),
            DockEnum.Storage => nameof(Storage),
            DockEnum.System => nameof(System),
            DockEnum.Planet => nameof(Planet),
            DockEnum.MarketConnector => nameof(MarketConnector),
            DockEnum.Log => nameof(Log),
            _ => string.Empty
        });
}