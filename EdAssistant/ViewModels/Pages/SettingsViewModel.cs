using CommunityToolkit.Mvvm.ComponentModel;

namespace EdAssistant.ViewModels.Pages;

public sealed partial class SettingsViewModel : PageViewModel
{
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
}