namespace EdAssistant.Views;

public partial class MainView : UserControl
{
    private MainWindow MainWindow => (this.GetVisualRoot() as MainWindow)!;

    public MainView() => InitializeComponent();

    private void OnMinimizeClick(object? sender, RoutedEventArgs e) =>
        MainWindow.WindowState = MainWindow.WindowState == WindowState.Minimized
            ? WindowState.Normal
            : WindowState.Minimized;

    private void OnMaximizeClick(object? sender, RoutedEventArgs e) => 
        MainWindow.WindowState = MainWindow.WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;

    private void OnCloseClick(object? sender, RoutedEventArgs e) => Environment.Exit(0);

    private void OnPointPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            MainWindow.BeginMoveDrag(e);
        }
    }

    private void OnDoubleTapped(object? sender, TappedEventArgs e) =>
        MainWindow.WindowState = MainWindow.WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
}
