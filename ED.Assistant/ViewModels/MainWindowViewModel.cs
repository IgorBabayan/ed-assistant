using CommunityToolkit.Mvvm.ComponentModel;
using ED.Assistant.Services.DialogService;

namespace ED.Assistant.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
	private readonly IDialogService _dialogService;
	private readonly ISettingsViewModel _settingsViewModel;

	public MainWindowViewModel(IDialogService dialogService, ISettingsViewModel settingsViewModel)
	{
		_dialogService = dialogService;
		_settingsViewModel = settingsViewModel;
	}

	[ObservableProperty]
    private string? _cMDR = "o7, Commander";

	[ObservableProperty]
	private string? _ship = "Ship not found";

	[ObservableProperty]
	private string? _status = "Ready";

	[RelayCommand]
    private async Task Load()
    {
        
    }

	[RelayCommand]
	private async Task Settings()
	{
		await _dialogService.ShowDialogAsync<SettingsViewModel, bool>(_settingsViewModel);
	}
}
