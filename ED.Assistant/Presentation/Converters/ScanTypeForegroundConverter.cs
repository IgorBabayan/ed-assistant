using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using ED.Assistant.Domain.Types;
using System.Globalization;

namespace ED.Assistant.Presentation.Converters;

public sealed class ScanTypeForegroundConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		var resources = Avalonia.Application.Current?.Resources;

		if (resources is null)
			return Brushes.White;

		return value?.ToString() switch
		{
			var t when t == ScanType.AutoScan => resources["SecondaryTextBrush"] as IBrush ?? Brushes.Gray,
			var t when t == ScanType.Detailed => resources["PrimaryAccentBrush"] as IBrush ?? Brushes.DeepSkyBlue,
			_ => resources["PrimaryTextBrush"] as IBrush ?? Brushes.White
		};
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotSupportedException();
}
