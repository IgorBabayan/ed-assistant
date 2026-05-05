using ED.Assistant.Application.JournalLoading;
using ED.Assistant.Application.State;
using ED.Assistant.Domain.Events;
using ED.Assistant.Domain.Types;
using ED.Assistant.Helpers;
using ED.Assistant.ViewModels;
using System.Collections.ObjectModel;

namespace ED.Assistant.Presentation.ViewModels.Exobiology;

public sealed class ExobiologyViewModel : LoadableViewModel
{
	public ObservableCollection<OrganicPlanetViewModel> Planets { get; } = [];

	protected override bool ActivateOnNavigation => true;

	public ExobiologyViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache) : base(journalLoader, stateStore, memoryCache) { }

	protected override void UpdateFromState(JournalState state)
	{
		var systemAddress = state.FSDJump?.SystemAddress;
		if (systemAddress is null)
			return;

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

					planet.Signals.Add(new OrganicSignalViewModel
					{
						Type = latest.Genus,
						Name = latest.Species,
						Variant = latest.Variant,
						CollectedCount = collectedCount,
						BaseValue = Constants.EmptyValue,
						Distance = Constants.EmptyValue
					});
				}

				var sampledGenusIds = sampledGroups.Select(g => g.Key.GenusId).ToHashSet();
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
						Distance = Constants.EmptyValue
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
}
