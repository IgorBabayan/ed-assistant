using Avalonia.Data.Converters;
using System;
using System.Globalization;
using EdAssistant.Helpers.Extensions;

namespace EdAssistant.Helpers.Converters;

public class EnumLocalizationConverter : IValueConverter
{
    public static readonly EnumLocalizationConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            return enumValue.GetLocalizedDisplayName();
        }

        return value?.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}