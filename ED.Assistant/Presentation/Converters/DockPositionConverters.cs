using Avalonia.Data.Converters;
using Avalonia.Layout;
using ED.Assistant.Domain.Config;

namespace ED.Assistant.Presentation.Converters;

public static class DockPositionConverters
{
    public static readonly IValueConverter ToOrientation =
        new FuncValueConverter<DockPosition, Orientation>(position =>
            position == DockPosition.Bottom ? Orientation.Horizontal : Orientation.Vertical);
}