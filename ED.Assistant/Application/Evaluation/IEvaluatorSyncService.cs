namespace ED.Assistant.Application.Evaluation;

public interface IEvaluatorSyncService
{
    /// <summary>Raised (on a background thread) after a sync actually wrote rows.</summary>
    event EventHandler? DataChanged;

    Task SyncAsync(JournalState state, CancellationToken cancellationToken = default);
}