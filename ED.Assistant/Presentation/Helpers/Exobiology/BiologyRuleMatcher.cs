namespace ED.Assistant.Presentation.Helpers.Exobiology;

internal static class BiologyRuleMatcher
{
    private const double MetresPerSecondSquaredPerG = 9.80665;
    private const double PascalsPerAtmosphere = 101325;
    
    public static BiologyRuleMatch Evaluate(
        Rule rule,
        ScanEvent? body,
        IReadOnlyList<ScanEvent> systemScans)
    {
        var checks = new List<bool?>();
        if (body is null)
        {
            checks.Add(null);
        }
        else
        {
            checks.Add(Allowed<BodyClassEnum>(
                rule.BodyClasses.Select(c => c.Id),
                body.PlanetClass));

            var atmosphere = ParseAtmosphere(body.AtmosphereType);

            checks.Add(Allowed(
                rule.Atmospheres.Select(a => a.Id),
                atmosphere));

            checks.Add(InRange(
                Positive(body.SurfaceGravity) / MetresPerSecondSquaredPerG,
                rule.MinGravity,
                rule.MaxGravity));

            checks.Add(InRange(
                Positive(body.SurfaceTemperature),
                rule.MinTemperature,
                rule.MaxTemperature));

            double? pressure =
                body.SurfacePressure > 0 ||
                atmosphere == AtmosphereEnum.None
                    ? body.SurfacePressure / PascalsPerAtmosphere
                    : null;

            checks.Add(InRange(
                pressure,
                rule.MinPressure,
                rule.MaxPressure));

            checks.Add(InRange(
                Positive(body.OrbitalPeriod),
                null,
                rule.MaxOrbitalPeriod));

            checks.Add(MatchesVolcanism(
                rule.Volcanisms,
                body.Volcanism));

            var compositions = body.AtmosphereCompositions;
            checks.AddRange(rule.AtmosphereComponents.Select(component =>
                compositions?.Any(c =>
                    ParseAtmosphere(c.Name) is { } type &&
                    (int)type == component.AtmosphereId &&
                    c.Percent >= component.MinPercentage)));
        }

        if (rule.SystemBodyClasses.Count > 0)
        {
            checks.Add(
                systemScans.Any(s =>
                    Parse<BodyClassEnum>(s.PlanetClass) is { } type &&
                    rule.SystemBodyClasses.Any(c => c.Id == (int)type))
                    ? true
                    : null);
        }

        checks.AddRange(rule.Stars
            .GroupBy(s => s.Type)
            .Select(group => MatchesStars(group, body, systemScans)));

        // JournalState does not contain the information needed for these.
        if (rule.Guardian is not null || rule.Nebula is not null || rule.ParentBodyClasses.Count > 0)
        {
            checks.Add(null);
        }

        if (checks.Contains(false))
            return BiologyRuleMatch.Rejected;

        return checks.Contains(null)
            ? BiologyRuleMatch.Possible
            : BiologyRuleMatch.Matched;
    }
    
    private static bool? MatchesStars(
        IGrouping<RuleStarType, RuleStar> rules,
        ScanEvent? body,
        IReadOnlyList<ScanEvent> scans)
    {
        var stars = scans
            .Where(s => !string.IsNullOrWhiteSpace(s.StarType))
            .ToList();

        var complete = false;

        if (rules.Key == RuleStarType.ParentStar)
        {
            if (body?.Parents is null)
                return null;

            var ids = body.Parents
                .Where(p => p.Type.Equals(
                    "Star",
                    StringComparison.OrdinalIgnoreCase))
                .Select(p => p.BodyId)
                .ToHashSet();

            if (ids.Count == 0)
                return null;

            stars = [..stars.Where(s => ids.Contains(s.BodyId))];

            complete = ids.All(id =>
                stars.Any(s => s.BodyId == id));
        }

        var matches = stars
            .SelectMany(s => rules.Select(r => MatchesStar(r, s)))
            .ToList();

        if (matches.Contains(true))
            return true;

        return complete && matches.All(m => m == false)
            ? false
            : null;
    }
    
    private static bool? MatchesStar(RuleStar rule, ScanEvent star)
    {
        var observed = Parse<StarClassEnum>(star.StarType);

        if (observed is null ||
            !Enum.IsDefined((StarClassEnum)rule.StarClassId))
        {
            return null;
        }

        if ((int)observed.Value != rule.StarClassId)
            return false;

        if (string.IsNullOrWhiteSpace(rule.LuminosityClass))
            return true;

        if (string.IsNullOrWhiteSpace(star.Luminosity))
            return null;

        return string.Equals(
            rule.LuminosityClass.Trim(),
            star.Luminosity.Trim(),
            StringComparison.OrdinalIgnoreCase);
    }
    
    private static bool? MatchesVolcanism(
        IEnumerable<Volcanism> requirements,
        string observed)
    {
        var rules = requirements.ToList();

        if (rules.Count == 0)
            return true;

        var value = Normalize(observed);

        var absent =
            value.Length == 0 ||
            value == Normalize(nameof(VolcanismEnum.None));

        return rules.Any(r => (VolcanismEnum)r.Id switch
        {
            VolcanismEnum.None => absent,
            VolcanismEnum.Any => !absent,
            var type when Enum.IsDefined(type) =>
                !absent &&
                value.Contains(
                    Normalize(type.ToString()),
                    StringComparison.Ordinal),

            _ => false
        });
    }

    private static bool? Allowed<T>(
        IEnumerable<int> required,
        string observed)
        where T : struct, Enum =>
        Allowed(required, Parse<T>(observed));

    private static bool? Allowed<T>(
        IEnumerable<int> required,
        T? observed)
        where T : struct, Enum
    {
        var ids = required.ToList();

        if (ids.Count == 0)
            return true;

        if (observed is null)
            return null;

        return ids.Contains(Convert.ToInt32(observed.Value));
    }

    private static AtmosphereEnum? ParseAtmosphere(string value) =>
        string.IsNullOrWhiteSpace(value)
            ? AtmosphereEnum.None
            : Parse<AtmosphereEnum>(value);
    
    private static T? Parse<T>(string value)
        where T : struct, Enum
    {
        var normalized = Normalize(value);

        foreach (var item in Enum.GetValues<T>())
        {
            if (Normalize(item.ToString()) == normalized)
                return item;
        }

        return null;
    }

    private static string Normalize(string value) =>
        new string(
                value
                    .Where(char.IsLetterOrDigit)
                    .Select(char.ToLowerInvariant)
                    .ToArray())
            .Replace("sulfur", "sulphur", StringComparison.Ordinal);

    private static double? Positive(double value) =>
        value > 0 ? value : null;

    private static bool? InRange(
        double? value,
        double? min,
        double? max)
    {
        if (min is null && max is null)
            return true;

        if (value is null)
            return null;

        return (min is null || value >= min) &&
               (max is null || value <= max);
    }
}