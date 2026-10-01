using System.Globalization;

namespace ED.Assistant.Plugins.Explorer;

internal static class Formatting
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    public static string Credits(long value) => value.ToString("N0", Invariant);

    public static string Compact(long value) => value switch
    {
        >= 1_000_000_000 => (value / 1_000_000_000d).ToString("0.##", Invariant) + "B",
        >= 1_000_000 => (value / 1_000_000d).ToString("0.##", Invariant) + "M",
        >= 1_000 => (value / 1_000d).ToString("0.#", Invariant) + "K",
        _ => value.ToString(Invariant)
    };
}
