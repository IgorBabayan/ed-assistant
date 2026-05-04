using ED.Assistant.Data.Models.Events;
using ED.Assistant.Data.Services.Events;
using Moq;

namespace ED.Assistant.Tests;

[TestClass]
public sealed class LogStorageTests
{
	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Throw_When_LogFolder_Is_Null()
	{
		var applier = new Mock<IJournalStateApplier>();
		var storage = new LogStorage(applier.Object);

		await Assert.ThrowsAsync<ArgumentNullException>(
			() => storage.LoadLastLogsAsync(null!, TestContext.CancellationToken));

		applier.Verify(
			x => x.ApplyFromFilesAsync(
				It.IsAny<JournalState>(),
				It.IsAny<IEnumerable<string>>(),
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Throw_When_LogFolder_Is_Empty()
	{
		var applier = new Mock<IJournalStateApplier>();
		var storage = new LogStorage(applier.Object);

		await Assert.ThrowsAsync<ArgumentNullException>(
			() => storage.LoadLastLogsAsync("", TestContext.CancellationToken));

		applier.Verify(
			x => x.ApplyFromFilesAsync(
				It.IsAny<JournalState>(),
				It.IsAny<IEnumerable<string>>(),
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Throw_When_LogFolder_Does_Not_Exist()
	{
		var applier = new Mock<IJournalStateApplier>();
		var storage = new LogStorage(applier.Object);

		await Assert.ThrowsAsync<DirectoryNotFoundException>(
			() => storage.LoadLastLogsAsync("not-existing-folder", TestContext.CancellationToken));

		applier.Verify(
			x => x.ApplyFromFilesAsync(
				It.IsAny<JournalState>(),
				It.IsAny<IEnumerable<string>>(),
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Throw_When_Journal_Files_Not_Found()
	{
		using var temp = new TempDirectory();

		var applier = new Mock<IJournalStateApplier>();
		var storage = new LogStorage(applier.Object);

		await Assert.ThrowsAsync<Exception>(
			() => storage.LoadLastLogsAsync(temp.Path, TestContext.CancellationToken));

		applier.Verify(
			x => x.ApplyFromFilesAsync(
				It.IsAny<JournalState>(),
				It.IsAny<IEnumerable<string>>(),
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Pass_Only_Latest_Day_Files_To_Applier()
	{
		using var temp = new TempDirectory();

		var oldFile = Path.Combine(temp.Path, "Journal.2026-04-29T100000.01.log");
		var firstLatestFile = Path.Combine(temp.Path, "Journal.2026-04-30T080000.01.log");
		var secondLatestFile = Path.Combine(temp.Path, "Journal.2026-04-30T120000.01.log");

		await File.WriteAllTextAsync(oldFile, "{}", TestContext.CancellationToken);
		await File.WriteAllTextAsync(firstLatestFile, "{}", TestContext.CancellationToken);
		await File.WriteAllTextAsync(secondLatestFile, "{}", TestContext.CancellationToken);

		var applier = new Mock<IJournalStateApplier>();

		var storage = new LogStorage(applier.Object);

		await storage.LoadLastLogsAsync(temp.Path, TestContext.CancellationToken);

		applier.Verify(
			x => x.ApplyFromFilesAsync(
				It.Is<JournalState>(s =>
					s.FileName == "Journal.2026-04-30T080000.01.log (+1)"),
				It.Is<IEnumerable<string>>(files =>
					files.SequenceEqual(new[]
					{
						firstLatestFile,
						secondLatestFile
					})),
				TestContext.CancellationToken),
			Times.Once);
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Pass_Latest_Day_Files_In_DateTime_Order()
	{
		using var temp = new TempDirectory();

		var lastFile = Path.Combine(temp.Path, "Journal.2026-04-30T180000.01.log");
		var firstFile = Path.Combine(temp.Path, "Journal.2026-04-30T080000.01.log");
		var middleFile = Path.Combine(temp.Path, "Journal.2026-04-30T120000.01.log");

		await File.WriteAllTextAsync(lastFile, "{}", TestContext.CancellationToken);
		await File.WriteAllTextAsync(firstFile, "{}", TestContext.CancellationToken);
		await File.WriteAllTextAsync(middleFile, "{}", TestContext.CancellationToken);

		var applier = new Mock<IJournalStateApplier>();
		var storage = new LogStorage(applier.Object);

		await storage.LoadLastLogsAsync(temp.Path, TestContext.CancellationToken);

		applier.Verify(
			x => x.ApplyFromFilesAsync(
				It.IsAny<JournalState>(),
				It.Is<IEnumerable<string>>(files =>
					files.SequenceEqual(new[]
					{
						firstFile,
						middleFile,
						lastFile
					})),
				TestContext.CancellationToken),
			Times.Once);
	}

	private sealed class TempDirectory : IDisposable
	{
		public string Path { get; } =
			System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N"));

		public TempDirectory() => Directory.CreateDirectory(Path);

		public void Dispose()
		{
			if (Directory.Exists(Path))
				Directory.Delete(Path, recursive: true);
		}
	}

	public TestContext TestContext { get; set; } = null!;
}
