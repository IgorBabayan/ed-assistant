using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using ED.Assistant.Application.Dialog;
using ED.Assistant.Application.Path;
using ED.Assistant.Application.Settings;
using ED.Assistant.Domain.Config;
using ED.Assistant.Plugins;

namespace ED.Assistant.Presentation.ViewModels.Settings;

public partial class SettingsViewModel : BaseViewModel
{
	private readonly IFolderPickerService _folderPickerService;
	private readonly ISettingsStorage _settingsStorage;
	private readonly IPathFinder _pathFinder;
	private readonly IPluginCatalog _pluginCatalog;
	private readonly IPluginUninstaller _pluginUninstaller;
	
	private Dictionary<string, bool> _savedAddonStates = new();

	[ObservableProperty]
	public partial string? LogFolder { get; set; } = string.Empty;

	[ObservableProperty]
	public partial bool EnableAutoWatch { get; set; }
	
	[ObservableProperty]
	public partial bool HideExcludedSignals { get; set; }
	
	[ObservableProperty]
	public partial decimal? ReadLogsForDays { get; set; } = AppSettings.DefaultReadLogsForDays;

	[ObservableProperty]
	public partial DockPosition DockPosition { get; set; }

	public IReadOnlyList<DockPosition> DockPositions { get; } = Enum.GetValues<DockPosition>();

	public ObservableCollection<AddonItemViewModel> Addons { get; } = [];

	public bool HasAddons => Addons.Count > 0;

	public bool HasPendingAddonChanges => Addons.Any(a => a.RequiresRestart);

	public string AddonsFolder => _pluginCatalog.Root;

	public event Action<bool?>? CloseRequested;

	public SettingsViewModel(IPathFinder pathFinder, IFolderPickerService folderPickerService,
		ISettingsStorage settingsStorage, IPluginCatalog pluginCatalog, IPluginUninstaller pluginUninstaller)
	{
		_pathFinder = pathFinder;
		_folderPickerService = folderPickerService;
		_settingsStorage = settingsStorage;
		_pluginCatalog = pluginCatalog;
		_pluginUninstaller = pluginUninstaller;
	}

	public async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		var settings = await _settingsStorage.LoadAsync(_pathFinder.GetConfigPath(), cancellationToken);
		LogFolder = string.IsNullOrWhiteSpace(settings.LogFolder) ? _pathFinder.GetPathToLogs() : settings.LogFolder;
		EnableAutoWatch = settings.IsAutoWatchEnable;
		HideExcludedSignals = settings.HideExcludedSignals;
		ReadLogsForDays = settings.ReadLogsForDays;
		DockPosition = settings.DockPosition;

		_savedAddonStates = new Dictionary<string, bool>(settings.Addons);
		LoadAddons(settings);
	}

	protected override void OnDispose()
	{
		foreach (var addon in Addons)
			addon.PropertyChanged -= OnAddonChanged;

		base.OnDispose();
	}

	[RelayCommand]
	private async Task Save(CancellationToken cancellationToken = default)
	{
		var path = _pathFinder.GetConfigPath();
		await _settingsStorage.SaveAsync(path, new AppSettings
		{ 
			LogFolder = LogFolder,
			IsAutoWatchEnable = EnableAutoWatch,
			HideExcludedSignals = HideExcludedSignals,
			ReadLogsForDays = ReadLogsForDays is { } days
				? (int)Math.Clamp(days, 0, int.MaxValue)
				: AppSettings.DefaultReadLogsForDays,
			DockPosition = DockPosition,
			Addons = BuildAddonStates()
		}, cancellationToken);

		CloseRequested?.Invoke(true);
	}

	[RelayCommand]
	private void Cancel() => CloseRequested?.Invoke(false);

	[RelayCommand]
	private async Task OpenFolder(Window? owner)
	{
		var folder = await _folderPickerService.PickFolderAsync("Select Elite Dangerous log folder", owner);
		if (folder is not null)
		{
			LogFolder = folder;
		}
	}

	[RelayCommand]
	private async Task OpenAddonsFolder(Window? owner)
	{
		var directory = Directory.CreateDirectory(_pluginCatalog.Root);
		await _folderPickerService.PickFolderAsync("Select ED Assistant plugin folder", owner);
	}

	// Rebuilt on every open, so toggles from a cancelled dialog are discarded
	private void LoadAddons(AppSettings settings)
	{
		foreach (var addon in Addons)
			addon.PropertyChanged -= OnAddonChanged;

		Addons.Clear();

		foreach (var plugin in _pluginCatalog.Installed)
		{
			var item = new AddonItemViewModel(plugin, settings.IsAddonEnabled(plugin.Descriptor.Key), RemoveAddonAsync);
			item.PropertyChanged += OnAddonChanged;
			Addons.Add(item);
		}

		OnPropertyChanged(nameof(HasAddons));
		OnPropertyChanged(nameof(HasPendingAddonChanges));
	}
	
	private async Task RemoveAddonAsync(AddonItemViewModel item)
	{
		try
		{
			var result = await _pluginUninstaller.RemoveAsync(item.Plugin);

			// The addon is gone (or will be), so its enabled flag shouldn't stay in settings
			_savedAddonStates.Remove(item.Key);

			if (result == PluginRemovalResult.Removed)
			{
				item.PropertyChanged -= OnAddonChanged;
				Addons.Remove(item);
				OnPropertyChanged(nameof(HasAddons));
			}
			else
			{
				item.IsRemovalPending = true;
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine($"Removing addon '{item.Key}' failed: {ex}");
			item.RemoveError = ex.Message;
		}

		OnPropertyChanged(nameof(HasPendingAddonChanges));
	}

	private void OnAddonChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(AddonItemViewModel.IsEnabled) or nameof(AddonItemViewModel.IsRemovalPending))
			OnPropertyChanged(nameof(HasPendingAddonChanges));
	}
	
	private Dictionary<string, bool> BuildAddonStates()
	{
		var states = new Dictionary<string, bool>(_savedAddonStates);
		foreach (var addon in Addons)
		{
			if (addon.IsRemovalPending)
				states.Remove(addon.Key);
			else
				states[addon.Key] = addon.IsEnabled;
		}

		return states;
	}
}
