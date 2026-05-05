using System.IO;

namespace ED.Assistant.Data.Seed.Path;

sealed class DbPathProvider : IDbPathProvider
{
	private const string AppName = "ED Assistant";
	private const string DbName = "bio-samples.db";

	public string GetBioSamplesDbPath()
	{
		var appData = GetAppDataDirectory();
		Directory.CreateDirectory(appData);

		return IOPath.Combine(appData, DbName);
	}

	public string GetSeedPath() => IOPath.Combine(AppContext.BaseDirectory, "Seed");

	public string GetSeedDataBasePath() => IOPath.Combine(GetSeedPath(), "DataBase");

	public bool BioSamplesDbExists() => File.Exists(GetBioSamplesDbPath());

	private static string GetAppDataDirectory()
	{
		if (OperatingSystem.IsWindows())
		{
			return IOPath.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				AppName);
		}

		if (OperatingSystem.IsMacOS())
		{
			return IOPath.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				"Library", "Application Support", AppName);
		}

		return IOPath.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
			".local", "share", AppName);
	}
}