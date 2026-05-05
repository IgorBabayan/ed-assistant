using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace ED.Assistant.Presentation.Converters;

public sealed class OrganicSampleBrushConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		var step = int.Parse(parameter!.ToString()!);
		var count = (int)value!;

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
