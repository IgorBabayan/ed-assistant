namespace ED.Assistant.Domain.Config;

public class AppSettings
{
	public const int DefaultReadLogsForDays = 5;
	
	[JsonPropertyName(nameof(LogFolder))]
	public string? LogFolder { get; set; }

	[JsonPropertyName(nameof(IsAutoWatchEnable))]
	public bool IsAutoWatchEnable { get; set; }

	[JsonPropertyName(nameof(HideExcludedSignals))]
	public bool HideExcludedSignals { get; set; }
	
	[JsonPropertyName(nameof(ReadLogsForDays))]
	public int ReadLogsForDays { get; set; } = DefaultReadLogsForDays;
	
	[JsonPropertyName("DockPosition")]
	[JsonConverter(typeof(JsonStringEnumConverter<DockPosition>))]
	public DockPosition DockPosition { get; set; } = DockPosition.Bottom;
	
	[JsonPropertyName(nameof(Addons))]
	public Dictionary<string, bool> Addons { get; set; } = new();
	
	[JsonPropertyName(nameof(AutoUpdate))]
	public bool AutoUpdate { get; set; }

	public bool IsAddonEnabled(string key) => !Addons.TryGetValue(key, out var enabled) || enabled;
}
