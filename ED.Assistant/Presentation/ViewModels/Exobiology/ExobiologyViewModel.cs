using ED.Assistant.Data;
using ED.Assistant.Data.Repository;
using ED.Assistant.Domain.Types;
using ED.Assistant.Extensions;
using ED.Assistant.Helpers;
using ED.Assistant.Presentation.ViewModels.System;

namespace ED.Assistant.Presentation.ViewModels.Exobiology;

public sealed class ExobiologyViewModel : LoadableViewModel
{
	private readonly IRepository<BioSpecies> _speciesRepository;

	public ObservableCollection<OrganicPlanetViewModel> Planets { get; } = [];

	protected override bool ActivateOnNavigation => true;

	public ExobiologyViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache, IRepository<BioSpecies> speciesRepository)
		: base(journalLoader, stateStore, memoryCache) =>
		_speciesRepository = speciesRepository;

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		var systemAddress = state.FSDJump?.SystemAddress;
		if (systemAddress is null)
			return;

		var species = await _speciesRepository
			.AsNoTracking()
			.Include(x => x.Genus)
			.ToListAsync(cancellationToken);

		var planets = state.FSSSignals.Values
			.Where(x =>
				x.SystemAddress == systemAddress &&
				x.Signals?.Any(s => s.TypeId == SignalType.Biological) == true)
			.OrderBy(x => x.BodyName)
			.Select(fssSignal =>
			{
				var planet = new OrganicPlanetViewModel
				{
					BodyId = fssSignal.BodyId,
					BodyName = fssSignal.BodyName
				};

				var saaSignal = state.SAASignals.GetValueOrDefault(fssSignal.BodyId);
				var sampledGroups = state.Organics
					.Where(o =>
						o.SystemAddress == fssSignal.SystemAddress &&
						o.BodyId == fssSignal.BodyId)
					.GroupBy(o => new
					{
						o.GenusId,
						o.SpeciesId,
						o.VariantId
					})
					.ToList();

				foreach (var group in sampledGroups)
				{
					var events = group.OrderByDescending(o => o.Timestamp).ToList();
					var latest = events.First();
					var collectedCount = events.Any(o => o.ScanType == ScanType.Analyse)
						? 3
						: Math.Min(events.Count(o => o.ScanType == ScanType.Sample), 2);

					var matchedSpecies = species.FirstOrDefault(x => MatchesName(latest.Species, x.Name,
						x.DisplayName));
					planet.Signals.Add(new OrganicSignalViewModel
					{
						Type = latest.Genus,
						Name = latest.Species,
						Variant = latest.Variant,
						CollectedCount = collectedCount,
						BaseValue = matchedSpecies is null
							? Constants.EmptyValue
							: matchedSpecies.BaseValue.ToMillions(),
						Distance = matchedSpecies is null
							? GetGenusDistance(species, latest.Genus)
							: FormatDistance(matchedSpecies.MinScanDistanceM)
					});
				}

				var sampledGenusIds = sampledGroups.Select(g =>
					g.Key.GenusId).ToHashSet();
				foreach (var genus in saaSignal?.Genuses ?? [])
				{
					if (sampledGenusIds.Contains(genus.GenusId))
						continue;

					planet.Signals.Add(new OrganicSignalViewModel
					{
						Type = genus.Genus,
						Name = Constants.EmptyValue,
						Variant = Constants.EmptyValue,
						CollectedCount = 0,
						BaseValue = Constants.EmptyValue,
						Distance = GetGenusDistance(species, genus.Genus)
					});
				}

				if (planet.Signals.Count == 0)
				{
					planet.Signals.Add(new OrganicSignalViewModel
					{
						Type = "Biological",
						Name = Constants.EmptyValue,
						Variant = Constants.EmptyValue,
						CollectedCount = 0,
						BaseValue = Constants.EmptyValue,
						Distance = Constants.EmptyValue
					});
				}

				return planet;
			}).ToList();

		Planets.Clear();

		foreach (var planet in planets)
			Planets.Add(planet);
	}

	private static string FormatDistance(int? distance) =>
		distance is > 0 ? $"{distance:N0} m" : Constants.EmptyValue;

	private static bool MatchesName(string name, string databaseName, string displayName) =>
		!string.IsNullOrWhiteSpace(name) &&
		(string.Equals(name.Trim(), databaseName, StringComparison.OrdinalIgnoreCase) ||
		 string.Equals(name.Trim(), displayName, StringComparison.OrdinalIgnoreCase));

	private static string GetGenusDistance(IEnumerable<BioSpecies> species, string genus)
	{
		var distances = species
			.Where(x => MatchesName(genus, x.Genus.Name, x.Genus.DisplayName))
			.Select(x => x.MinScanDistanceM)
			.Distinct()
			.Take(2)
			.ToList();

		return distances.Count == 1 ? FormatDistance(distances[0]) : Constants.EmptyValue;
	}
}
