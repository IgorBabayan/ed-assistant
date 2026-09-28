using System.Globalization;

namespace ED.Assistant.Extensions;

static class NumberExtensions
{
    public static string ToMillions(this decimal value) 
        => $"{(value / 1_000_000m).ToString("0.##", CultureInfo.InvariantCulture)} M";
}