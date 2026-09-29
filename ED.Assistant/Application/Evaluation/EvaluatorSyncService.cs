using System.Diagnostics;
using ED.Assistant.Application.Catalog;
using ED.Assistant.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using EvaluatorEntity = ED.Assistant.Data.Evaluator.Evaluator;

namespace ED.Assistant.Application.Evaluation;

sealed class EvaluatorSyncService : IEvaluatorSyncService, IDisposable
{
	private const int FirstFootStepMultiplier = 5;

	private readonly IServiceScopeFactory _scopeFactory;
	private readonly IGenusCatalog _genusCatalog;
	private readonly SemaphoreSlim _gate = new(1, 1);

	public event EventHandler? DataChanged;

	public EvaluatorSyncService(IServiceScopeFactory scopeFactory, IGenusCatalog genusCatalog)
	{
		_scopeFactory = scopeFactory;
		_genusCatalog = genusCatalog;
	}

	public async Task SyncAsync(JournalState state, CancellationToken cancellationToken = default)
	{
		var changes = state.PendingEvaluatorChanges.ToArray();
		state.PendingEvaluatorChanges.Clear();

		if (changes.Length == 0)
			return;

		var written = 0;

		await _gate.WaitAsync(cancellationToken);
		try
		{
			written = await ApplyAsync(changes, cancellationToken);
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

		if (written > 0)
			DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public void Dispose() => _gate.Dispose();

	/// <summary>
	/// Applies the whole batch with three reads and one SaveChanges (one transaction),
	/// instead of one or two queries plus a commit per change.
	/// </summary>
	private async Task<int> ApplyAsync(EvaluatorChange[] changes, CancellationToken cancellationToken)
	{
		var catalog = await _genusCatalog.GetByCodexNameAsync(cancellationToken);

		var genusIdSet = new HashSet<int>();
		var from = DateTime.MaxValue;
		var to = DateTime.MinValue;

		void Track(string speciesId, DateTime timestamp)
		{
			if (!catalog.TryGetValue(speciesId, out var genus))
				return;

			genusIdSet.Add(genus.Id);

			if (timestamp < from) from = timestamp;
			if (timestamp > to) to = timestamp;
		}

		foreach (var change in changes)
		{
			switch (change)
			{
				case OrganicSampled sampled:
					Track(sampled.SpeciesId, sampled.Timestamp);
					break;

				case OrganicDataSold sold:
					foreach (var speciesId in sold.SpeciesIds)
						Track(speciesId, sold.Timestamp);
					break;
			}
		}

		if (genusIdSet.Count == 0)
			return 0;

		var genusIds = genusIdSet.ToArray();

		using var scope = _scopeFactory.CreateScope();
		var services = scope.ServiceProvider;

		var repository = services.GetRequiredService<IRepository<EvaluatorEntity>>();
		var unitOfWork = services.GetRequiredService<IUnitOfWork>();

		// 1. Samples already stored in the batch's time range.
		//    Journal files are replayed on every load; IgnoreQueryFilters so sold rows count too.
		var existingRows = await repository.AsNoTracking()
			.IgnoreQueryFilters()
			.Where(x => genusIds.Contains(x.GenusId) && x.DateCreation >= from && x.DateCreation <= to)
			.Select(x => new { x.GenusId, x.DateCreation })
			.ToListAsync(cancellationToken);

		var existing = existingRows
			.Select(x => (x.GenusId, x.DateCreation))
			.ToHashSet();

		// 2. How many rows each sale in the range already consumed on a previous replay
		var soldRows = await repository.AsNoTracking()
			.IgnoreQueryFilters()
			.Where(x => genusIds.Contains(x.GenusId) && x.DateSold != null && x.DateSold >= from && x.DateSold <= to)
			.Select(x => new { x.GenusId, x.DateSold })
			.ToListAsync(cancellationToken);

		var soldCounts = soldRows
			.GroupBy(x => (x.GenusId, DateSold: x.DateSold!.Value))
			.ToDictionary(g => g.Key, g => g.Count());

		// 3. Unsold rows (the IsActive query filter applies) that sales in this batch may consume.
		//    Tracked, so the IsActive/DateSold changes below are saved.
		var activeRows = await repository.ListAsync(
			x => genusIds.Contains(x.GenusId) && x.DateCreation <= to,
			tracking: true,
			cancellationToken: cancellationToken);

		var active = activeRows
			.GroupBy(x => x.GenusId)
			.ToDictionary(g => g.Key, g => g.OrderBy(x => x.DateCreation).ToList());

		// Apply in journal order, in memory. New rows are added to `active` right away,
		// so a sale later in the same batch sees samples inserted before it.
		foreach (var change in changes)
		{
			switch (change)
			{
				case OrganicSampled sampled:
					ApplySample(sampled, catalog, repository, existing, active);
					break;

				case OrganicDataSold sold:
					ApplySale(sold, catalog, soldCounts, active);
					break;
			}
		}

		return await unitOfWork.SaveChangesAsync(cancellationToken);
	}

	private static void ApplySample(OrganicSampled sampled,
		IReadOnlyDictionary<string, Genus> catalog, IRepository<EvaluatorEntity> repository,
		HashSet<(int GenusId, DateTime DateCreation)> existing,
		Dictionary<int, List<EvaluatorEntity>> active)
	{
		if (!catalog.TryGetValue(sampled.SpeciesId, out var genus))
			return;

		if (!existing.Add((genus.Id, sampled.Timestamp)))
			return;

		var entity = new EvaluatorEntity
		{
			GenusId = genus.Id,
			DateCreation = sampled.Timestamp,
			HasFirstFootStep = sampled.HasFirstFootStep,
			Total = genus.Value * (sampled.HasFirstFootStep ? FirstFootStepMultiplier : 1),
			IsActive = true
		};

		repository.Add(entity);

		if (!active.TryGetValue(genus.Id, out var rows))
			active[genus.Id] = rows = [];

		InsertSorted(rows, entity);
	}

	private static void ApplySale(OrganicDataSold sold,
		IReadOnlyDictionary<string, Genus> catalog,
		Dictionary<(int GenusId, DateTime DateSold), int> soldCounts,
		Dictionary<int, List<EvaluatorEntity>> active)
	{
		foreach (var group in sold.SpeciesIds.GroupBy(id => id, StringComparer.OrdinalIgnoreCase))
		{
			if (!catalog.TryGetValue(group.Key, out var genus))
				continue;

			var key = (genus.Id, sold.Timestamp);
			soldCounts.TryGetValue(key, out var alreadySold);

			var toSell = group.Count() - alreadySold;
			if (toSell <= 0)
				continue;

			if (!active.TryGetValue(genus.Id, out var rows))
				continue;

			// Oldest first; can't sell samples taken after the sale
			var sell = rows
				.Where(x => x.IsActive && x.DateCreation <= sold.Timestamp)
				.Take(toSell)
				.ToList();

			foreach (var row in sell)
			{
				row.IsActive = false;
				row.DateSold = sold.Timestamp;
			}

			soldCounts[key] = alreadySold + sell.Count;
		}
	}

	private static void InsertSorted(List<EvaluatorEntity> rows, EvaluatorEntity entity)
	{
		var index = rows.FindLastIndex(x => x.DateCreation <= entity.DateCreation) + 1;
		rows.Insert(index, entity);
	}
}