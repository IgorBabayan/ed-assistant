using ED.Assistant.Plugins.Explorer.Data;

namespace ED.Assistant.Plugins.Explorer.Valuation;

public enum ScanKind
{
    /// <summary>FSS, not first discoverer.</summary>
    Fss,
    /// <summary>FSS and first discoverer.</summary>
    FssFirstDiscovery,
    /// <summary>FSS + DSS with efficiency bonus, neither first discovered nor first mapped.</summary>
    FssDss,
    /// <summary>FSS + DSS with efficiency bonus, first discovered and first mapped.</summary>
    FssFirstDiscoveryDss
}

/// <param name="PlanetClass">The journal's PlanetClass value.</param>
public sealed record BodyValue(string PlanetClass, string DisplayName, bool IsTerraformable, double MedianMass,
    long Fss, long FssFirstDiscovery, long FssDss, long FssFirstDiscoveryDss);

public static class BodyValueCatalog
{
    // The DSS columns include the 25% efficiency bonus; mapping with more probes than the target loses it
    private const decimal EfficiencyBonus = 1.25m;

    private static readonly BodyValue[] Values =
    [
        new("Ammonia world", "Ammonia World", false, 0.43914, 143_463, 373_004, 597_762, 1_724_965),
        new("Earthlike body", "Earth-like World", false, 0.498039, 270_290, 702_753, 1_126_206, 3_249_900),
        new("Water world", "Water World", false, 0.780638, 99_747, 259_343, 415_613, 1_199_337),
        new("Water world", "Water World", true, 0.453011, 268_616, 698_400, 1_119_231, 3_229_773),
        new("High metal content body", "High Metal Content Planet", false, 0.344919, 14_070, 36_581, 58_624, 169_171),
        new("High metal content body", "High Metal Content Planet", true, 0.466929, 163_948, 426_264, 683_116, 1_971_272),
        new("Icy body", "Icy Body", false, 0.01854, 500, 1_300, 1_569, 4_527),
        new("Metal rich body", "Metal Rich Body", false, 0.323933, 31_632, 82_244, 131_802, 380_341),
        new("Rocky body", "Rocky Body", false, 0.003359, 500, 1_300, 1_476, 4_260),
        new("Rocky body", "Rocky Body", true, 0.142312, 129_504, 336_711, 539_601, 1_557_130),
        new("Rocky ice body", "Rocky Ice Body", false, 0.180686, 500, 1_300, 1_752, 5_057),
        new("Sudarsky class I gas giant", "Class I Gas Giant", false, 69.551636, 3_845, 9_997, 16_021, 46_233),
        new("Sudarsky class II gas giant", "Class II Gas Giant", false, 476.240875, 28_405, 73_853, 118_354, 341_536),
        new("Sudarsky class III gas giant", "Class III Gas Giant", false, 1148.921509, 995, 2_587, 4_145, 11_963),
        new("Sudarsky class IV gas giant", "Class IV Gas Giant", false, 2615.635376, 1_119, 2_910, 4_663, 13_457),
        new("Sudarsky class V gas giant", "Class V Gas Giant", false, 925.575806, 966, 2_510, 4_023, 11_609),
        new("Gas giant with ammonia based life", "Gas Giant with Ammonia-based Life", false, 170.455071, 774, 2_014, 3_227, 9_312),
        new("Gas giant with water based life", "Gas Giant with Water-based Life", false, 477.001832, 883, 2_295, 3_679, 10_616),
        new("Helium rich gas giant", "Helium-Rich Gas Giant", false, 550.141846, 900, 2_339, 3_749, 10_818),
        new("Water giant", "Water Giant", false, 47.163769, 667, 1_734, 2_779, 8_019)
    ];

    private static readonly Dictionary<string, BodyValue> ByKey =
        Values.ToDictionary(v => Key(v.PlanetClass, v.IsTerraformable), StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Exact (class, terraformable) row first; a terraformable body of a class that has no
    /// terraformable row falls back to the non-terraformable one.
    /// </summary>
    public static bool TryGet(string planetClass, bool isTerraformable, out BodyValue value) =>
        ByKey.TryGetValue(Key(planetClass, isTerraformable), out value!) ||
        ByKey.TryGetValue(Key(planetClass, false), out value!);

    public static ScanKind GetScanKind(ExplorerBody body)
    {
        var firstDiscovery = !body.WasDiscovered;
        var firstMapped = !body.WasMapped;

        if (!body.IsMapped)
            return firstDiscovery ? ScanKind.FssFirstDiscovery : ScanKind.Fss;

        // The table has no "first mapped only" column: that case is valued as plain FSS+DSS
        return firstDiscovery && firstMapped ? ScanKind.FssFirstDiscoveryDss : ScanKind.FssDss;
    }

    public static long Calculate(ExplorerBody body)
    {
        if (!TryGet(body.PlanetClass, body.IsTerraformable, out var rule))
            return 0;

        var kind = GetScanKind(body);
        var value = kind switch
        {
            ScanKind.Fss => rule.Fss,
            ScanKind.FssFirstDiscovery => rule.FssFirstDiscovery,
            ScanKind.FssDss => rule.FssDss,
            _ => rule.FssFirstDiscoveryDss
        };

        if (body.IsMapped && !body.IsEfficientMapping)
            value = (long)Math.Round(value / EfficiencyBonus);

        return value;
    }

    public static string Label(ScanKind kind) => kind switch
    {
        ScanKind.Fss => "FSS",
        ScanKind.FssFirstDiscovery => "FSS+FD",
        ScanKind.FssDss => "FSS+DSS",
        _ => "FSS+FD+DSS"
    };

    public static string Describe(ScanKind kind) => kind switch
    {
        ScanKind.Fss => "Scanned, not first discovered",
        ScanKind.FssFirstDiscovery => "Scanned, first discovered",
        ScanKind.FssDss => "Scanned and mapped, not first discovered/mapped",
        _ => "Scanned and mapped, first discovered and first mapped"
    };

    public static string DisplayName(ExplorerBody body)
    {
        if (!TryGet(body.PlanetClass, body.IsTerraformable, out var rule))
            return body.PlanetClass;

        return rule.IsTerraformable ? $"{rule.DisplayName} (TF)" : rule.DisplayName;
    }

    private static string Key(string planetClass, bool isTerraformable) =>
        isTerraformable ? planetClass + "|tf" : planetClass;
}
