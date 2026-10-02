using System.Globalization;
using ED.Assistant.Plugins.Explorer.Data;
using ED.Assistant.Plugins.Explorer.Valuation;

namespace ED.Assistant.Plugins.Explorer.ViewModels;

public sealed class ExplorerItemViewModel
{
    // Journal PlanetClass values
    private const string EarthlikeClass = "Earthlike body";
    private const string WaterWorldClass = "Water world";

    public string Body { get; init; } = string.Empty;
    public string Scanned { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Scan { get; init; } = string.Empty;
    public string ScanTip { get; init; } = string.Empty;
    public string Value { get; init; } = "—";
    public bool IsFirstDiscovery { get; init; }
    public bool IsEarthlike { get; init; }
    public bool IsWaterWorld { get; init; }

    public static ExplorerItemViewModel From(ExplorerBody body)
    {
        var kind = BodyValueCatalog.GetScanKind(body);
        var inefficient = body.IsMapped && !body.IsEfficientMapping;

        return new ExplorerItemViewModel
        {
            Body = body.BodyName,
            // SQLite returns Kind=Unspecified; journal timestamps are UTC
            Scanned = DateTime.SpecifyKind(body.DateScanned, DateTimeKind.Utc)
                .ToLocalTime()
                .ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
            Type = BodyValueCatalog.DisplayName(body),
            Scan = BodyValueCatalog.Label(kind) + (inefficient ? "*" : string.Empty),
            ScanTip = inefficient
                ? BodyValueCatalog.Describe(kind) + " (mapped without the efficiency bonus)"
                : BodyValueCatalog.Describe(kind),
            Value = Formatting.Credits(body.Value),
            IsFirstDiscovery = !body.WasDiscovered,
            IsEarthlike = string.Equals(body.PlanetClass, EarthlikeClass, StringComparison.OrdinalIgnoreCase),
            IsWaterWorld = string.Equals(body.PlanetClass, WaterWorldClass, StringComparison.OrdinalIgnoreCase)
        };
    }
}