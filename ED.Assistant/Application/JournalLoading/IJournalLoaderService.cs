namespace ED.Assistant.Application.JournalLoading;

public interface IJournalLoaderService
{
	Task LoadLastLogsAsync(CancellationToken cancellationToken = default);
}
