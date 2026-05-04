using ED.Assistant.Data.Services.Events;

namespace ED.Assistant.Tests;

[TestClass]
public class LogStorageTests
{
	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Throw_When_LogFolder_Is_Null()
	{
		//var journalAplier = new Mock<IJournalStateApplier>();
		var journalAplier = new JournalStateApplier();
		var storage = new LogStorage(journalAplier);
		await Assert.ThrowsAsync<ArgumentNullException>(() => storage.LoadLastLogsAsync(null!));
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Throw_When_LogFolder_Is_Empty()
	{
		var journalAplier = new JournalStateApplier();
		var storage = new LogStorage(journalAplier);
		await Assert.ThrowsAsync<ArgumentNullException>(() => storage.LoadLastLogsAsync(""));
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Throw_When_LogFolder_Does_Not_Exist()
	{
		var journalAplier = new JournalStateApplier();
		var storage = new LogStorage(journalAplier);
		await Assert.ThrowsAsync<DirectoryNotFoundException>(() => storage.LoadLastLogsAsync("not-existing-folder"));
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Throw_When_Journal_Files_Not_Found()
	{
		var folder = CreateTempFolder();

		try
		{
			var journalAplier = new JournalStateApplier();
			var storage = new LogStorage(journalAplier);
			await Assert.ThrowsAsync<Exception>(() => storage.LoadLastLogsAsync(folder));
		}
		finally
		{
			Directory.Delete(folder, true);
		}
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Read_All_Journal_Files_From_Latest_Day()
	{
		using var temp = new TempDirectory();

		await File.WriteAllTextAsync(Path.Combine(temp.Path, "Journal.2026-04-29T100000.01.log"),
			"""
        { "timestamp":"2026-04-29T10:00:00Z", "event":"Commander", "FID":"F1", "Name":"OLD_DAY" }
        """, TestContext.CancellationToken);

		await File.WriteAllTextAsync(Path.Combine(temp.Path, "Journal.2026-04-30T080000.01.log"),
			"""
        { "timestamp":"2026-04-30T08:00:00Z", "event":"Commander", "FID":"F2", "Name":"FIRST_FILE" }
        """, TestContext.CancellationToken);

		await File.WriteAllTextAsync(Path.Combine(temp.Path, "Journal.2026-04-30T120000.01.log"),
			"""
        { "timestamp":"2026-04-30T12:00:00Z", "event":"Commander", "FID":"F3", "Name":"SECOND_FILE" }
        """, TestContext.CancellationToken);

		var journalAplier = new JournalStateApplier();
		var storage = new LogStorage(journalAplier);

		// Act
		var state = await storage.LoadLastLogsAsync(temp.Path);

		// Assert
		Assert.IsNotNull(state.Commander);
		Assert.AreEqual("SECOND_FILE", state.Commander.Name);
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Read_Latest_Day_Files_In_DateTime_Order()
	{
		// Arrange
		using var temp = new TempDirectory();

		await File.WriteAllTextAsync(Path.Combine(temp.Path, "Journal.2026-04-30T180000.01.log"),
			"""
        { "timestamp":"2026-04-30T18:00:00Z", "event":"Commander", "FID":"F3", "Name":"LAST" }
        """, TestContext.CancellationToken);

		await File.WriteAllTextAsync(Path.Combine(temp.Path, "Journal.2026-04-30T080000.01.log"),
			"""
        { "timestamp":"2026-04-30T08:00:00Z", "event":"Commander", "FID":"F1", "Name":"FIRST" }
        """, TestContext.CancellationToken);

		await File.WriteAllTextAsync(Path.Combine(temp.Path, "Journal.2026-04-30T120000.01.log"),
			"""
        { "timestamp":"2026-04-30T12:00:00Z", "event":"Commander", "FID":"F2", "Name":"MIDDLE" }
        """, TestContext.CancellationToken);

		var journalAplier = new JournalStateApplier();
		var storage = new LogStorage(journalAplier);

		// Act
		var state = await storage.LoadLastLogsAsync(temp.Path);

		// Assert
		Assert.IsNotNull(state.Commander);
		Assert.AreEqual("LAST", state.Commander.Name);
	}

	private static string CreateTempFolder()
	{
		var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
		Directory.CreateDirectory(folder);
		return folder;
	}

	internal sealed class TempDirectory : IDisposable
	{
		public string Path { get; } = System.IO.Path.Combine(System.IO.Path.GetTempPath(),
			Guid.NewGuid().ToString("N"));

		public TempDirectory() => Directory.CreateDirectory(Path);

		public void Dispose()
		{
			if (Directory.Exists(Path))
				Directory.Delete(Path, recursive: true);
		}
	}

	public TestContext TestContext { get; set; }
}
