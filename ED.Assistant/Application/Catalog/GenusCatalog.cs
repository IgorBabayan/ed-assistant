using ED.Assistant.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ED.Assistant.Application.Catalog;

sealed class GenusCatalog : IGenusCatalog, IDisposable
{
	private readonly IServiceScopeFactory _scopeFactory;
	private readonly SemaphoreSlim _gate = new(1, 1);

	private volatile IReadOnlyList<Genus>? _all;
	private volatile IReadOnlyDictionary<string, Genus>? _byCodexName;

	public GenusCatalog(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

	public async Task<IReadOnlyList<Genus>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		await EnsureLoadedAsync(cancellationToken);
		return _all!;
	}

	public async Task<IReadOnlyDictionary<string, Genus>> GetByCodexNameAsync(
		CancellationToken cancellationToken = default)
	{
		await EnsureLoadedAsync(cancellationToken);
		return _byCodexName!;
	}

	public void Dispose() => _gate.Dispose();

	private async Task EnsureLoadedAsync(CancellationToken cancellationToken)
	{
		if (_all is not null)
			return;

		await _gate.WaitAsync(cancellationToken);
		try
		{
			if (_all is not null)
				return;

			// Short-lived scope: no DbContext is kept alive by this singleton
			using var scope = _scopeFactory.CreateScope();
			var repository = scope.ServiceProvider.GetRequiredService<IRepository<Genus>>();

			var genera = await repository
				.AsNoTracking()
				.AsSplitQuery()
				.Include(c => c.Rules).ThenInclude(r => r.BodyClasses)
				.Include(c => c.Rules).ThenInclude(r => r.Atmospheres)
				.Include(c => c.Rules).ThenInclude(r => r.Volcanisms)
				.Include(c => c.Rules).ThenInclude(r => r.SystemBodyClasses)
				.Include(c => c.Rules).ThenInclude(r => r.AtmosphereComponents)
					.ThenInclude(r => r.Atmosphere)
				.Include(c => c.Rules).ThenInclude(r => r.Stars)
					.ThenInclude(r => r.StarClass)
				.ToListAsync(cancellationToken);

			_byCodexName = genera
				.GroupBy(g => g.CodexName, StringComparer.OrdinalIgnoreCase)
				.ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

			// Published last: readers that see _all also see _byCodexName
			_all = genera;
		}
		finally
		{
			_gate.Release();
		}
	}
}