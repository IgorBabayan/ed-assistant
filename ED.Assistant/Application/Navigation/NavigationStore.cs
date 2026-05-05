namespace ED.Assistant.Application.Navigation;

partial class NavigationStore : BaseViewModel, INavigationStore
{
	[ObservableProperty]
	public partial LoadableViewModel? CurrentViewModel { get; set; } = default;
}