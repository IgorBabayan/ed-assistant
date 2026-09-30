using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Application.Navigation;

class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationStore _navigationStore;

    public NavigationService(IServiceProvider serviceProvider, INavigationStore navigationStore)
    {
        _serviceProvider = serviceProvider;
        _navigationStore = navigationStore;
    }

    public Task NavigateToAsync<TViewModel>(CancellationToken cancellationToken = default)
        where TViewModel : LoadableViewModel
    {
        cancellationToken.ThrowIfCancellationRequested();
        return NavigateToAsync(_serviceProvider.GetRequiredService<TViewModel>(), cancellationToken);
    }

    public async Task NavigateToAsync(LoadableViewModel viewModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(viewModel);
        cancellationToken.ThrowIfCancellationRequested();

        var previous = _navigationStore.CurrentViewModel;

        // Already there: nothing to do
        if (ReferenceEquals(previous, viewModel))
            return;

        // The hidden view model stops updating and only marks itself dirty
        previous?.OnNavigatedFrom();

        _navigationStore.CurrentViewModel = viewModel;

        await viewModel.OnNavigatedToAsync(cancellationToken);
    }
}