namespace ED.Assistant.Application.Evaluation;

public interface IEvaluatorSyncService
{
    Task SyncAsync(JournalState state, CancellationToken cancellationToken = default);
}