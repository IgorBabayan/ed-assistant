namespace ED.Assistant.Domain.Config;

public class AppSettings
{
	[JsonPropertyName(nameof(LogFolder))]
	public string? LogFolder { get; set; }

	[JsonPropertyName(nameof(IsAutoWatchEnable))]
	public bool IsAutoWatchEnable { get; set; }

	[JsonPropertyName(nameof(HideExcludedSignals))]
	public bool HideExcludedSignals { get; set; }
}
