using System.IO;

namespace ED.Assistant.Application.Path;

public class WindowsPathResolver : IPlatformPathResolver
{
    public string GetLogsPath()
    {
        var profileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return IOPath.Combine(profileFolder, "Saved Games", "Frontier Developments", "Elite Dangerous");
    }

	public string GetConfigPath()
    {
		var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var configFolderPath = IOPath.Combine(localAppData, "ED Assistant");
        if (!Directory.Exists(configFolderPath))
            Directory.CreateDirectory(configFolderPath);

        return IOPath.Combine(configFolderPath, "config.json");
	}
}
