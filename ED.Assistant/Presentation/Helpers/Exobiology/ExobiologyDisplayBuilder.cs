using System.Globalization;
using ED.Assistant.Domain.Types;
using ED.Assistant.Extensions;
using ED.Assistant.Presentation.ViewModels.System;

namespace ED.Assistant.Presentation.Helpers.Exobiology;

static class ExobiologyDisplayBuilder
{
    public static IReadOnlyList<OrganicPlanetViewModel> Build(
        JournalState state,
        IReadOnlyList<Genus> catalog,
        IReadOnlyList<OrganicPlanetViewModel>? previousPlanets = null)
    {
        if (state.CurrentSystemAddress is not { } address)
            return [];

        var scans = state.Scans.Values
            .Where(s => s.SystemAddress == address)
            .ToArray();

        var dss = state.SAASignals.Values
            .Where(s => s.SystemAddress == address)
            .ToDictionary(s => s.BodyId);

        var bodies = state.FSSSignals.Values
            .Where(s =>
                s.SystemAddress == address &&
                HasBiology(s.Signals))
            .Select(s => (s.BodyId, s.BodyName))
            .Concat(
                dss.Values
                    .Where(s => HasBiology(s.Signals))
                    .Select(s => (s.BodyId, s.BodyName)))
            .DistinctBy(s => s.BodyId)
            .OrderBy(s => s.BodyName);

        var planets = new List<OrganicPlanetViewModel>();
        foreach (var body in bodies)
        {
            var planet = new OrganicPlanetViewModel
            {
                BodyId = body.BodyId,
                BodyName = body.BodyName
            };

            var scan = scans.FirstOrDefault(s =>
                s.BodyId == body.BodyId);

            var mapped = dss.GetValueOrDefault(body.BodyId);

            var confirmed = state.Organics
                .Where(o =>
                    o.SystemAddress == address &&
                    o.BodyId == body.BodyId)
                .GroupBy(o => (o.SpeciesId, o.VariantId))
                .ToList();

            foreach (var group in confirmed)
            {
                var events = group
                    .OrderBy(e => e.Timestamp)
                    .ToArray();

                var latest = events[^1];

                var entry = catalog.FirstOrDefault(g =>
                    Same(g.CodexName, latest.SpeciesId));

                planet.Signals.Add(new OrganicSignalViewModel
                {
                    Type = Text(latest.Genus, latest.GenusId),

                    Name = entry?.Name ??
                           Text(latest.Species, latest.SpeciesId),

                    Variant = Text(latest.Variant, "—"),

                    CollectedCount = SampleCount(events),

                    BaseValue = entry is null
                        ? "—"
                        : FormatValue(entry.Value),

                    Distance = entry is null
                        ? "—"
                        : DistanceFor(entry.Rules, scan, scans)
                });
            }

            var previous = previousPlanets?
                .FirstOrDefault(p => p.BodyId == body.BodyId);

            var signalCount = mapped?.Signals?
                                  .Where(s => s.TypeId == SignalType.Biological)
                                  .Sum(s => s.Count)
                              ?? state.FSSSignals.Values
                                  .Where(s =>
                                      s.SystemAddress == address &&
                                      s.BodyId == body.BodyId)
                                  .SelectMany(s => s.Signals ?? [])
                                  .Where(s => s.TypeId == SignalType.Biological)
                                  .Sum(s => s.Count);

            var confirmedSpecies = confirmed
                .Select(g => g.Key.SpeciesId)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            
            var confirmedGenera = confirmed
                .SelectMany(group => group)
                .Select(o => o.GenusId)
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            AddPredictions(
                planet,
                catalog,
                scan,
                scans,
                mapped,
                signalCount,
                previous,
                confirmedSpecies,
                confirmedGenera);

            if (mapped is not null)
            {
                foreach (var genus in
                         (mapped.Genuses ?? []).DistinctBy(g => g.GenusId))
                {
                    var hasConfirmedSpecies = confirmed.Any(g =>
                        g.Any(o => Same(o.GenusId, genus.GenusId)));

                    var hasPossiblePrediction = planet.Signals.Any(s =>
                        s.IsPrediction &&
                        !s.IsExcluded &&
                        Same(s.GenusId, genus.GenusId));

                    if (hasConfirmedSpecies || hasPossiblePrediction)
                        continue;

                    // Keep detected genera visible even if the database
                    // cannot produce a matching species prediction.
                    var candidates = catalog
                        .Where(g => Same(g.CodexType, genus.GenusId))
                        .ToList();

                    planet.Signals.Add(new OrganicSignalViewModel
                    {
                        Type = Text(genus.Genus, genus.GenusId),
                        Name = "—",
                        Distance = DistanceFor(
                            candidates.SelectMany(g => g.Rules),
                            scan,
                            scans)
                    });
                }
            }

            if (planet.Signals.Count == 0)
            {
                planet.Signals.Add(new OrganicSignalViewModel
                {
                    Type = "Biological",
                    Name = "—"
                });
            }

            var allSignals = mapped?.Signals?
                                 .Where(s => s.TypeId == SignalType.Biological)
                                 .Sum(s => s.Count)
                             ?? state.FSSSignals.Values
                                 .Where(s =>
                                     s.SystemAddress == address &&
                                     s.BodyId == body.BodyId)
                                 .SelectMany(s => s.Signals ?? [])
                                 .Where(s => s.TypeId == SignalType.Biological)
                                 .Sum(s => s.Count);

            var collectedSignals = confirmed
                .Count(group => group.Any(e => e.ScanType == ScanType.Analyse));

            var displayPlanet = new OrganicPlanetViewModel
            {
                BodyId = planet.BodyId,
                BodyName = $"{body.BodyName} ({collectedSignals}/{allSignals} signals)"
            };

            foreach (var signal in planet.Signals)
                displayPlanet.Signals.Add(signal);

            planets.Add(displayPlanet);
        }

        return planets;
    }

