using ED.Assistant.Data.Models.Config;
using ED.Assistant.Data.Services.Settings;

namespace ED.Assistant.Tests;

[TestClass]
public sealed class StorageTest
{
	[TestMethod]
	public async Task SaveAsync_Should_Create_Settings_File()
	{
		var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
		Directory.CreateDirectory(tempDir);

		var filePath = Path.Combine(tempDir, "config.json");
		var storage = new SettingsStorage();
		var settings = new AppSettings
		{
			LogFolder = "test-folder"
		};

		try
		{
			await storage.SaveAsync(filePath, settings);
			Assert.IsTrue(File.Exists(filePath));
		}
		finally
		{
			Directory.Delete(tempDir, recursive: true);
		}
	}
}
