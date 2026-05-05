using ED.Assistant.Application.Dialog;
using ED.Assistant.Application.JournalLoading;
using ED.Assistant.Application.Navigation;
using ED.Assistant.Application.Path;
using ED.Assistant.Application.Settings;
using ED.Assistant.Application.State;
using ED.Assistant.Application.Storage;
using ED.Assistant.Domain.System;
using ED.Assistant.Presentation.ViewModels.ConfirmDialog;
using ED.Assistant.Presentation.ViewModels.Dashboard;
using ED.Assistant.Presentation.ViewModels.Exobiology;
using ED.Assistant.Presentation.ViewModels.Journal;
using ED.Assistant.Presentation.ViewModels.Material;
using ED.Assistant.Presentation.ViewModels.ShipLocker;
using ED.Assistant.Presentation.Views.ConfirmDialog;
using ED.Assistant.Presentation.Views.Settings;
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
			.AddSingleton<ExobiologyViewModel>()
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
			.AddSingleton<IJournalLoaderService, JournalLoaderService>()
			.AddSingleton<INavigationStore, NavigationStore>()
			.AddSingleton<INavigationService, NavigationService>()
			.AddSingleton<IJournalStateApplier, JournalStateApplier>()
			.AddSingleton<IJournalWatchService, JournalWatchService>()
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
