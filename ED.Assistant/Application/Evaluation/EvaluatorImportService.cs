using ED.Assistant.Application.Storage;
using ED.Assistant.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using EvaluatorEntity = ED.Assistant.Data.Evaluator.Evaluator;

namespace ED.Assistant.Application.Evaluation;

internal sealed class EvaluatorImportService : IEvaluatorImportService
{
	// LogStorage treats 0 days as "every journal file in the folder"
	private const int AllLogs = 0;

	private readonly ILogStorage _logStorage;
	private readonly IEvaluatorSyncService _evaluatorSync;
	private readonly IServiceScopeFactory _scopeFactory;

	public event EventHandler<EvaluatorImportResult>? Imported;

	public EvaluatorImportService(ILogStorage logStorage, IEvaluatorSyncService evaluatorSync,
		IServiceScopeFactory scopeFactory)
	{
		_logStorage = logStorage;
		_evaluatorSync = evaluatorSync;
		_scopeFactory = scopeFactory;
	}

	public async Task<EvaluatorImportResult> ImportAsync(string logFolder,
		CancellationToken cancellationToken = default)
	{
		EvaluatorImportResult result;

		try
		{
			var before = await CountUnsoldAsync(cancellationToken);

			await Task.Run(async () =>
			{
				var state = await _logStorage.LoadLastLogsAsync(logFolder, AllLogs, cancellationToken);
				await _evaluatorSync.SyncAsync(state, cancellationToken);
			}, cancellationToken);

			var after = await CountUnsoldAsync(cancellationToken);
			result = new EvaluatorImportResult(logFolder, Math.Max(0, after - before));
		}
		catch (Exception ex) when (ex is not OperationCanceledException)
		{
			// DirectoryNotFound / "Journal files not found." from LogStorage
			result = new EvaluatorImportResult(logFolder, 0, ex.Message);
		}

		Imported?.Invoke(this, result);
		return result;
	}

	private async Task<int> CountUnsoldAsync(CancellationToken cancellationToken)
	{
		using var scope = _scopeFactory.CreateScope();
		var repository = scope.ServiceProvider.GetRequiredService<IRepository<EvaluatorEntity>>();

		// IsActive query filter = unsold only
		return await repository.AsNoTracking().CountAsync(cancellationToken);
	}
}