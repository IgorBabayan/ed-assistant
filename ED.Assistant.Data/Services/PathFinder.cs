namespace ED.Assistant.Data.Services;

public interface IPathFinder
{
    string GetPathToLogs();
}

class PathFinder : IPathFinder
{
    public string GetPathToLogs()
    {
        if (OperatingSystem.IsWindows())
            return WindowsPathToLog();

        if (OperatingSystem.IsLinux())
            return LinuxPathToLog();

        if (OperatingSystem.IsMacOS())
            return MacOSPathToLog();

        throw new NotImplementedException();
    }

    public static string WindowsPathToLog()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(localAppData, "Saved Games", "Frontier Developments", "Elite Dangerous");
    }

    public static string LinuxPathToLog() => throw new NotImplementedException();

    public static string MacOSPathToLog() => throw new NotImplementedException();
}
