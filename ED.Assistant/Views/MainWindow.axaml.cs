using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace ED.Assistant.Views;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    private void OnPointerPressed(object? sender, PointerPressedEventArgs args)
    {
        if (!args.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;

        if (args.ClickCount == 2)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;

            return;
        }

        if (args.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(args);
        }
    }

	private void OnMinimizeClick(object? sender, RoutedEventArgs args) => WindowState = WindowState.Minimized;

	private void OnMaximizeClick(object? sender, RoutedEventArgs args) => WindowState = WindowState == WindowState.Maximized
        ? WindowState.Normal
        : WindowState.Maximized;

	private void OnCloseClick(object? sender, RoutedEventArgs args)
	{
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.TryShutdown();
		}
	}
}