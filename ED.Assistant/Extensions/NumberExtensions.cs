using System.Globalization;

namespace ED.Assistant.Extensions;

internal static class NumberExtensions
{
    extension(decimal value)
    {
        public string ToMillions()
            => $"{(value / 1_000_000m).ToString("0.##", CultureInfo.InvariantCulture)} M";

        public string ToCompact()
        {
            var millions = Math.Round(value / 1_000_000m, 2);

            return millions >= 1000m
                ? $"{(millions / 1000m).ToString("0.##", CultureInfo.InvariantCulture)} B"
                : $"{millions.ToString("0.##", CultureInfo.InvariantCulture)} M";
        }
    }
}