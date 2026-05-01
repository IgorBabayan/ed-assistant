using ED.Assistant.Services.DialogService;
using ED.Assistant.Services.Navigation;
using ED.Assistant.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Extensions;

internal static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterViewModels(this IServiceCollection services)
	{
		services.AddSingleton<MainWindowViewModel>()
			.AddSingleton<ConfirmDialogViewModel>()
			.AddSingleton<SettingsViewModel>()
			.AddSingleton<DashboardViewModel>();
		return services;
	}

	public static IServiceCollection RegisterServices(this IServiceCollection services)
	{
		services.AddSingleton<IDialogService, DialogService>()
			.AddSingleton<IFolderPickerService, FolderPickerService>()
			.AddSingleton<INavigationStore, NavigationStore>()
			.AddSingleton<INavigationService, NavigationService>();
		return services;
	}

	public static IServiceCollection RegisterWindows(this IServiceCollection services)
	{
		services.AddTransient<ConfirmDialogWindow>()
			.AddTransient<SettingsWindow>();
		return services;
	}
}
