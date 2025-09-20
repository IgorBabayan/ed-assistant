namespace EdAssistant.Services.Desktop;

class DesktopService(ILogger<DesktopService> logger) : IDesktopService
{
    private readonly StringBuilder _builder = new();

    public async Task Save()
    {
        var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var path = Path.Combine(homeFolder, ".local", "share", "applications");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        path = Path.Combine(path, "EDAssistant.desktop");
        if (File.Exists(path))
            File.Delete(path);
        
        await using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        await using var writer = new StreamWriter(fileStream);
        await writer.WriteLineAsync(_builder.ToString());

        try
        {
            await RunCommand("chmod", $"+x \"{path}\"");
            await RunCommand("update-desktop-database", Path.GetDirectoryName(path)!);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, Localization.Instance["Exceptions.CannotMakeDesktopFileExecutable"]);
        }
    }

    public async Task CreateDesktopFile()
    {
        var executablePath = Environment.ProcessPath;
        var iconPath = Path.Combine(Path.GetDirectoryName(executablePath)!, "logo.png");

        await ResourceHelper.SaveResourceToFileAsync("avares://EdAssistant/Assets/logo.png", iconPath);
        
        _builder.Clear();
        _builder.AppendLine("[Desktop Entry]")
            .AppendLine("Name=Elite Dangerous assistant")
            .AppendLine("Comment=Elite Dangerous assistant tool")
            .AppendLine($"Version={Assembly.GetExecutingAssembly().GetName().Version}")
            .AppendLine($"Exec=dotnet {EscapePathForDesktopFile(executablePath!)}.dll")
            .AppendLine($"Icon={EscapePathForDesktopFile(iconPath)}")
            .AppendLine("Terminal=false")
            .AppendLine("Type=Application")
            .AppendLine("StartupNotify=true")
            .AppendLine("Categories=Game;Utility;")
            .AppendLine("Keywords=elite;dangerous;gaming;assistant;");
    }

    private static async Task RunCommand(string command, string arguments)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        await process.WaitForExitAsync();
    }
    
    private static string EscapePathForDesktopFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return path;

        return path.Replace(" ", @"\ ")
            .Replace("(", @"\(")
            .Replace(")", @"\)")
            .Replace("&", @"\&")
            .Replace(";", @"\;")
            .Replace("'", @"\'")
            .Replace("\"", "\\\"")
            .Replace("`", @"\`")
            .Replace("$", @"\$");
    }
}