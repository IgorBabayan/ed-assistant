namespace ED.Assistant.Application.JournalLoading;

public interface IJournalWatchService : IDisposable
{
	bool IsRunning { get; }
	Task StartAsync(string logFolder, CancellationToken cancellationToken = default);
	Task StopAsync();
}
