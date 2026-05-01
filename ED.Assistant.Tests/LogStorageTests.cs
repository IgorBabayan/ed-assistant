using ED.Assistant.Data.Models.Events;
using ED.Assistant.Data.Services;
using ED.Assistant.Data.Services.Events;

namespace ED.Assistant.Tests;

[TestClass]
public class LogStorageTests
{
	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Throw_When_LogFolder_Is_Null()
	{
		var storage = new LogStorage(new FakeJournalEventDispatcher());
		await Assert.ThrowsAsync<ArgumentNullException>(() => storage.LoadLastLogsAsync(null!));
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Throw_When_LogFolder_Is_Empty()
	{
		var storage = new LogStorage(new FakeJournalEventDispatcher());
		await Assert.ThrowsAsync<ArgumentNullException>(() => storage.LoadLastLogsAsync(""));
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Throw_When_LogFolder_Does_Not_Exist()
	{
		var storage = new LogStorage(new FakeJournalEventDispatcher());
		await Assert.ThrowsAsync<DirectoryNotFoundException>(() => storage.LoadLastLogsAsync("not-existing-folder"));
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Throw_When_Journal_Files_Not_Found()
	{
		var folder = CreateTempFolder();

		try
		{
			var storage = new LogStorage(new FakeJournalEventDispatcher());
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
		var folder = CreateTempFolder();

		try
		{
			await File.WriteAllLinesAsync(Path.Combine(folder, "Journal.2026-04-30T082047.01.log"),
				["old-day-line"]);

			await File.WriteAllLinesAsync(Path.Combine(folder, "Journal.2026-05-01T060000.01.log"),
				["latest-day-line-1"]);

			await File.WriteAllLinesAsync(Path.Combine(folder, "Journal.2026-05-01T070000.01.log"),
				["latest-day-line-2"]);

			var dispatcher = new FakeJournalEventDispatcher();
			var storage = new LogStorage(dispatcher);

			await storage.LoadLastLogsAsync(folder);

			CollectionAssert.AreEqual(new[] { "latest-day-line-1", "latest-day-line-2" },
				dispatcher.Lines);
		}
		finally
		{
			Directory.Delete(folder, true);
		}
	}

	[TestMethod]
	public async Task LoadLastLogsAsync_Should_Read_Latest_Day_Files_In_DateTime_Order()
	{
		var folder = CreateTempFolder();

		try
		{
			await File.WriteAllLinesAsync(Path.Combine(folder, "Journal.2026-05-01T070000.01.log"),
				["second"]);

			await File.WriteAllLinesAsync(Path.Combine(folder, "Journal.2026-05-01T060000.01.log"),
				["first"]);

			var dispatcher = new FakeJournalEventDispatcher();
			var storage = new LogStorage(dispatcher);

			await storage.LoadLastLogsAsync(folder);
			CollectionAssert.AreEqual(new[] { "first", "second" }, dispatcher.Lines);
		}
		finally
		{
			Directory.Delete(folder, true);
		}
	}

	private static string CreateTempFolder()
	{
		var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
		Directory.CreateDirectory(folder);
		return folder;
	}

	private sealed class FakeJournalEventDispatcher : IJournalEventDispatcher
	{
		public List<string> Lines { get; } = [];

		public void On<TEvent>(string eventName, Action<TEvent> handler)
			where TEvent : IJournalEvent { }

		public async Task DispatchAsync(IAsyncEnumerable<string> lines, CancellationToken cancellationToken = default)
		{
			await foreach (var line in lines.WithCancellation(cancellationToken))
			{
				Lines.Add(line);
			}
		}
	}
}
