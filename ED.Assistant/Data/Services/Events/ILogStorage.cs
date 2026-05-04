using System.Globalization;
using System.IO;
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

		var state = new JournalState
		{
			FileName = latestDayLogs!.Count == 1
				? IOPath.GetFileName(latestDayLogs!.First())
				: $"{IOPath.GetFileName(latestDayLogs!.First())} (+{latestDayLogs!.Count - 1})"
		};
		return await ParseDataAsync(state, latestDayLogs!, cancellationToken);
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

	private static List<string>? GetLatestDayLogs(string logFolder)
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

	private static async Task<JournalState> ParseDataAsync(JournalState state, IEnumerable<string> latestDayLogs,
		CancellationToken cancellationToken = default)
	{
		var dispatcher = new JournalEventDispatcher();
		var aggregator = new JournalStateAggregator(dispatcher);

		dispatcher.OnAny(e => state.LastEvent = e);

		aggregator.RegisterLast<CommanderEvent>(CommanderEvent.EventName, e => state.Commander = e);
		aggregator.RegisterLast<LoadGameEvent>(LoadGameEvent.EventName, e => state.LoadGame = e);
		aggregator.RegisterLast<MaterialsEvent>(MaterialsEvent.EventName, e => state.Materials = e);
		aggregator.RegisterLast<RankEvent>(RankEvent.EventName, e => state.Ranks = e);
		aggregator.RegisterLast<ShipLockerEvent>(ShipLockerEvent.EventName, e => state.ShipLocker = e);
		aggregator.RegisterLast<FSDJumpEvent>(FSDJumpEvent.EventName, e =>
		{
			state.FSDJump = e;
			state.Scans.Clear();
			state.FSSSignals.Clear();
		});

		aggregator.RegisterByKey<ScanEvent, int>(ScanEvent.EventName, e => e.BodyId, state.Scans);
		aggregator.RegisterByKey<FSSBodySignalsEvent, int>(FSSBodySignalsEvent.EventName, e => e.BodyId, state.FSSSignals);
		aggregator.RegisterByKey<BaryCentreEvent, int>(BaryCentreEvent.EventName, e => e.BodyId,
			state.BaryCentres);

		await dispatcher.DispatchAsync(ReadLinesFromFilesAsync(latestDayLogs, cancellationToken),
			cancellationToken);

		return state;
	}
}