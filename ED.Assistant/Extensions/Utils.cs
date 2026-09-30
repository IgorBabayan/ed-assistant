using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace ED.Assistant.Extensions;

public static class Utils
{
	public static Window GetMainWindow()
	{
		if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
		    {
			    MainWindow: not null
		    } desktop)
		{
			return desktop.MainWindow;
		}

		throw new InvalidOperationException("MainWindow not found.");
	}
}