    private static void AddPredictions(
        OrganicPlanetViewModel planet,
        IReadOnlyList<Genus> catalog,
        ScanEvent? scan,
        IReadOnlyList<ScanEvent> scans,
        SAASignalsFoundEvent? mapped,
        int signalCount,
        OrganicPlanetViewModel? previous,
        HashSet<string> confirmedSpecies,
        HashSet<string> confirmedGenera)
    {
        var candidates =
            new Dictionary<string, OrganicSignalViewModel>(
                StringComparer.OrdinalIgnoreCase);

        // Preserve predictions shown before DSS, even if a later
        // Scan event changes which database rules match.
        if (mapped is not null && previous is not null)
        {
            foreach (var row in previous.Signals.Where(s => s.IsPrediction))
            {
                candidates[row.SpeciesId] = row;
            }
        }

        foreach (var entry in catalog)
        {
            var matching = entry.Rules
                .Select(r => (
                    Rule: r,
                    Match: BiologyRuleMatcher.Evaluate(r, scan, scans)))
                .Where(r => r.Match != BiologyRuleMatch.Rejected)
                .ToList();

            if (matching.Count == 0)
                continue;
            
            candidates[entry.CodexName] = new OrganicSignalViewModel
            {
                IsPrediction = true,
                SpeciesId = entry.CodexName,
                GenusId = entry.CodexType,

                Type = matching.Any(r =>
                    r.Match == BiologyRuleMatch.Matched)
                    ? "Predicted"
                    : "Predicted (partial)",

                Name = entry.Name,
                BaseValue = FormatValue(entry.Value),

                Distance = FormatDistance(
                    matching.Select(r => r.Rule.Distance))
            };
        }

        var detected = (mapped?.Genuses ?? [])
            .Select(g => g.GenusId)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // Missing or incomplete DSS information is not evidence
        // that a predicted genus is absent.
        var canExclude =
            mapped is not null &&
            signalCount > 0 &&
            detected.Count >= signalCount;

        foreach (var row in candidates.Values.OrderBy(s => s.Name))
        {
            // The confirmed species already has its own row with sample progress.
            if (confirmedSpecies.Contains(row.SpeciesId))
                continue;

            var hasGenus = !string.IsNullOrWhiteSpace(row.GenusId);

            var excludedByDss =
                canExclude &&
                hasGenus &&
                !detected.Contains(row.GenusId);

            // This species is unconfirmed, but another species of its genus
            // has already been identified by an organic sample.
            var excludedBySample =
                hasGenus &&
                confirmedGenera.Contains(row.GenusId);

            var excluded = excludedByDss || excludedBySample;

            var predictionType = row.Type.StartsWith(
                "Predicted",
                StringComparison.Ordinal)
                ? row.Type
                : "Predicted";

            var detectedGenus = mapped?.Genuses?
                .FirstOrDefault(g => Same(g.GenusId, row.GenusId));

            planet.Signals.Add(new OrganicSignalViewModel
            {
                IsPrediction = true,
                IsExcluded = excluded,

                SpeciesId = row.SpeciesId,
                GenusId = row.GenusId,

                Type = excludedBySample
                    ? "Excluded by sample"
                    : excludedByDss
                        ? "Excluded by DSS"
                        : detectedGenus is not null
                            ? Text(detectedGenus.Genus, detectedGenus.GenusId)
                            : predictionType,

                Name = row.Name,
                BaseValue = row.BaseValue,
                Distance = row.Distance
            });
        }
    }

    private static string DistanceFor(
        IEnumerable<Rule> rules,
        ScanEvent? body,
        IReadOnlyList<ScanEvent> scans)
    {
        var available = rules.ToList();

        var possible = available
            .Where(r =>
                BiologyRuleMatcher.Evaluate(r, body, scans) !=
                BiologyRuleMatch.Rejected)
            .ToList();

        var selected = possible.Count > 0
            ? possible
            : available;

        return FormatDistance(selected.Select(r => r.Distance));
    }

    private static string FormatDistance(IEnumerable<double?> distances)
    {
        var values = distances
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        if (values.Count == 0)
            return "—";

        return string.Join(
            " / ",
            values.Select(v =>
                v is { } value
                    ? value.ToString(
                        "#,0.##",
                        CultureInfo.InvariantCulture) + " m"
                    : "—"));
    }

    private static int SampleCount(IEnumerable<ScanOrganicEvent> events)
    {
        var count = 0;

        foreach (var e in events.DistinctBy(e =>
                     (e.Timestamp, e.ScanType)))
        {
            if (e.ScanType == ScanType.Log)
            {
                count = 1;
            }
            else if (e.ScanType == ScanType.Sample && count < 3)
            {
                count = Math.Min(2, count + 1);
            }
            else if (e.ScanType == ScanType.Analyse)
            {
                count = 3;
            }
        }

        return count;
    }

    private static bool HasBiology(IEnumerable<SignalItem>? signals) =>
        signals?.Any(s =>
            s.TypeId == SignalType.Biological &&
            s.Count > 0) == true;

    private static bool Same(string left, string right) =>
        string.Equals(
            left,
            right,
            StringComparison.OrdinalIgnoreCase);

    private static string Text(string value, string fallback) =>
        string.IsNullOrWhiteSpace(value) ? fallback : value;

    private static string FormatValue(decimal value) =>
        value > 0
            ? value.ToMillions()
            : "—";
}