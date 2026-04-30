using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ED.Assistant.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Services.DialogService;

public interface IDialogService
{
	Task<TResult?> ShowDialogAsync<TViewModel, TResult>(TViewModel viewModel)
		   where TViewModel : ViewModelBase;
}

class DialogService : IDialogService
{
	private readonly IServiceProvider _serviceProvider;

	public DialogService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

	public async Task<TResult?> ShowDialogAsync<TViewModel, TResult>(TViewModel viewModel)
		where TViewModel : ViewModelBase
	{
		var owner = GetMainWindow();
		Window dialog = viewModel switch
		{
			ConfirmDialogViewModel => _serviceProvider.GetRequiredService<ConfirmDialogWindow>(),
			SettingsViewModel => _serviceProvider.GetRequiredService<SettingsWindow>(),

			_ => throw new InvalidOperationException($"No dialog registered for {typeof(TViewModel).Name}")
		};

		dialog.DataContext = viewModel;
		dialog.Width = owner.Bounds.Width;
		dialog.Height = owner.Bounds.Height;
		dialog.Position = owner.Position;
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
