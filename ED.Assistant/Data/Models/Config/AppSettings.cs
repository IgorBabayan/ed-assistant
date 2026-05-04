namespace ED.Assistant.Data.Models.Config;

public class AppSettings
{
	[JsonPropertyName("Logfolder")]
	public string? LogFolder { get; set; }

	[JsonPropertyName("IsAutoWatchEnable")]
	public bool IsAutoWatchEnable { get; set; }
}
