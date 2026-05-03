using ED.Assistant.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Services.Navigation;

public interface INavigationService
{
	Task NavigateToAsync<TViewModel>(CancellationToken cancellationToken = default)
		where TViewModel : LoadableViewModel;
}

class NavigationService : INavigationService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly INavigationStore _navigationStore;

	public NavigationService(
		IServiceProvider serviceProvider,
		INavigationStore navigationStore)
	{
		_serviceProvider = serviceProvider;
		_navigationStore = navigationStore;
	}

	public async Task NavigateToAsync<TViewModel>(CancellationToken cancellationToken = default)
		where TViewModel : LoadableViewModel
	{
		var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
		_navigationStore.CurrentViewModel = viewModel;

		if (viewModel is INavigationAware navigationAware)
			await navigationAware.OnNavigatedToAsync(cancellationToken);
	}
}