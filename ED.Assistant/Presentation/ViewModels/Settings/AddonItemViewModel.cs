using ED.Assistant.Plugins;

namespace ED.Assistant.Presentation.ViewModels.Settings;

public sealed partial class AddonItemViewModel : ObservableObject
{
	private readonly PluginLoadStatus _loadStatus;
	private readonly string? _error;

	public string Key { get; }
	public string Name { get; }
	public string Version { get; }
	public string? Description { get; }
	public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Status), nameof(RequiresRestart), nameof(HasError))]
	public partial bool IsEnabled { get; set; }

	/// <summary>The saved choice differs from what is running right now.</summary>
	public bool RequiresRestart =>
		_loadStatus != PluginLoadStatus.Failed && IsEnabled != (_loadStatus == PluginLoadStatus.Loaded);

	public bool HasError => IsEnabled && _loadStatus == PluginLoadStatus.Failed;

	public string Status => (IsEnabled, _loadStatus) switch
	{
		(true, PluginLoadStatus.Loaded) => "Loaded",
		(true, PluginLoadStatus.Failed) => $"Failed to load: {_error}",
		(true, _) => "Will be loaded after restart",
		(false, PluginLoadStatus.Loaded) => "Will be unloaded after restart",
		_ => "Disabled"
	};

	public AddonItemViewModel(InstalledPlugin plugin, bool isEnabled)
	{
		Key = plugin.Descriptor.Key;
		Name = plugin.Descriptor.Name;
		Version = plugin.Descriptor.Version;
		Description = plugin.Descriptor.Description;
		_loadStatus = plugin.Status;
		_error = plugin.Error;
		IsEnabled = isEnabled;
	}
}
