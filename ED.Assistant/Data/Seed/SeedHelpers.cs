using System.Globalization;

namespace ED.Assistant.Data.Seed;

internal static class SeedHelpers
{
    public static void Atmospheres<TRule>(ModelBuilder mb, TRule rule, params AtmosphereEnum[] items) where TRule : struct, Enum =>
        mb.Entity("RuleAtmosphere").HasData(items.Select(x => new { RuleId = Convert.ToInt32(rule, CultureInfo.InvariantCulture), AtmosphereId = (int)x }));

    public static void BodyClasses<TRule>(ModelBuilder mb, TRule rule, params BodyClassEnum[] items) where TRule : struct, Enum =>
        mb.Entity("RuleBodyClass").HasData(items.Select(x => new { RuleId = Convert.ToInt32(rule, CultureInfo.InvariantCulture), BodyClassId = (int)x }));

    public static void Volcanisms<TRule>(ModelBuilder mb, TRule rule, params VolcanismEnum[] items) where TRule : struct, Enum =>
        mb.Entity("RuleVolcanism").HasData(items.Select(x => new { RuleId = Convert.ToInt32(rule, CultureInfo.InvariantCulture), VolcanismId = (int)x }));
}