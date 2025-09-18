namespace EdAssistant.Services.Navigate;

public interface INavigationService
{
    Task NavigateAsync<TViewModel>() where TViewModel : BaseViewModel;
    Task NavigateAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel;
    Task NavigateAsync(Type viewModelType);
    Task NavigateAsync(Type viewModelType, object parameter);
    void Initialize(ContentControl navigationHost);

    void RegisterMapping<TViewModel, TView>()
        where TViewModel : BaseViewModel
        where TView : UserControl;

    void RegisterMapping(Type viewModelType, Type viewType);
    bool CanGoBack { get; }
    Task GoBackAsync();
    void ClearHistory();
    event EventHandler<NavigationEventArgs> Navigated;
    event EventHandler<NavigationEventArgs> Navigating;
}