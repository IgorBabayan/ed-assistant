namespace EdAssistant.ViewModels.Pages;

public abstract partial class PageViewModel(ILogger<PageViewModel>? logger = null) : BaseViewModel, IAsyncInitializable
{
    private readonly object _initializationLock = new();
    
    [ObservableProperty]
    private bool _isInitialized;

    [ObservableProperty]
    private bool _isInitializing;
    
    public async Task InitializeAsync()
    {
        lock (_initializationLock)
        {
            if (IsInitialized || IsInitializing)
                return;
            
            IsInitializing = true;
        }

        try
        {
            logger?.LogDebug(Localization.Instance["Navigating.Initializing"], GetType().Name);
            await OnInitializeAsync();
            IsInitialized = true;
            logger?.LogDebug(Localization.Instance["Navigating.SuccessfullyInitialized"], GetType().Name);
        }
        catch (Exception exception)
        {
            logger?.LogError(exception, Localization.Instance["Navigating.FailedToInitialize"], GetType().Name);
            throw;
        }
        finally
        {
            IsInitializing = false;
        }
    }
    
    protected virtual async Task OnInitializeAsync() => await Task.CompletedTask;
    
    public virtual async Task OnNavigatedFromAsync() => await Task.CompletedTask;
    
    public virtual async Task OnNavigatedToAsync(object? parameter = null) => await Task.CompletedTask;

    protected override void OnDispose(bool disposing)
    {
        if (disposing)
        {
            logger?.LogDebug(Localization.Instance["Navigating.Disposing"], GetType().Name);
        }
        
        base.OnDispose(disposing);
    }
}

public sealed partial class MarketConnectorViewModel : PageViewModel { }
public sealed partial class LogViewModel : PageViewModel { }