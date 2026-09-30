using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace ED.Assistant.Presentation.Converters;

public sealed class AutoWatchForegroundConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		// Shared immutable brushes instead of a new SolidColorBrush on every binding update
		return value is true
			? Brushes.Green
			: Brushes.Red;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotSupportedException();
}
