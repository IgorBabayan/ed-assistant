namespace ED.Assistant.Data.Services.Path;

public class WindowsPathResolver : IPlatformPathResolver
{
    public string GetLogsPath()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return System.IO.Path.Combine(localAppData, "Saved Games", "Frontier Developments", "Elite Dangerous");
    }
}
