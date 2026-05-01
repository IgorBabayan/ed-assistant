namespace ED.Assistant.ViewModels;

public interface INavigationStore
{
	BaseViewModel? CurrentViewModel { get; set; }
}

partial class NavigationStore : ObservableObject, INavigationStore
{
	[ObservableProperty]
	private BaseViewModel? currentViewModel = default;
}