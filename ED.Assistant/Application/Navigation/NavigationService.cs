using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Application.Navigation;

internal class NavigationService : INavigationService
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
        cancellationToken.ThrowIfCancellationRequested();
		
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        var previous = _navigationStore.CurrentViewModel;

        // The hidden view model stops updating and only marks itself dirty
        if (previous is not null && !ReferenceEquals(previous, viewModel))
            previous.OnNavigatedFrom();

        _navigationStore.CurrentViewModel = viewModel;

        await viewModel.OnNavigatedToAsync(cancellationToken);
    }
}