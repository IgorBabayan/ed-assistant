namespace EdAssistant.Helpers.Converters;

public class BoolToYesNoConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Localization.Instance["Common.Yes"] : Localization.Instance["Common.No"];
        }

        return Localization.Instance["Common.No"];
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string strValue)
        {
            return strValue.Equals(Localization.Instance["Common.Yes"], StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }
}