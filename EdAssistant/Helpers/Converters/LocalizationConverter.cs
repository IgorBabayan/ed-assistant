namespace EdAssistant.Helpers.Converters;

public class LocalizationConverter : IValueConverter
{
    public static readonly LocalizationConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return null;
        return Localization.Instance[value.ToString()!];
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}