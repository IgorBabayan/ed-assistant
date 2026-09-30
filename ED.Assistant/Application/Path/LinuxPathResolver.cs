using System.IO;
using System.Text.RegularExpressions;

namespace ED.Assistant.Application.Path;

public partial class LinuxPathResolver : IPlatformPathResolver
{
	private const string AppFolder = "ed-assistant";
	private const string EliteAppId = "359320";
	
	private static readonly string Home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

	// Source-generated instead of Regex.Match(string, pattern) (SYSLIB1045)
	[GeneratedRegex("^\\s*\"path\"\\s*\"(.+)\"")]
	private static partial Regex LibraryPathRegex();
	
    public string GetLogsPath()
    {
	    foreach (var steamRoot in GetSteamRoots())
	    {
		    foreach (var library in GetLibraryFolders(steamRoot))
		    {
			    var path = BuildJournalPath(library);
			    if (Directory.Exists(path))
				    return path;
		    }    
	    }
	    
	    return BuildJournalPath(IOPath.Combine(Home, ".local", "share", "Steam"));
    }

    public string GetConfigPath()
	{
		var configHome = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		var configFolder = IOPath.Combine(configHome, AppFolder);
		Directory.CreateDirectory(configFolder);

		return IOPath.Combine(configFolder, "config.json");
	}
    
	private static string BuildJournalPath(string steamLibrary) =>
		IOPath.Combine(steamLibrary, "steamapps", "compatdata", EliteAppId, "pfx", "drive_c",
			"users", "steamuser", "Saved Games", "Frontier Developments", "Elite Dangerous");
	
	private static IEnumerable<string> GetSteamRoots()
	{
		string[] candidates =
		[
			IOPath.Combine(Home, ".steam", "steam"),
			IOPath.Combine(Home, ".local", "share", "Steam"),
			IOPath.Combine(Home, ".var", "app", "com.valvesoftware.Steam", ".local", "share", "Steam"),
			IOPath.Combine(Home, "snap", "steam", "common", ".local", "share", "Steam")
		];

		return candidates.Where(Directory.Exists);
	}

	private static IEnumerable<string> GetLibraryFolders(string steamRoot)
	{
		yield return steamRoot;

		var vdf = IOPath.Combine(steamRoot, "steamapps", "libraryfolders.vdf");
		if (!File.Exists(vdf))
			yield break;

		foreach (var line in File.ReadLines(vdf))
		{
			var match = LibraryPathRegex().Match(line);
			if (match.Success)
				yield return match.Groups[1].Value;
		}
	}
}
