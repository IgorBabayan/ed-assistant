namespace ED.Assistant.Application.Storage;

public interface ILogStorage
{
	Task<JournalState> LoadLastLogsAsync(string logFolder, int days, CancellationToken cancellationToken = default);
}
