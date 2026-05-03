using ED.Assistant.Data.Services.Events;
using ED.Assistant.Data.Services.Path;
using ED.Assistant.Data.Services.Settings;
using ED.Assistant.Services.DialogService;
using ED.Assistant.Services.Journal;
using ED.Assistant.Services.Navigation;
using ED.Assistant.Services.SystemBuilder;
using ED.Assistant.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Extensions;

static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterDataServices(this IServiceCollection services)
	{
		services.AddSingleton<WindowsPathResolver>();
		services.AddSingleton<LinuxPathResolver>();
		services.AddSingleton<MacPathResolver>();

		services.AddSingleton<IPlatformPathResolver>(sp =>
		{
			if (OperatingSystem.IsWindows())
				return sp.GetRequiredService<WindowsPathResolver>();

			if (OperatingSystem.IsLinux())
				return sp.GetRequiredService<LinuxPathResolver>();

			if (OperatingSystem.IsMacOS())
				return sp.GetRequiredService<MacPathResolver>();

			throw new PlatformNotSupportedException("Unsupported OS");
		});
		
		return services;
	}

	public static IServiceCollection RegisterViewModels(this IServiceCollection services)
	{
		services.AddSingleton<MainWindowViewModel>()
			.AddSingleton<ConfirmDialogViewModel>()
			.AddSingleton<SettingsViewModel>()
			.AddSingleton<DashboardViewModel>()
			.AddSingleton<SystemViewModel>()
			.AddSingleton<SignalsViewModel>()
			.AddSingleton<JournalViewModel>()
			.AddSingleton<MaterialItemViewModel>()
			.AddSingleton<MaterialViewModel>()
			.AddSingleton<ShipLockerViewModel>();
		return services;
	}

	public static IServiceCollection RegisterServices(this IServiceCollection services)
	{
		services.AddSingleton<IPathFinder, PathFinder>()
			.AddSingleton<ISettingsStorage, SettingsStorage>()
			.AddSingleton<ILogStorage, LogStorage>()
			.AddSingleton<IDialogService, DialogService>()
			.AddSingleton<IFolderPickerService, FolderPickerService>()
			.AddSingleton<IJournalStateStore, JournalStateStore>()
			.AddSingleton<INavigationStore, NavigationStore>()
			.AddSingleton<INavigationService, NavigationService>()
			.AddSingleton<ISystemStructureBuilder, SystemStructureBuilder>();
		return services;
	}

	public static IServiceCollection RegisterWindows(this IServiceCollection services)
	{
		services.AddTransient<ConfirmDialogWindow>()
			.AddTransient<SettingsWindow>();
		return services;
	}
}
