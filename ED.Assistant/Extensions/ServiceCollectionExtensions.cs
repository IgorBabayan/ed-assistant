using ED.Assistant.Services.DialogService;
using ED.Assistant.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Extensions;

internal static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterViewModels(this IServiceCollection services)
	{
		services.AddSingleton<MainWindowViewModel>()
			.AddSingleton<IConfirmDialogViewModel, ConfirmDialogViewModel>()
			.AddSingleton<ISettingsViewModel, SettingsViewModel>();
		return services;
	}

	public static IServiceCollection RegisterServices(this IServiceCollection services)
	{
		services.AddSingleton<IDialogService, DialogService>();
		return services;
	}
}
