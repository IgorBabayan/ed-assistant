namespace ED.Assistant.Data.Models.Config;

public class AppSettings
{
	[JsonPropertyName("Logfolder")]
	public string? LogFolder { get; set; }
}
