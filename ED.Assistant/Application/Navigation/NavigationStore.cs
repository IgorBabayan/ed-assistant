namespace ED.Assistant.Application.Navigation;

internal partial class NavigationStore : BaseViewModel, INavigationStore
{
	[ObservableProperty]
	public partial LoadableViewModel? CurrentViewModel { get; set; }
}