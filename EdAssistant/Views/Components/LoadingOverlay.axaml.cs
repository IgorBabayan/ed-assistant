namespace EdAssistant.Views.Components;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]

public partial class LoadingOverlay : UserControl
{
    public static readonly StyledProperty<string> LoadingTextProperty =
        AvaloniaProperty.Register<LoadingOverlay, string>(nameof(LoadingText), 
            defaultValue: Localization.Instance["Common.Loading"]);
    
    public static readonly StyledProperty<bool> IsLoadingProperty =
        AvaloniaProperty.Register<LoadingOverlay, bool>(nameof(IsLoading));
    
    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }
    
    public string LoadingText
    {
        get => GetValue(LoadingTextProperty);
        set => SetValue(LoadingTextProperty, value);
    }
    
    public LoadingOverlay() => InitializeComponent();
}
