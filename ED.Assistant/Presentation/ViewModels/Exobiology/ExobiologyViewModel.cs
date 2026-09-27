using ED.Assistant.Data.Repository;
using ED.Assistant.Domain.Types;
using ED.Assistant.Extensions;
using ED.Assistant.Helpers;
using ED.Assistant.Presentation.ViewModels.System;

namespace ED.Assistant.Presentation.ViewModels.Exobiology;

public sealed class ExobiologyViewModel : LoadableViewModel
{
	/*private const double GravityDivisor = 9.797759;
	private const double PressureDivisor = 101231.656250;

	private readonly IRepository<BioSpecies> _speciesRepository;*/

	public ObservableCollection<OrganicPlanetViewModel> Planets { get; } = [];

	protected override bool ActivateOnNavigation => true;

	public ExobiologyViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		IMemoryCache memoryCache/*, IRepository<BioSpecies> speciesRepository*/)
		: base(journalLoader, stateStore, memoryCache) {}
		/*=> _speciesRepository = speciesRepository;*/

	protected override async Task UpdateFromStateAsync(JournalState state,
		CancellationToken cancellationToken = default)
	{
		/*var systemAddress = state.FSDJump?.SystemAddress;
		if (systemAddress is null)
			return;

		var species = await _speciesRepository
			.AsNoTracking()
			.AsSplitQuery()
			.Include(x => x.Genus)
			.Include(x => x.SpawnRules)
				.ThenInclude(x => x.BodyTypes)
				.ThenInclude(x => x.BodyType)
			.Include(x => x.SpawnRules)
				.ThenInclude(x => x.SystemBodyTypes)
				.ThenInclude(x => x.BodyType)
			.Include(x => x.SpawnRules)
				.ThenInclude(x => x.Atmospheres)
				.ThenInclude(x => x.Atmosphere)
			.Include(x => x.SpawnRules)
				.ThenInclude(x => x.AtmosphereComponents)
				.ThenInclude(x => x.Atmosphere)
			.Include(x => x.SpawnRules)
				.ThenInclude(x => x.VolcanismPatterns)
			.Include(x => x.SpawnRules)
				.ThenInclude(x => x.Stars)
			.ToListAsync(cancellationToken);

		var systemScans = state.Scans.Values
			.Where(x => x.SystemAddress == systemAddress)
			.ToList();

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
				var saaGenuses = saaSignal?.Genuses?.ToList() ?? [];
				var bodyScan = state.Scans.GetValueOrDefault(fssSignal.BodyId);

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

					var matchedSpecies = species.FirstOrDefault(x => MatchesSpecies(latest, x));
					planet.Signals.Add(new OrganicSignalViewModel
					{
						Type = matchedSpecies is null
							? latest.Genus
							: GetGenusName(matchedSpecies.Genus),
						Name = matchedSpecies is null
							? latest.Species
							: GetSpeciesName(matchedSpecies),
						Variant = string.IsNullOrWhiteSpace(latest.Variant)
							? Constants.EmptyValue
							: latest.Variant,
						CollectedCount = collectedCount,
						BaseValue = matchedSpecies is null
							? Constants.EmptyValue
							: matchedSpecies.BaseValue.ToMillions(),
						Distance = matchedSpecies is null
							? GetGenusDistance(species, latest.Genus)
							: FormatDistance(matchedSpecies.MinScanDistanceM)
					});
				}

				var predictions = species
					.Where(x => x.SpawnRules.Any(rule =>
						bodyScan is null
							? MatchesRuleWithPartialData(rule, systemScans)
							: MatchesRule(rule, bodyScan, systemScans)))
					.OrderBy(x => GetGenusName(x.Genus))
					.ThenBy(GetSpeciesName)
					.ToList();

				if (saaGenuses.Count > 0)
				{
					foreach (var genus in saaGenuses)
					{
						var genusPredictions = predictions
							.Where(x => MatchesGenus(genus, x.Genus))
							.ToList();

						var hasConfirmedGenus = sampledGroups.Any(group =>
							group.Any(organic =>
								string.Equals(organic.GenusId, genus.GenusId,
									StringComparison.OrdinalIgnoreCase)));

						if (genusPredictions.Count == 0)
						{
							if (!hasConfirmedGenus)
								AddPlaceholder(planet, genus.Genus, GetGenusDistance(species, genus.Genus));

							continue;
						}

						foreach (var prediction in genusPredictions)
						{
							var alreadyConfirmed = sampledGroups.Any(group =>
								group.Any(organic => MatchesSpecies(organic, prediction)));

							if (!alreadyConfirmed)
								AddPrediction(planet, prediction);
						}
					}
				}
				else
				{
					foreach (var prediction in predictions)
					{
						var alreadyConfirmed = sampledGroups.Any(group =>
							group.Any(organic => MatchesSpecies(organic, prediction)));

						if (!alreadyConfirmed)
							AddPrediction(planet, prediction);
					}
				}

				if (planet.Signals.Count == 0)
					AddPlaceholder(planet, "Biological", Constants.EmptyValue);

				return planet;
			}).ToList();

		Planets.Clear();

		foreach (var planet in planets)
			Planets.Add(planet);*/
	}

	/*private static bool MatchesRuleWithPartialData(BioSpawnRule rule,
		IReadOnlyList<ScanEvent> systemScans)
	{
		// FSSBodySignals may arrive before we have a Scan event for the body.
		// In that case planet-specific conditions are unknown, so they must not
		// eliminate a species. We only apply constraints that can be checked from
		// the system data already present in the journal state.
		if (!MatchesKnownSystemStars(rule.Stars, systemScans))
			return false;

		// System-body requirements are also treated as unknown when the required
		// body type has not been seen yet. The journal state may only contain a
		// partial set of system bodies at this point.
		return true;
	}

	private static bool MatchesKnownSystemStars(IEnumerable<BioSpawnRuleStar> requirements,
		IReadOnlyList<ScanEvent> systemScans)
	{
		var items = requirements
			.Where(x => x.Scope == StarScope.System)
			.ToList();

		if (items.Count == 0)
			return true;

		var knownStars = systemScans
			.Where(x => !string.IsNullOrWhiteSpace(x.StarType))
			.ToList();

		if (knownStars.Count == 0)
			return true;

		return knownStars.Any(star => items.Any(requirement =>
			MatchesStar(requirement, star)));
	}

	private static bool MatchesRule(BioSpawnRule rule, ScanEvent body, IReadOnlyList<ScanEvent> systemScans)
	{
		if (!MatchesBodyTypes(rule.BodyTypes, body.PlanetClass))
			return false;

		if (!MatchesAtmospheres(rule.Atmospheres, body))
			return false;

		if (!MatchesAtmosphereComponents(rule.AtmosphereComponents, body))
			return false;

		if (!MatchesNumericConditions(rule, body))
			return false;

		if (!MatchesVolcanism(rule, body.Volcanism))
			return false;

		if (!MatchesSystemBodyTypes(rule.SystemBodyTypes, systemScans))
			return false;

		if (!MatchesStars(rule.Stars, body, systemScans))
			return false;

		// Nebula rules are intentionally left as unknown here. The journal state has
		// system coordinates, but the app does not currently carry the nebula catalog
		// needed to evaluate those rules without producing false negatives.
		return true;
	}

	private static bool MatchesBodyTypes(IEnumerable<BioSpawnRuleBodyType> conditions, string bodyType)
	{
		var items = conditions.ToList();
		if (items.Count == 0 || string.IsNullOrWhiteSpace(bodyType))
			return true;

		var required = items.Where(x => x.Mode == ConditionMode.Required).ToList();
		if (required.Count > 0 &&
			!required.Any(x => SameBodyType(bodyType, x.BodyType.Name)))
		{
			return false;
		}

		if (items.Any(x =>
				x.Mode == ConditionMode.Excluded &&
				SameBodyType(bodyType, x.BodyType.Name)))
		{
			return false;
		}

		return !items.Any(x => x.Mode == ConditionMode.Any) ||
			!string.IsNullOrWhiteSpace(bodyType);
	}

	private static bool MatchesAtmospheres(IEnumerable<BioSpawnRuleAtmosphere> conditions, ScanEvent body)
	{
		var items = conditions.ToList();
		if (items.Count == 0)
			return true;

		var atmosphere = string.IsNullOrWhiteSpace(body.AtmosphereType)
			? "None"
			: body.AtmosphereType.Trim();

		var required = items.Where(x => x.Mode == ConditionMode.Required).ToList();
		if (required.Count > 0 &&
			!required.Any(x => SameAtmosphere(atmosphere, x.Atmosphere.Name)))
		{
			return false;
		}

		if (items.Any(x =>
				x.Mode == ConditionMode.Excluded &&
				SameAtmosphere(atmosphere, x.Atmosphere.Name)))
		{
			return false;
		}

		if (items.Any(x => x.Mode == ConditionMode.Any) &&
			SameValue(atmosphere, "None"))
		{
			return false;
		}

		return true;
	}

	private static bool MatchesAtmosphereComponents(
		IEnumerable<BioSpawnRuleAtmosphereComponent> requirements, ScanEvent body)
	{
		var items = requirements.ToList();
		if (items.Count == 0 || body.AtmosphereCompositions is null)
			return true;

		var components = body.AtmosphereCompositions.ToList();
		return items.All(requirement =>
		{
			var component = components.FirstOrDefault(x =>
				SameAtmosphere(x.Name, requirement.Atmosphere.Name));
			return component is not null && component.Percent >= requirement.MinPercent;
		});
	}

	private static bool MatchesNumericConditions(BioSpawnRule rule, ScanEvent body)
	{
		if (body.SurfaceTemperature > 0)
		{
			if (rule.MinTemperatureK is { } minTemperature &&
				body.SurfaceTemperature < minTemperature)
				return false;

			if (rule.MaxTemperatureK is { } maxTemperature &&
				body.SurfaceTemperature > maxTemperature)
				return false;
		}

		if (body.SurfaceGravity > 0)
		{
			var gravity = body.SurfaceGravity / GravityDivisor;
			if (rule.MinGravityG is { } minGravity && gravity < minGravity)
				return false;

			if (rule.MaxGravityG is { } maxGravity && gravity > maxGravity)
				return false;
		}

		if (body.SurfacePressure > 0)
		{
			var pressure = body.SurfacePressure / PressureDivisor;
			if (rule.MinPressureAtmospheres is { } minPressure && pressure < minPressure)
				return false;

			if (rule.MaxPressureAtmospheres is { } maxPressure &&
				(rule.MaxPressureExclusive ? pressure >= maxPressure : pressure > maxPressure))
			{
				return false;
			}
		}

		if (body.OrbitalPeriod > 0 &&
			rule.MaxOrbitalPeriodSeconds is { } maxOrbitalPeriod &&
			(rule.MaxOrbitalPeriodExclusive
				? body.OrbitalPeriod >= maxOrbitalPeriod
				: body.OrbitalPeriod > maxOrbitalPeriod))
		{
			return false;
		}

		if (body.DistanceFromArrivalLS > 0 &&
			rule.MinArrivalDistanceLs is { } minArrivalDistance &&
			body.DistanceFromArrivalLS < minArrivalDistance)
		{
			return false;
		}

		return true;
	}

	private static bool MatchesVolcanism(BioSpawnRule rule, string volcanism)
	{
		volcanism ??= string.Empty;

		return rule.VolcanismMode switch
		{
			VolcanismMode.Unrestricted => true,
			VolcanismMode.None => string.IsNullOrWhiteSpace(volcanism),
			VolcanismMode.Any => !string.IsNullOrWhiteSpace(volcanism),
			VolcanismMode.Patterns => rule.VolcanismPatterns.Any(pattern =>
				pattern.Match switch
				{
					VolcanismMatch.Exact => SameValue(volcanism, pattern.Pattern),
					VolcanismMatch.Contains => volcanism.Contains(
						pattern.Pattern, StringComparison.OrdinalIgnoreCase),
					_ => false
				}),
			_ => true
		};
	}

	private static bool MatchesSystemBodyTypes(IEnumerable<BioSpawnRuleSystemBodyType> requirements,
		IReadOnlyList<ScanEvent> systemScans)
	{
		var items = requirements.ToList();
		if (items.Count == 0)
			return true;

		var knownBodyTypes = systemScans
			.Select(x => x.PlanetClass)
			.Where(x => !string.IsNullOrWhiteSpace(x))
			.ToList();

		if (knownBodyTypes.Count == 0)
			return true;

		return items.Any(requirement =>
			knownBodyTypes.Any(bodyType => SameBodyType(bodyType, requirement.BodyType.Name)));
	}

	private static bool MatchesStars(IEnumerable<BioSpawnRuleStar> requirements, ScanEvent body,
		IReadOnlyList<ScanEvent> systemScans)
	{
		var items = requirements.ToList();
		if (items.Count == 0)
			return true;

		var systemStars = systemScans
			.Where(x => !string.IsNullOrWhiteSpace(x.StarType))
			.ToList();

		var systemRequirements = items.Where(x => x.Scope == StarScope.System).ToList();
		if (systemRequirements.Count > 0 &&
			systemStars.Count > 0 &&
			!systemStars.Any(star => systemRequirements.Any(requirement =>
				MatchesStar(requirement, star))))
		{
			return false;
		}

		var parentRequirements = items.Where(x => x.Scope == StarScope.Parent).ToList();
		if (parentRequirements.Count == 0)
			return true;

		var parentIds = body.Parents?
			.Where(x => string.Equals(x.Type, "Star", StringComparison.OrdinalIgnoreCase))
			.Select(x => x.BodyId)
			.ToHashSet() ?? [];

		var mainStar = systemStars.OrderBy(x => x.BodyId).FirstOrDefault();
		var parentStars = systemStars
			.Where(x => parentIds.Contains(x.BodyId) ||
				(mainStar is not null && x.BodyId == mainStar.BodyId))
			.ToList();

		return parentStars.Count == 0 ||
			parentStars.Any(star => parentRequirements.Any(requirement =>
				MatchesStar(requirement, star)));
	}

	private static bool MatchesStar(BioSpawnRuleStar requirement, ScanEvent star)
	{
		if (!MatchesStarType(requirement.StarType, star.StarType))
			return false;

		if (string.IsNullOrWhiteSpace(requirement.Luminosity) ||
			string.IsNullOrWhiteSpace(star.Luminosity))
		{
			return true;
		}

		var suffixes = new[] { string.Empty, "a", "b", "ab", "z" };
		return suffixes.Any(suffix =>
			string.Equals(requirement.Luminosity + suffix, star.Luminosity,
				StringComparison.OrdinalIgnoreCase));
	}

	private static bool MatchesStarType(string query, string starType) =>
		query switch
		{
			"A" => starType is "A" or "A_BlueWhiteSuperGiant",
			"B" => starType is "B" or "B_BlueWhiteSuperGiant",
			"F" => starType is "F" or "F_WhiteSuperGiant",
			"G" => starType is "G" or "G_WhiteSuperGiant",
			"K" => starType is "K" or "K_OrangeGiant",
			"M" => starType is "M" or "M_RedGiant" or "M_RedSuperGiant",
			"D" or "C" or "W" => starType.StartsWith(query, StringComparison.OrdinalIgnoreCase),
			_ => string.Equals(query, starType, StringComparison.OrdinalIgnoreCase)
		};

	private static bool MatchesSpecies(ScanOrganicEvent organic, BioSpecies species) =>
		(!string.IsNullOrWhiteSpace(organic.SpeciesId) &&
		 string.Equals(organic.SpeciesId, species.JournalName, StringComparison.OrdinalIgnoreCase)) ||
		MatchesName(organic.Species, species.Name, species.DisplayName);

	private static bool MatchesGenus(GenusItem genus, BioGenus databaseGenus) =>
		(!string.IsNullOrWhiteSpace(genus.GenusId) &&
		 string.Equals(genus.GenusId, databaseGenus.JournalName, StringComparison.OrdinalIgnoreCase)) ||
		MatchesName(genus.Genus, databaseGenus.Name, databaseGenus.DisplayName);

	private static bool SameValue(string left, string right) =>
		string.Equals(left.Trim(), right.Trim(), StringComparison.OrdinalIgnoreCase);

	private static bool SameBodyType(string left, string right) =>
		NormalizeBodyType(left) == NormalizeBodyType(right);

	private static bool SameAtmosphere(string left, string right) =>
		NormalizeAtmosphere(left) == NormalizeAtmosphere(right);

	private static string NormalizeBodyType(string value) => NormalizeLookup(value) switch
	{
		"rocky" => "rockybody",
		"icy" => "icybody",
		"rockyice" => "rockyicebody",
		"hcs" => "highmetalcontentbody",
		"highmetalcontent" => "highmetalcontentbody",
		var normalized => normalized
	};

	private static string NormalizeAtmosphere(string value) => NormalizeLookup(value) switch
	{
		"sulfurdioxide" => "sulphurdioxide",
		"sulfurdioxiderich" => "sulphurdioxiderich",
		var normalized => normalized
	};

	private static string NormalizeLookup(string value) =>
		new(value.Where(char.IsLetterOrDigit)
			.Select(char.ToLowerInvariant)
			.ToArray());

	private static void AddPrediction(OrganicPlanetViewModel planet, BioSpecies species) =>
		planet.Signals.Add(new OrganicSignalViewModel
		{
			Type = GetGenusName(species.Genus),
			Name = GetSpeciesName(species),
			Variant = Constants.EmptyValue,
			CollectedCount = 0,
			BaseValue = species.BaseValue.ToMillions(),
			Distance = FormatDistance(species.MinScanDistanceM)
		});

	private static void AddPlaceholder(OrganicPlanetViewModel planet, string type, string distance) =>
		planet.Signals.Add(new OrganicSignalViewModel
		{
			Type = type,
			Name = Constants.EmptyValue,
			Variant = Constants.EmptyValue,
			CollectedCount = 0,
			BaseValue = Constants.EmptyValue,
			Distance = distance
		});

	private static string GetGenusName(BioGenus genus) =>
		string.IsNullOrWhiteSpace(genus.DisplayName) ? genus.Name : genus.DisplayName;

	private static string GetSpeciesName(BioSpecies species) =>
		string.IsNullOrWhiteSpace(species.DisplayName) ? species.Name : species.DisplayName;

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
	}*/
}
