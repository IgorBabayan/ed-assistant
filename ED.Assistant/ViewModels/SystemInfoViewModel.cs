using System.Collections.ObjectModel;

namespace ED.Assistant.ViewModels;

public partial class SystemInfoViewModel : BaseViewModel
{
	[ObservableProperty]
	private SystemInfoViewModel? _currentSystem = default;

	[ObservableProperty]
	private CommanderViewModel? _commander = default;

	[ObservableProperty]
	private ObservableCollection<SignalViewModel>? _signals = default;

	[ObservableProperty]
	private ObservableCollection<RecentEventViewModel>? _recentEvents = default;
}
