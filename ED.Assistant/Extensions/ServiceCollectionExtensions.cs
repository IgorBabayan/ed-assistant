using ED.Assistant.Services.DialogService;
using ED.Assistant.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Extensions;

internal static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterViewModels(this IServiceCollection services)
	{
		services.AddSingleton<MainWindowViewModel>()
			.AddSingleton<ConfirmDialogViewModel>()
			.AddSingleton<SettingsViewModel>();
		return services;
	}

	public static IServiceCollection RegisterServices(this IServiceCollection services)
	{
		services.AddSingleton<IDialogService, DialogService>()
			.AddSingleton<IFolderPickerService, FolderPickerService>();
		return services;
	}

	public static IServiceCollection RegisterWindows(this IServiceCollection services)
	{
		services.AddTransient<ConfirmDialogWindow>()
			.AddTransient<SettingsWindow>();
		return services;
	}
}
