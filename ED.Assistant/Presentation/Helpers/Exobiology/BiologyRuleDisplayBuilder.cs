using System.Globalization;
using ED.Assistant.Presentation.ViewModels.System;

namespace ED.Assistant.Presentation.Helpers.Exobiology;

static class BiologyRuleDisplayBuilder
{
    public static IReadOnlyList<OrganicSpawnRuleViewModel> Build(
        Genus genus,
        ScanEvent? body,
        IReadOnlyList<ScanEvent> systemScans,
        bool isConfirmed = false)
    {
        var evaluated = genus.Rules
            .Select(rule => (
                Rule: rule,
                Match: BiologyRuleMatcher.Evaluate(
                    rule,
                    body,
                    systemScans)))
            .ToArray();

        var relevant = evaluated
            .Where(x => x.Match != BiologyRuleMatch.Rejected)
            .ToArray();

        var selected = relevant.Length > 0
            ? relevant
            : evaluated;

        return selected
            .Select((item, index) =>
                new OrganicSpawnRuleViewModel
                {
                    Title = isConfirmed
                        ? "Confirmed by biological sample"
                        : selected.Length > 1
                            ? $"Rule {index + 1} · {MatchText(item.Match)}"
                            : MatchText(item.Match),

                    Description = Describe(item.Rule)
                })
            .ToArray();
    }

    private static string Describe(Rule rule)
    {
        var rows = new List<string>();

        AddList(
            rows,
            "Body",
            rule.BodyClasses.Select(x => x.Name));

        AddList(
            rows,
            "Atmosphere",
            rule.Atmospheres.Select(x => x.Name));

        AddRange(
            rows,
            "Gravity",
            rule.MinGravity,
            rule.MaxGravity,
            "g");

        AddRange(
            rows,
            "Temperature",
            rule.MinTemperature,
            rule.MaxTemperature,
            "K");

        AddRange(
            rows,
            "Pressure",
            rule.MinPressure,
            rule.MaxPressure,
            "atm");

        AddList(
            rows,
            "Volcanism",
            rule.Volcanisms.Select(x => x.Name));

        AddList(
            rows,
            "System body",
            rule.SystemBodyClasses.Select(x => x.Name));

        AddList(rows, "Parent body", rule.ParentBodyClasses.Select(x => x.Name));

        foreach (var group in rule.Stars.GroupBy(x => x.Type))
        {
            var title = group.Key == RuleStarType.ParentStar
                ? "Parent star"
                : "Star";

            var values = group.Select(x =>
                string.IsNullOrWhiteSpace(x.LuminosityClass)
                    ? x.StarClass.Name
                    : $"{x.StarClass.Name} {x.LuminosityClass}");

            AddList(rows, title, values);
        }

        foreach (var component in rule.AtmosphereComponents)
        {
            rows.Add(
                $"Atmosphere component: " +
                $"{component.Atmosphere.Name} ≥ " +
                $"{Number(component.MinPercentage)}%");
        }

        if (rule.MaxOrbitalPeriod is { } orbitalPeriod)
        {
            rows.Add(
                $"Orbital period: ≤ {Duration(orbitalPeriod)}");
        }

        if (rule.Guardian is { } guardian)
        {
            rows.Add(
                $"Guardian: {(guardian ? "Yes" : "No")}");
        }

        if (rule.Nebula is { } nebula)
        {
            rows.Add($"Nebula: {nebula}");
        }

        return rows.Count > 0
            ? string.Join(Environment.NewLine, rows)
            : "No additional spawn conditions.";
    }

    private static void AddList(
        ICollection<string> rows,
        string title,
        IEnumerable<string> values)
    {
        var items = values
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToArray();

        if (items.Length > 0)
        {
            rows.Add(
                $"{title}: {string.Join(", ", items)}");
        }
    }

    private static void AddRange(
        ICollection<string> rows,
        string title,
        double? min,
        double? max,
        string unit)
    {
        if (min is null && max is null)
            return;

        var value = (min, max) switch
        {
            ({ } minimum, { } maximum) =>
                $"{Number(minimum)} – {Number(maximum)}",

            ({ } minimum, null) =>
                $"≥ {Number(minimum)}",

            (null, { } maximum) =>
                $"≤ {Number(maximum)}",

            _ => string.Empty
        };

        rows.Add($"{title}: {value} {unit}");
    }

    private static string MatchText(BiologyRuleMatch match) =>
        match switch
        {
            BiologyRuleMatch.Matched =>
                "Matches current body",

            BiologyRuleMatch.Possible =>
                "Possible — more scan data required",

            _ =>
                "Does not match current scan"
        };

    private static string Number(double value) =>
        value.ToString(
            "0.###",
            CultureInfo.InvariantCulture);

    private static string Duration(double seconds)
    {
        var value = TimeSpan.FromSeconds(seconds);

        return value.TotalDays >= 1
            ? $"{value.TotalDays:0.##} d"
            : $"{value.TotalHours:0.##} h";
    }
}