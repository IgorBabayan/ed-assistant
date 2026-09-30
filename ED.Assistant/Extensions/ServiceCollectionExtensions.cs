using ED.Assistant.Application.Catalog;
using ED.Assistant.Application.Dialog;
using ED.Assistant.Application.Evaluation;
using ED.Assistant.Application.Linux;
using ED.Assistant.Application.Navigation;
using ED.Assistant.Application.Path;
using ED.Assistant.Application.Settings;
using ED.Assistant.Application.Storage;
using ED.Assistant.Data.Repository;
using ED.Assistant.Data.Storage;
using ED.Assistant.Domain.System;
using ED.Assistant.Presentation.ViewModels.ConfirmDialog;
using ED.Assistant.Presentation.ViewModels.Dashboard;
using ED.Assistant.Presentation.ViewModels.Evaluator;
using ED.Assistant.Presentation.ViewModels.Exobiology;
using ED.Assistant.Presentation.ViewModels.Journal;
using ED.Assistant.Presentation.ViewModels.Material;
using ED.Assistant.Presentation.ViewModels.Settings;
using ED.Assistant.Presentation.ViewModels.Shell;
using ED.Assistant.Presentation.ViewModels.ShipLocker;
using ED.Assistant.Presentation.ViewModels.System;
using ED.Assistant.Presentation.Views.ConfirmDialog;
using ED.Assistant.Presentation.Views.Import;
using ED.Assistant.Presentation.Views.Settings;
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
			.AddSingleton<MaterialViewModel>()
			.AddSingleton<EvaluatorViewModel>()
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
			.AddSingleton<IDbPathProvider, DbPathProvider>()
			.AddSingleton<IGenusCatalog, GenusCatalog>()
			.AddSingleton<IEvaluatorSyncService, EvaluatorSyncService>()
			.AddSingleton<IEvaluatorImportService, EvaluatorImportService>()
			.AddSingleton<ISystemStructureBuilder, SystemStructureBuilder>();

		if (OperatingSystem.IsLinux())
		{
			services.AddSingleton<IDesktopService, DesktopService>();
		}
		else
		{
			services.AddSingleton<IDesktopService, NullDesktopService>();
		}
		return services;
	}

	public static IServiceCollection RegisterWindows(this IServiceCollection services)
	{
		services.AddTransient<ConfirmDialogWindow>()
			.AddTransient<ImportFolderWindow>()
			.AddTransient<SettingsWindow>();
		return services;
	}

	public static IServiceCollection RegisterDbServices(this IServiceCollection services)
	{
		services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		
		return services;
	}
}