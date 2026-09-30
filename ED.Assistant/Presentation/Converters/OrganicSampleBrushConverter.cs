using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace ED.Assistant.Presentation.Converters;

public sealed class OrganicSampleBrushConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		// A null/unset value (e.g. while the DataContext is changing) must not throw
		if (value is not int count
			|| !int.TryParse(parameter?.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var step))
		{
			return Brushes.Gray;
		}

		if (count >= step)
		{
			return step switch
			{
				1 => Brushes.Red,
				2 => Brushes.Orange,
				3 => Brushes.LimeGreen,
				_ => Brushes.Gray
			};
		}

		return Brushes.Gray;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}
