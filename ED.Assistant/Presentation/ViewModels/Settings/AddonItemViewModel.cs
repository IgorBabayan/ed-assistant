using ED.Assistant.Plugins;

public sealed partial class AddonItemViewModel : ObservableObject
{
	private readonly PluginLoadStatus _loadStatus;
	private readonly string? _error;
	private readonly Func<AddonItemViewModel, Task> _remove;
	private readonly Func<AddonItemViewModel, Task> _openSettings;
	private readonly bool _hasSettings;

	public InstalledPlugin Plugin { get; }
	public string Key { get; }
	public string Name { get; }
	public string Version { get; }
	public string? Description { get; }
	public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Status), nameof(RequiresRestart), nameof(HasError))]
	public partial bool IsEnabled { get; set; }

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Status), nameof(RequiresRestart), nameof(HasError), nameof(CanRemove),
		nameof(CanOpenSettings), nameof(IsSettingsButtonVisible))]
	public partial bool IsRemovalPending { get; set; }

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsSettingsButtonVisible))]
	public partial bool IsConfirmingRemove { get; set; }

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Status), nameof(HasError))]
	public partial string? RemoveError { get; set; }

	public bool CanRemove => !IsRemovalPending;

	/// <summary>Only a loaded addon can show its settings pages.</summary>
	public bool CanOpenSettings => _hasSettings && !IsRemovalPending;

	// Hidden while "Delete with data?" is shown, to leave room for it
	public bool IsSettingsButtonVisible => CanOpenSettings && !IsConfirmingRemove;

	/// <summary>The saved choice differs from what is running right now.</summary>
	public bool RequiresRestart => IsRemovalPending ||
		(_loadStatus != PluginLoadStatus.Failed && IsEnabled != (_loadStatus == PluginLoadStatus.Loaded));

	public bool HasError => RemoveError is not null ||
		(!IsRemovalPending && IsEnabled && _loadStatus == PluginLoadStatus.Failed);

	public string Status =>
		RemoveError is not null ? $"Remove failed: {RemoveError}"
		: IsRemovalPending ? "Will be removed after restart"
		: (IsEnabled, _loadStatus) switch
		{
			(true, PluginLoadStatus.Loaded) => "Loaded",
			(true, PluginLoadStatus.Failed) => $"Failed to load: {_error}",
			(true, _) => "Will be loaded after restart",
			(false, PluginLoadStatus.Loaded) => "Will be unloaded after restart",
			_ => "Disabled"
		};

	public AddonItemViewModel(InstalledPlugin plugin, bool isEnabled, bool hasSettings,
		Func<AddonItemViewModel, Task> remove, Func<AddonItemViewModel, Task> openSettings)
	{
		Plugin = plugin;
		Key = plugin.Descriptor.Key;
		Name = plugin.Descriptor.Name;
		Version = plugin.Descriptor.Version;
		Description = plugin.Descriptor.Description;
		_loadStatus = plugin.Status;
		_error = plugin.Error;
		_remove = remove;
		_openSettings = openSettings;
		_hasSettings = hasSettings;
		IsEnabled = isEnabled;
		IsRemovalPending = plugin.IsRemovalPending;
	}

	[RelayCommand]
	private Task OpenSettings() => _openSettings(this);

	[RelayCommand]
	private void RequestRemove()
	{
		RemoveError = null;
		IsConfirmingRemove = true;
	}

	[RelayCommand]
	private void CancelRemove() => IsConfirmingRemove = false;

	// AsyncRelayCommand: disabled while running, so a double click can't start two removals
	[RelayCommand]
	private async Task ConfirmRemove()
	{
		IsConfirmingRemove = false;
		await _remove(this);
	}
}