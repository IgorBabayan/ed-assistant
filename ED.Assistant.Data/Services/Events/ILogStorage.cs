using ED.Assistant.Data.Models.Events;
using System.Globalization;
using System.Runtime.CompilerServices;
using IOPath = System.IO.Path;

namespace ED.Assistant.Data.Services.Events;

public interface ILogStorage
{
	Task<JournalState> LoadLastLogsAsync(string logFolder, CancellationToken cancellationToken = default);
}

class LogStorage : ILogStorage
{
	public async Task<JournalState> LoadLastLogsAsync(string logFolder, CancellationToken cancellationToken = default)
	{
		EnsureLogFolderExists(logFolder);

		var latestDayLogs = GetLatestDayLogs(logFolder);
		if (latestDayLogs?.Any() == false)
			throw new Exception("Log files not found");

		return await ParseDataAsync(latestDayLogs!, cancellationToken);
	}

	private static void EnsureLogFolderExists(string logFolder)
	{
		if (string.IsNullOrWhiteSpace(logFolder))
			throw new ArgumentNullException(nameof(logFolder));

		if (!Directory.Exists(logFolder))
			throw new DirectoryNotFoundException($"The specified log folder '{logFolder}' does not exist.");
	}

	private static async IAsyncEnumerable<string> ReadLinesFromFilesAsync(IEnumerable<string> filePaths,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		foreach (var filePath in filePaths)
		{
			await using var stream = new FileStream(filePath, new FileStreamOptions
			{
				Mode = FileMode.Open,
				Access = FileAccess.Read,
				Share = FileShare.ReadWrite,
				Options = FileOptions.Asynchronous | FileOptions.SequentialScan
			});
			using var reader = new StreamReader(stream);

			while (await reader.ReadLineAsync(cancellationToken) is { } line)
			{
				yield return line;
			}
		}
	}

	private static IEnumerable<string>? GetLatestDayLogs(string logFolder)
		=> Directory.GetFiles(logFolder, "Journal.*.log")
			.Select(path =>
			{
				var parts = IOPath.GetFileNameWithoutExtension(path).Split('.');

				if (parts.Length < 2)
					return null;

				if (!DateTime.TryParseExact(parts[1], "yyyy-MM-ddTHHmmss", CultureInfo.InvariantCulture,
					DateTimeStyles.None, out var dateTime))
					return null;

				return new
				{
					Path = path,
					DateTime = dateTime,
					Date = DateOnly.FromDateTime(dateTime)
				};
			})
			.Where(x => x is not null)
			.GroupBy(x => x!.Date)
			.OrderByDescending(g => g.Key)
			.FirstOrDefault()?
			.OrderBy(x => x!.DateTime)
			.Select(x => x!.Path)
			.ToList() ?? [];

	private async Task<JournalState> ParseDataAsync(IEnumerable<string> latestDayLogs, CancellationToken cancellationToken = default)
	{
		var state = new JournalState();
		var dispatcher = new JournalEventDispatcher();
		var aggregator = new JournalStateAggregator(dispatcher);

		aggregator.RegisterLast<CommanderEvent>(CommanderEvent.EventName, e => state.Commander = e);
		aggregator.RegisterLast<LoadGameEvent>(LoadGameEvent.EventName, e => state.LoadGame = e);
		aggregator.RegisterLast<MaterialsEvent>(MaterialsEvent.EventName, e => state.Materials = e);
		aggregator.RegisterLast<RankEvent>(RankEvent.EventName, e => state.Ranks = e);
		aggregator.RegisterLast<FSDJumpEvent>(FSDJumpEvent.EventName, e => state.FSDJump = e);

		aggregator.RegisterByKey<ScanEvent, int>(ScanEvent.EventName, e => e.BodyId, state.Scans);
		aggregator.RegisterByKey<BaryCentreEvent, int>(BaryCentreEvent.EventName, e => e.BodyId,
			state.BaryCentres);

		await dispatcher.DispatchAsync(ReadLinesFromFilesAsync(latestDayLogs, cancellationToken),
			cancellationToken);

		return state;
	}
}