namespace ED.Assistant.ViewModels;

public interface INavigationStore
{
	ViewModelBase? CurrentViewModel { get; set; }
}

partial class NavigationStore : ObservableObject, INavigationStore
{
	[ObservableProperty]
	private ViewModelBase? currentViewModel = default;
}