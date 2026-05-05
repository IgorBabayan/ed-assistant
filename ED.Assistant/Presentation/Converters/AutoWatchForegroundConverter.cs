using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace ED.Assistant.Presentation.Converters;

public sealed class AutoWatchForegroundConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value is true
			? new SolidColorBrush(Colors.Green)
			: new SolidColorBrush(Colors.Red);
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotSupportedException();
}
