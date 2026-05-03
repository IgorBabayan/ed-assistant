namespace ED.Assistant.ViewModels;

public interface INavigationStore
{
	LoadableViewModel? CurrentViewModel { get; set; }
}

partial class NavigationStore : ObservableObject, INavigationStore
{
	[ObservableProperty]
	private LoadableViewModel? currentViewModel = default;
}