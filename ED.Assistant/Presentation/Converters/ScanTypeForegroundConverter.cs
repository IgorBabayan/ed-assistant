using Avalonia.Data.Converters;
using Avalonia.Media;
using ED.Assistant.Domain.Types;
using System.Globalization;

namespace ED.Assistant.Presentation.Converters;

public sealed class ScanTypeForegroundConverter : IValueConverter
{
	// The indexer on Application.Resources only looks at the top-level dictionary and throws
	// KeyNotFoundException for keys that live in merged dictionaries (Theme.axaml).
	// TryGetResource searches merged dictionaries and theme variants.
	private static IBrush FindBrush(string key, IBrush fallback)
	{
		var app = Avalonia.Application.Current;
		return app is not null
			   && app.TryGetResource(key, app.ActualThemeVariant, out var resource)
			   && resource is IBrush brush
			? brush
			: fallback;
	}

	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value?.ToString() switch
		{
			var t when t == ScanType.AutoScan => FindBrush("SecondaryTextBrush", Brushes.Gray),
			var t when t == ScanType.Detailed => FindBrush("PrimaryAccentBrush", Brushes.DeepSkyBlue),
			_ => FindBrush("PrimaryTextBrush", Brushes.White)
		};
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> throw new NotSupportedException();
}
