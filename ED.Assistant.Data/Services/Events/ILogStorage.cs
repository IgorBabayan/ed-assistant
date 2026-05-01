using ED.Assistant.Data.Models.Events;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using IOPath = System.IO.Path;

namespace ED.Assistant.Data.Services.Events;

public interface ILogStorage
{
	Task LoadAllLogs(string logFolder, CancellationToken cancellationToken = default);
	Task LoadLastLogs(string logFolder, CancellationToken cancellationToken = default);
}

class LogStorage : ILogStorage
{
	public async Task LoadAllLogs(string logFolder, CancellationToken cancellationToken = default)
	{
		EnsureLogFolderExists(logFolder);

		var files = Directory.GetFiles(logFolder, "Journal.*.log")
			.Select(path =>
			{
				var fileName = IOPath.GetFileNameWithoutExtension(path);

				// Journal.2026-04-30T082047.01
				var parts = fileName.Split('.');

				// parts[1] = 2026-04-30T082047
				var datePart = parts[1];

				var date = DateTime.ParseExact(
					datePart,
					"yyyy-MM-ddTHHmmss",
					CultureInfo.InvariantCulture
				);

				return new
				{
					Path = path,
					date.Date
				};
			})
			.GroupBy(x => x.Date)
			.ToDictionary(
				g => g.Key,
				g => g.Select(x => x.Path).ToList()
			);
		
	}

	public async Task LoadLastLogs(string logFolder, CancellationToken cancellationToken = default)
	{
		EnsureLogFolderExists(logFolder);

		var latestDayLogs = GetLatestDayLogs(logFolder);
		if (latestDayLogs?.Any() == false)
			throw new Exception("Log files not found");

		await ParseData(latestDayLogs!, cancellationToken);
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

	private static async Task ParseData(IEnumerable<string> latestDayLogs, CancellationToken cancellationToken = default)
	{
		CommanderEvent? commander = null;
		await foreach (var line in ReadLinesFromFilesAsync(latestDayLogs!, cancellationToken))
		{
			if (line.Contains(CommanderEvent.EventNameJSON))
			{
				var obj = JsonSerializer.Deserialize<CommanderEvent>(line);
				if (obj is not null)
					commander = obj;
			}

			//! TODO: add other implementations for other events we want to track, e.g. FSDJump, Docked, Undocked, etc.
		}
	}
}