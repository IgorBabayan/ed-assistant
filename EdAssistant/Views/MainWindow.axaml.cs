namespace EdAssistant.Views;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();
    
    public ContentControl NavigationHost 
    {
        get
        {
            var mainView = this.FindControl<MainView>("MainView");
            return mainView?.FindControl<ContentControl>("PART_NavigationContent") 
                   ?? throw new InvalidOperationException(Localization.Instance["MainWindow.Exceptions.NavigationNotFound"]);
        }
    }
}
