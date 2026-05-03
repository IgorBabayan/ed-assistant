namespace ED.Assistant.Data.Services.Path;

public interface IPathFinder
{
    string GetPathToLogs();
	string GetConfigPath();
}

class PathFinder : IPathFinder
{
    private readonly IPlatformPathResolver _resolver;

    public PathFinder(IPlatformPathResolver resolver) => _resolver = resolver;

    public string GetPathToLogs() => _resolver.GetLogsPath();

    public string GetConfigPath() => _resolver.GetConfigPath();
}
