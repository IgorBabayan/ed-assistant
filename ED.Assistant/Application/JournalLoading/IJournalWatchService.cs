namespace ED.Assistant.Application.JournalLoading;

public interface IJournalWatchService : IDisposable
{
	Task StartAsync(string logFolder, CancellationToken cancellationToken = default);
	void Stop();
}
