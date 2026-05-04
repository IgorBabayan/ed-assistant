using System.Globalization;
using System.IO;
using IOPath = System.IO.Path;

namespace ED.Assistant.Data.Services.Events;

public interface ILogStorage
{
	Task<JournalState> LoadLastLogsAsync(string logFolder, CancellationToken cancellationToken = default);
}

class LogStorage : ILogStorage
{
	private readonly IJournalStateApplier _journalStateApplier;

	public LogStorage(IJournalStateApplier journalStateApplier) => _journalStateApplier = journalStateApplier;

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

		await _journalStateApplier.ApplyFromFilesAsync(state, latestDayLogs, cancellationToken);
		return state;
	}

	private static void EnsureLogFolderExists(string logFolder)
	{
		if (string.IsNullOrWhiteSpace(logFolder))
			throw new ArgumentNullException(nameof(logFolder));

		if (!Directory.Exists(logFolder))
			throw new DirectoryNotFoundException($"The specified log folder '{logFolder}' does not exist.");
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
}