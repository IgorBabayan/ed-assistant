using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace ED.Assistant.Extensions;

public static class Utils
{
	public static Window GetMainWindow()
	{
		if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
			&& desktop.MainWindow is not null)
		{
			return desktop.MainWindow;
		}

		throw new InvalidOperationException("MainWindow not found.");
	}
}
