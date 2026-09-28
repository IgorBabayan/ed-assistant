using Avalonia.Threading;
using ED.Assistant.Data.Repository;
using ED.Assistant.Presentation.Helpers.Exobiology;
using ED.Assistant.Presentation.ViewModels.System;

namespace ED.Assistant.Presentation.ViewModels.Exobiology;

public sealed class ExobiologyViewModel : LoadableViewModel
{
	private readonly IRepository<Genus> _genusRepository;

	private long? _previousSystemAddress;
	
	private IReadOnlyList<OrganicPlanetViewModel> _previousPlanets = [];

	public ObservableCollection<OrganicPlanetViewModel> Planets { get; } = [];

	protected override bool ActivateOnNavigation => true;

	public ExobiologyViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache, IRepository<Genus>  genusRepository)
		: base(journalLoader, stateStore, memoryCache)
		=> _genusRepository = genusRepository;

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		var snapshot = new JournalState
		{
			FSDJump = state.FSDJump,
			Location = state.Location
		};

		foreach (var pair in state.Scans)
			snapshot.Scans.Add(pair.Key, pair.Value);

		foreach (var pair in state.FSSSignals)
			snapshot.FSSSignals.Add(pair.Key, pair.Value);

		foreach (var pair in state.SAASignals)
			snapshot.SAASignals.Add(pair.Key, pair.Value);

		snapshot.Organics.AddRange(state.Organics);

		IReadOnlyList<OrganicPlanetViewModel> planets = [];
		
		if (snapshot.CurrentSystemAddress is { } address)
		{
			var catalog = await _genusRepository
				.AsNoTracking()
				.AsSplitQuery()
				.Include(c => c.Rules).ThenInclude(r => r.BodyClasses)
				.Include(c => c.Rules).ThenInclude(r => r.Atmospheres)
				.Include(c => c.Rules).ThenInclude(r => r.Volcanisms)
				.Include(c => c.Rules).ThenInclude(r => r.SystemBodyClasses)
				.Include(c => c.Rules).ThenInclude(r => r.AtmosphereComponents)
				.Include(c => c.Rules).ThenInclude(r => r.Stars)
				.ToListAsync(cancellationToken);

			planets = ExobiologyDisplayBuilder.Build(
				snapshot,
				catalog,
				_previousSystemAddress == address
					? _previousPlanets
					: null);
		}
		
		cancellationToken.ThrowIfCancellationRequested();
		
		_previousPlanets = planets;
		_previousSystemAddress = snapshot.CurrentSystemAddress;
		
		await Dispatcher.UIThread.InvokeAsync(() =>
		{
			cancellationToken.ThrowIfCancellationRequested();

			Planets.Clear();

			foreach (var planet in planets)
				Planets.Add(planet);
		});
	}
}
