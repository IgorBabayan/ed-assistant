namespace EdAssistant.Helpers;

public static class WrapPanelHelper
{
    public static readonly AttachedProperty<double> SpacingXProperty =
        AvaloniaProperty.RegisterAttached<WrapPanel, double>("SpacingX", typeof(WrapPanelHelper));

    public static readonly AttachedProperty<double> SpacingYProperty =
        AvaloniaProperty.RegisterAttached<WrapPanel, double>("SpacingY", typeof(WrapPanelHelper));

    private static readonly AttachedProperty<bool> HookedProperty =
        AvaloniaProperty.RegisterAttached<WrapPanel, bool>("__Hooked", typeof(WrapPanelHelper));

    static WrapPanelHelper()
    {
        SpacingXProperty.Changed.AddClassHandler<WrapPanel>((p, _) => Apply(p));
        SpacingYProperty.Changed.AddClassHandler<WrapPanel>((p, _) => Apply(p));
    }

    private static void EnsureHooked(WrapPanel p)
    {
        if (p.GetValue(HookedProperty)) return;
        p.SetValue(HookedProperty, true);

        // Re-apply when children change or visual attaches
        p.Children.CollectionChanged += (_, _) => Apply(p);
        p.AttachedToVisualTree += (_, _) => Apply(p);
        p.DetachedFromVisualTree += (_, _) => Apply(p);
    }

    private static void Apply(WrapPanel p)
    {
        EnsureHooked(p);

        var sx = p.GetValue(SpacingXProperty);
        var sy = p.GetValue(SpacingYProperty);
        var halfX = sx * 0.5;
        var halfY = sy * 0.5;

        foreach (var c in p.Children.OfType<Control>())
            c.Margin = new Thickness(halfX, halfY, halfX, halfY);
    }

    public static double GetSpacingX(WrapPanel p) => p.GetValue(SpacingXProperty);
    public static void SetSpacingX(WrapPanel p, double v) => p.SetValue(SpacingXProperty, v);

    public static double GetSpacingY(WrapPanel p) => p.GetValue(SpacingYProperty);
    public static void SetSpacingY(WrapPanel p, double v) => p.SetValue(SpacingYProperty, v);
}