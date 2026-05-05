namespace ED.Assistant.Application.Navigation;

public interface INavigationStore
{
	LoadableViewModel? CurrentViewModel { get; set; }
}
