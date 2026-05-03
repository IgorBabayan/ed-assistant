namespace ED.Assistant.Data.Services.Path;

public class LinuxPathResolver : IPlatformPathResolver
{
    public string GetLogsPath() => throw new NotImplementedException();
	public string GetConfigPath() => throw new NotImplementedException();
}
