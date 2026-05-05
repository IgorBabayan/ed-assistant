namespace ED.Assistant.Application.Path;

public class LinuxPathResolver : IPlatformPathResolver
{
    public string GetLogsPath() => throw new NotImplementedException();
	public string GetConfigPath() => throw new NotImplementedException();
}
