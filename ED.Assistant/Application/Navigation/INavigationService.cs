namespace ED.Assistant.Application.Navigation;

public interface INavigationService
{
	Task NavigateToAsync<TViewModel>(CancellationToken cancellationToken = default)
		where TViewModel : LoadableViewModel;
}
