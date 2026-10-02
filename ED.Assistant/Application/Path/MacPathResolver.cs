using System.IO;

namespace ED.Assistant.Application.Path;

public class MacPathResolver : IPlatformPathResolver
{
	private const string AppFolder = "ED Assistant";
	private const string DefaultBottleName = "Elite Dangerous";
	private const string DefaultBottleUser = "crossover";

	private static readonly string Home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

	public string GetLogsPath()
	{
		var defaultPath = BuildJournalPath(DefaultBottleName, DefaultBottleUser);
		if (Directory.Exists(defaultPath))
			return defaultPath;

		foreach (var path in GetCrossOverJournalPaths())
		{
			if (Directory.Exists(path))
				return path;
		}

		return defaultPath;
	}

	public string GetConfigPath()
	{
		var appSupport = IOPath.Combine(Home, "Library", "Application Support");
		var configFolder = IOPath.Combine(appSupport, AppFolder);
		if (!Directory.Exists(configFolder))
			Directory.CreateDirectory(configFolder);

		return IOPath.Combine(configFolder, "config.json");
	}

	private static string GetCrossOverBottlesRoot() =>
		IOPath.Combine(Home, "Library", "Application Support", "CrossOver", "Bottles");

	private static string BuildJournalPath(string bottleName, string bottleUser) =>
		IOPath.Combine(GetCrossOverBottlesRoot(), bottleName, "drive_c", "users", bottleUser,
			"Saved Games", "Frontier Developments", "Elite Dangerous");

	// Falls back to scanning every CrossOver bottle/user pair in case the user
	// renamed the bottle or it was created under a different Windows user.
	private static IEnumerable<string> GetCrossOverJournalPaths()
	{
		var bottlesRoot = GetCrossOverBottlesRoot();
		if (!Directory.Exists(bottlesRoot))
			yield break;

		foreach (var bottle in Directory.EnumerateDirectories(bottlesRoot))
		{
			var usersRoot = IOPath.Combine(bottle, "drive_c", "users");
			if (!Directory.Exists(usersRoot))
				continue;

			foreach (var user in Directory.EnumerateDirectories(usersRoot))
			{
				yield return IOPath.Combine(user, "Saved Games", "Frontier Developments", "Elite Dangerous");
			}
		}
	}
}