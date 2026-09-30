using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ED.Assistant.Presentation.Converters;

public sealed class BiologyImageConverter : IValueConverter
{
    private static readonly ConcurrentDictionary<string, Bitmap> Cache = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int id)
            return null;

        var folder = GetBiologyFolder(id);

        if (folder is null)
            return null;

        var path = $"avares://ED.Assistant/Assets/Biology/{folder}/{id}.webp";
        var uri = new Uri(path);

        if (!AssetLoader.Exists(uri))
        {
            Debug.WriteLine($"[BIO IMAGE] NOT FOUND: {uri}");
            return null;
        }

        return Cache.GetOrAdd(
            path,
            _ =>
            {
                using var stream = AssetLoader.Open(uri);
                return new Bitmap(stream);
            });
    }

    public object ConvertBack(object? value, Type targetType, object? parameter,
        CultureInfo culture) => throw new NotSupportedException();
    
    private static string? GetBiologyFolder(int id) =>
        (id / 100) switch
        {
            10 => "Aleoida",
            11 => "Anemone",
            12 => "Bacterium",
            13 => "BrainTree",
            14 => "Cactoida",
            15 => "Clypeus",
            16 => "Concha",
            17 => "Electricae",
            18 => "Fonticulua",
            19 => "Frutexa",
            20 => "Fumerola",
            21 => "Fungoida",
            22 => "Osseus",
            23 => "Recepta",
            24 => "Crystalline Shard",
            25 => "Stratum",
            26 => "Tuber",
            27 => "Tubus",
            28 => "Tussock",

            _ => null
        };
}