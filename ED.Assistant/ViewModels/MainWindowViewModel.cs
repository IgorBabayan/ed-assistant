using CommunityToolkit.Mvvm.ComponentModel;

namespace ED.Assistant.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
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
}
