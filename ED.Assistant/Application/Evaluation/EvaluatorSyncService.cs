using System.Diagnostics;
using ED.Assistant.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using EvaluatorEntity = ED.Assistant.Data.Evaluator.Evaluator;

namespace ED.Assistant.Application.Evaluation;

sealed class EvaluatorSyncService : IEvaluatorSyncService
{
	private const int FirstFootStepMultiplier = 5;

	private readonly IServiceScopeFactory _scopeFactory;
	private readonly SemaphoreSlim _gate = new(1, 1);

	public EvaluatorSyncService(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

	public async Task SyncAsync(JournalState state, CancellationToken cancellationToken = default)
	{
		var changes = state.PendingEvaluatorChanges.ToArray();
		state.PendingEvaluatorChanges.Clear();

		if (changes.Length == 0)
			return;

		await _gate.WaitAsync(cancellationToken);
		try
		{
			using var scope = _scopeFactory.CreateScope();
			var services = scope.ServiceProvider;

			var genusRepository = services.GetRequiredService<IRepository<Genus>>();
			var evaluatorRepository = services.GetRequiredService<IRepository<EvaluatorEntity>>();
			var unitOfWork = services.GetRequiredService<IUnitOfWork>();

			var catalog = (await genusRepository.ListAsync(cancellationToken: cancellationToken))
				.GroupBy(g => g.CodexName, StringComparer.OrdinalIgnoreCase)
				.ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

			foreach (var change in changes)
			{
				switch (change)
				{
					case OrganicSampled sampled:
						await ApplySampleAsync(sampled, catalog, evaluatorRepository, cancellationToken);
						break;

					case OrganicDataSold sold:
						await ApplySaleAsync(sold, catalog, evaluatorRepository, cancellationToken);
						break;
				}

				// Save per change so a sale later in the same batch sees rows inserted before it
				await unitOfWork.SaveChangesAsync(cancellationToken);
			}
		}
		catch (Exception ex) when (ex is not OperationCanceledException)
		{
			// Both operations are idempotent, so anything missed here is applied on the next full load
			Debug.WriteLine($"Evaluator sync failed: {ex}");
		}
		finally
		{
			_gate.Release();
		}
	}

	private static async Task ApplySampleAsync(OrganicSampled sampled,
		IReadOnlyDictionary<string, Genus> catalog, IRepository<EvaluatorEntity> repository,
		CancellationToken cancellationToken)
	{
		if (!catalog.TryGetValue(sampled.SpeciesId, out var genus))
			return;

		var genusId = genus.Id;

		// Journal files are replayed on every load; IgnoreQueryFilters so sold rows count too
		var exists = await repository.AsNoTracking()
			.IgnoreQueryFilters()
			.AnyAsync(x => x.GenusId == genusId && x.DateCreation == sampled.Timestamp, cancellationToken);

		if (exists)
			return;

		repository.Add(new EvaluatorEntity
		{
			GenusId = genusId,
			DateCreation = sampled.Timestamp,
			HasFirstFootStep = sampled.HasFirstFootStep,
			Total = genus.Value * (sampled.HasFirstFootStep ? FirstFootStepMultiplier : 1),
			IsActive = true
		});
	}

	private static async Task ApplySaleAsync(OrganicDataSold sold,
		IReadOnlyDictionary<string, Genus> catalog, IRepository<EvaluatorEntity> repository,
		CancellationToken cancellationToken)
	{
		foreach (var group in sold.SpeciesIds.GroupBy(id => id, StringComparer.OrdinalIgnoreCase))
		{
			if (!catalog.TryGetValue(group.Key, out var genus))
				continue;

			var genusId = genus.Id;

			// How many rows this exact sale already consumed on a previous replay
			var alreadySold = await repository.AsNoTracking()
				.IgnoreQueryFilters()
				.CountAsync(x => x.GenusId == genusId && x.DateSold == sold.Timestamp, cancellationToken);

			var toSell = group.Count() - alreadySold;
			if (toSell <= 0)
				continue;

			// Query filter keeps this to active rows; can't sell samples taken after the sale
			var active = await repository.ListAsync(
				x => x.GenusId == genusId && x.DateCreation <= sold.Timestamp,
				tracking: true,
				cancellationToken: cancellationToken);

			foreach (var row in active.OrderBy(x => x.DateCreation).Take(toSell))
			{
				row.IsActive = false;
				row.DateSold = sold.Timestamp;
			}
		}
	}
}