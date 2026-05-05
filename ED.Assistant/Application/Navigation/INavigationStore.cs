namespace ED.Assistant.ViewModels;

public interface INavigationStore
{
	LoadableViewModel? CurrentViewModel { get; set; }
}
