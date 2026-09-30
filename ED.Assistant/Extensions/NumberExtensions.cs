using System.Globalization;

namespace ED.Assistant.Extensions;

static class NumberExtensions
{
    public static string ToMillions(this decimal value) 
        => $"{(value / 1_000_000m).ToString("0.##", CultureInfo.InvariantCulture)} M";
    
    public static string ToCompact(this decimal value)
    {
        var millions = Math.Round(value / 1_000_000m, 2);

        return millions >= 1000m
            ? $"{(millions / 1000m).ToString("0.##", CultureInfo.InvariantCulture)} B"
            : $"{millions.ToString("0.##", CultureInfo.InvariantCulture)} M";
    }
}