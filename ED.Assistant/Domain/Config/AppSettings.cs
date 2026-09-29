namespace ED.Assistant.Domain.Config;

public class AppSettings
{
	public const int DEFAULT_READ_LOGS_FOR_DAYS = 5;
	
	[JsonPropertyName(nameof(LogFolder))]
	public string? LogFolder { get; set; }

	[JsonPropertyName(nameof(IsAutoWatchEnable))]
	public bool IsAutoWatchEnable { get; set; }

	[JsonPropertyName(nameof(HideExcludedSignals))]
	public bool HideExcludedSignals { get; set; }
	
	[JsonPropertyName(nameof(ReadLogsForDays))]
	public int ReadLogsForDays { get; set; } = DEFAULT_READ_LOGS_FOR_DAYS;
	
	[JsonPropertyName("DockPosition")]
	[JsonConverter(typeof(JsonStringEnumConverter<DockPosition>))]
	public DockPosition DockPosition { get; set; } = DockPosition.Bottom;
}
