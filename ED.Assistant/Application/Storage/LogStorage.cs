using System.Globalization;
using System.IO;

namespace ED.Assistant.Application.Storage;

class LogStorage : ILogStorage
{
	private readonly IJournalStateApplier _journalStateApplier;

	public LogStorage(IJournalStateApplier journalStateApplier) => _journalStateApplier = journalStateApplier;

	public async Task<JournalState> LoadLastLogsAsync(string logFolder, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(logFolder))
			throw new ArgumentNullException(nameof(logFolder));

		if (!Directory.Exists(logFolder))
		{
			throw new DirectoryNotFoundException(
				$"Journal folder '{logFolder}' does not exist.");
		}

		var files = Directory
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
			.OrderBy(file => file.Time)
			.ThenBy(file => file.Path, StringComparer.Ordinal)
			.Select(file => file.Path)
			.ToArray();

		if (files.Length == 0)
		{
			throw new InvalidOperationException(
				"Journal files not found.");
		}

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
}