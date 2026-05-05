namespace ED.Assistant.ViewModels;

partial class NavigationStore : BaseViewModel, INavigationStore
{
	[ObservableProperty]
	public partial LoadableViewModel? CurrentViewModel { get; set; } = default;
}