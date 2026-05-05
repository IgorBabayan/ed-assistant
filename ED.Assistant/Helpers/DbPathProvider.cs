using System.IO;

namespace ED.Assistant.Helpers;

static class DbPathProvider
{
	public static string GetDatabasePath()
	{
		var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
			"ED Assistant");
		if (!Directory.Exists(folder))
			Directory.CreateDirectory(folder);

		return Path.Combine(folder, "bio-samples.db");
	}
}