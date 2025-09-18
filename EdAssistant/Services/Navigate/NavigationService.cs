namespace EdAssistant.Services.Navigate;

sealed class NavigationService(IServiceProvider serviceProvider, ILogger<NavigationService> logger) : INavigationService
{
    private readonly Dictionary<Type, Type> _viewModelToViewMapping = new();
    private readonly Stack<NavigationHistoryItem> _navigationHistory = new();
    private ContentControl _navigationHost = null!;
    
    public event EventHandler<NavigationEventArgs>? Navigated;
    public event EventHandler<NavigationEventArgs>? Navigating;
    
    public bool CanGoBack => _navigationHistory.Count > 1;
    
    public void Initialize(ContentControl navigationHost) => 
        _navigationHost = navigationHost ?? throw new ArgumentNullException(nameof(navigationHost));

    public void RegisterMapping<TViewModel, TView>()
        where TViewModel : BaseViewModel
        where TView : UserControl =>
        _viewModelToViewMapping[typeof(TViewModel)] = typeof(TView);
    
    public void RegisterMapping(Type viewModelType, Type viewType)
    {
        if (!typeof(UserControl).IsAssignableFrom(viewType))
            throw new ArgumentException(Localization.Instance["NavigationService.Exceptions.ViewMustInheritUserControl"], nameof(viewType));
        
        _viewModelToViewMapping[viewModelType] = viewType;
    }
    
    public async Task NavigateAsync<TViewModel>() where TViewModel : BaseViewModel
        => await NavigateAsync<TViewModel>(null);
    
    public async Task NavigateAsync<TViewModel>(object? parameter) where TViewModel : BaseViewModel 
        => await NavigateAsync(typeof(TViewModel), parameter);

    public async Task NavigateAsync(Type viewModelType) => await NavigateAsync(viewModelType, null);
    
    public async Task NavigateAsync(Type viewModelType, object? parameter)
    {
        if (_navigationHost is null)
            throw new InvalidOperationException(Localization.Instance["NavigationService.Exceptions.NavigationServiceNotInitialized"]);

        try
        {
            logger.LogInformation(Localization.Instance["NavigationService.Navigating.NavigatingTo"], viewModelType.Name);
            var navigatingArgs = new NavigationEventArgs
            {
                ViewModelType = viewModelType,
                Parameter = parameter
            };
            Navigating?.Invoke(this, navigatingArgs);
            
            if (navigatingArgs.Cancel)
            {
                logger.LogInformation(Localization.Instance["NavigationService.Navigating.NavigationWasCanceled"], viewModelType.Name);
                return;
            }

            // Show loading indicator
            await ShowLoadingIndicatorAsync();

            var viewModel = serviceProvider.GetRequiredService(viewModelType);
            navigatingArgs.ViewModel = viewModel;

            if (viewModel is IAsyncInitializable asyncInitializable)
            {
                if (!asyncInitializable.IsInitialized)
                {
                    logger.LogDebug(Localization.Instance["NavigationService.Navigating.InitializingViewModel"], viewModelType.Name);
                    await asyncInitializable.InitializeAsync();
                }
            }

            var view = CreateView(viewModelType, viewModel);
            // Navigate to the view on UI thread
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                _navigationHost.Content = view;
                _navigationHistory.Push(new()
                {
                    ViewModelType = viewModelType,
                    ViewModel = viewModel,
                    View = view,
                    Parameter = parameter
                });
            });

            // Hide loading indicator
            await HideLoadingIndicatorAsync();
            Navigated?.Invoke(this, navigatingArgs);
           
            logger.LogInformation(Localization.Instance["NavigationService.Navigating.SuccessfullyNavigated"], viewModelType.Name);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["NavigationService.Exceptions.FailedToNavigate"], viewModelType.Name);
            await HideLoadingIndicatorAsync();
            throw;
        }
    }
    
    public async Task GoBackAsync()
    {
        if (!CanGoBack)
        {
            logger.LogWarning(Localization.Instance["NavigationService.Warnings.CannotGoBack"]);
            return;
        }

        try
        {
            var currentItem = _navigationHistory.Pop();
            if (currentItem.ViewModel is IDisposable disposable)
            {
                disposable.Dispose();
            }

            var previousItem = _navigationHistory.Peek();
            logger.LogInformation(Localization.Instance["NavigationService.Navigating.GoingBackTo"], previousItem.ViewModelType.Name);

            // Navigate back on UI thread
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                _navigationHost.Content = previousItem.View;
            });

            Navigated?.Invoke(this, new NavigationEventArgs
            {
                ViewModelType = previousItem.ViewModelType,
                ViewModel = previousItem.ViewModel,
                Parameter = previousItem.Parameter
            });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["NavigationService.Exceptions.FailedToNavigateBack"]);
            throw;
        }
    }
    
    public void ClearHistory()
    {
        logger.LogInformation(Localization.Instance["NavigationService.Navigating.ClearingNavigationHistory"]);
        
        while (_navigationHistory.Count > 0)
        {
            var item = _navigationHistory.Pop();
            if (item.ViewModel is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
    
    private UserControl CreateView(Type viewModelType, object viewModel)
    {
        if (!_viewModelToViewMapping.TryGetValue(viewModelType, out var viewType))
        {
            throw new InvalidOperationException(string.Format(Localization.Instance["NavigationService.Exceptions.NoViewMappingFound"], 
                viewModelType.Name));
        }

        var view = (UserControl)serviceProvider.GetRequiredService(viewType);
        view.DataContext = viewModel;
        
        return view;
    }
    
    private async Task ShowLoadingIndicatorAsync()
    {
        // Implement loading indicator logic here for Avalonia
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            //! TODO: Double-check
            // logger.LogDebug("Showing loading indicator");
        });
    }
    
    private async Task HideLoadingIndicatorAsync()
    {
        // Implement loading indicator hiding logic here for Avalonia
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            //! TODO: Double-check
            //_logger.LogDebug("Hiding loading indicator");
        });
    }
}

class NavigationHistoryItem
{
    public required Type ViewModelType { get; init; }
    public required object ViewModel { get; init; }
    public required UserControl View { get; init; }
    public object? Parameter { get; init; }
}