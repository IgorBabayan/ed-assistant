namespace ED.Assistant.Application.Path;

public interface IPlatformPathResolver
{
    string GetLogsPath();
    string GetConfigPath();
}
