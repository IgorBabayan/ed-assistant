using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ED.Assistant.ViewModels;

namespace ED.Assistant.Services.DialogService;

public interface IDialogService
{
	Task<TResult?> ShowDialogAsync<TViewModel, TResult>(TViewModel viewModel)
		   where TViewModel : ViewModelBase;
}

class DialogService : IDialogService
{

	public DialogService()
	{
		
	}

	public async Task<TResult?> ShowDialogAsync<TViewModel, TResult>(TViewModel viewModel)
		where TViewModel : ViewModelBase
	{
		var owner = GetMainWindow();
		Window dialog = viewModel switch
		{
			ConfirmDialogViewModel => new ConfirmDialogWindow(),
			SettingsViewModel => new SettingsWindow(),

			_ => throw new InvalidOperationException($"No dialog registered for {typeof(TViewModel).Name}")
		};

		dialog.DataContext = viewModel;
		return await dialog.ShowDialog<TResult?>(owner);
	}

	private static Window GetMainWindow()
	{
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
			&& desktop.MainWindow is not null)
		{
			return desktop.MainWindow;
		}

		throw new InvalidOperationException("MainWindow not found.");
	}
}
