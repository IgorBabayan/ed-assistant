using System.Globalization;

namespace ED.Assistant.Extensions;

static class NumberExtensions
{
    public static string ToMillions(this int value) 
        => $"{(value / 1_000_000d).ToString("0.##", CultureInfo.InvariantCulture)} M";
}