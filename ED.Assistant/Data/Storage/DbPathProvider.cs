using System.IO;
using System.Runtime.InteropServices;

namespace ED.Assistant.Data.Storage;

sealed class DbPathProvider : IDbPathProvider
{
	private const string AppFolder = "ED Assistant";
	private const string DbFileName = "bio-samples.db";

	public string GetDatabasePath()
	{
		var directory = GetDatabaseDirectory();

		if (!Directory.Exists(directory))
			Directory.CreateDirectory(directory);

		return Path.Combine(directory, DbFileName);
	}

	public string GetDatabaseDirectory()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				AppFolder);
		}

		if (OperatingSystem.IsMacOS())
		{
			return IOPath.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
				"Library", "Application Support", AppFolder);
		}

		if (OperatingSystem.IsLinux())
		{
			return IOPath.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				".local", "share", AppFolder);
		}

		return IOPath.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				AppFolder);
	}
}
