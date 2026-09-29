using System.Globalization;
using System.IO;

namespace ED.Assistant.Application.Storage;

class LogStorage : ILogStorage
{
	private readonly IJournalStateApplier _journalStateApplier;

	public LogStorage(IJournalStateApplier journalStateApplier) => _journalStateApplier = journalStateApplier;

	public async Task<JournalState> LoadLastLogsAsync(string logFolder, int days, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(logFolder))
			throw new ArgumentNullException(nameof(logFolder));

		if (!Directory.Exists(logFolder))
		{
			throw new DirectoryNotFoundException(
				$"Journal folder '{logFolder}' does not exist.");
		}

		var allFiles = Directory
			.GetFiles(logFolder, "Journal.*.log")
			.Select(path => (
				Path: path,
				Parts: IOPath
					.GetFileNameWithoutExtension(path)
					.Split('.')))
			.Where(file => file.Parts.Length >= 2)
			.Select(file => (
				file.Path,
				Time: ParseTime(file.Parts[1])))
			.Where(file => file.Time is not null)
			.Select(file => (file.Path, Time: file.Time!.Value))
			.OrderBy(file => file.Time)
			.ThenBy(file => file.Path, StringComparer.Ordinal)
			.ToArray();

		if (allFiles.Length == 0)
		{
			throw new InvalidOperationException(
				"Journal files not found.");
		}

		var files = SelectWindow(allFiles, days);

		var state = new JournalState
		{
			FileName = files.Length == 1
				? IOPath.GetFileName(files[0])
				: $"{IOPath.GetFileName(files[^1])} " +
				  $"(+{files.Length - 1} earlier files)"
		};

		await _journalStateApplier.ApplyFromFilesAsync(
			state,
			files,
			cancellationToken);

		return state;
	}

	private static DateTime? ParseTime(string value)
	{
		return DateTime.TryParseExact(
			value,
			"yyyy-MM-ddTHHmmss",
			CultureInfo.InvariantCulture,
			DateTimeStyles.None,
			out var time)
			? time
			: null;
	}
	
	private static string[] SelectWindow(IReadOnlyList<(string Path, DateTime Time)> files, int days)
	{
		if (days <= 0)
			return files.Select(f => f.Path).ToArray();

		// Journal file names use the game machine's local time
		var cutoff = DateTime.Now.AddDays(-days);

		var first = files.Count;
		for (var i = 0; i < files.Count; i++)
		{
			if (files[i].Time >= cutoff)
			{
				first = i;
				break;
			}
		}

		var start = Math.Max(0, first - 1);
		return files.Skip(start).Select(f => f.Path).ToArray();
	}
}