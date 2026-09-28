using System.Globalization;
using Avalonia.Data.Converters;

namespace ED.Assistant.Presentation.Converters;

public class BioCostToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not long cost)
            return Brushes.White;

        if (cost > 10_000_000)
            return Brushes.LimeGreen;

        if (cost >= 5_000_000)
            return Brushes.Gold;

        return Brushes.White;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}