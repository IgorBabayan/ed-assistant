using ED.Assistant.Domain.Events;

namespace ED.Assistant.Application.State;

public interface IJournalStateApplier
{
	Task ApplyFromFilesAsync(JournalState state, IEnumerable<string> filePaths,
		CancellationToken cancellationToken = default);

	Task ApplyFromLinesAsync(JournalState state, IAsyncEnumerable<string> lines,
		CancellationToken cancellationToken = default);
}
