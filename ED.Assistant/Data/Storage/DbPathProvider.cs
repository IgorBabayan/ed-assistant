using System.IO;

namespace ED.Assistant.Data.Storage;

internal sealed class DbPathProvider : IDbPathProvider
{
	private const string AppFolder = "ED Assistant";
	private const string DbFileName = "bio-samples.db";

	public string GetDatabasePath()
	{
		var directory = GetDatabaseDirectory();

		// CreateDirectory is a no-op when the folder already exists
		Directory.CreateDirectory(directory);

		return IOPath.Combine(directory, DbFileName);
	}

	public string GetDatabaseDirectory()
	{
		if (OperatingSystem.IsWindows())
		{
			return IOPath.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
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
