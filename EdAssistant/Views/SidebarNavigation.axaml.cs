namespace EdAssistant.Views;

public partial class SidebarNavigation : UserControl
{
    public static readonly RoutedEvent<NavigationEventArgs> NavigationRequestedEvent =
        RoutedEvent.Register<SidebarNavigation, NavigationEventArgs>(
            nameof(NavigationRequested), RoutingStrategies.Bubble);

    public event EventHandler<NavigationEventArgs> NavigationRequested
    {
        add => AddHandler(NavigationRequestedEvent, value);
        remove => RemoveHandler(NavigationRequestedEvent, value);
    }

    private bool _isCollapsed;
    private Button _activeButton;

    public bool IsCollapsed
    {
        get => _isCollapsed;
        set
        {
            if (_isCollapsed != value)
            {
                _isCollapsed = value;
                if (_isCollapsed)
                    CollapseSidebar();
                else
                    ExpandSidebar();
            }
        }
    }

    public SidebarNavigation()
    {
        InitializeComponent();
        _activeButton = this.FindControl<Button>("HomeBtn")!;
    }

    private void ToggleButton_Click(object sender, RoutedEventArgs e)
    {
        _isCollapsed = !_isCollapsed;
        if (_isCollapsed)
        {
            CollapseSidebar();
        }
        else
        {
            ExpandSidebar();
        }
    }

    private void CollapseSidebar()
    {
        Width = 60;

        AddCollapsedClasses();

        var mainPanel = this.FindControl<StackPanel>("MainStackPanel");
        if (mainPanel is not null)
        {
            mainPanel.Margin = new Thickness(8, 16);
        }
    }

    private void ExpandSidebar()
    {
        Width = 280;

        RemoveCollapsedClasses();

        var mainPanel = this.FindControl<StackPanel>("MainStackPanel");
        if (mainPanel is not null)
        {
            mainPanel.Margin = new Thickness(20, 16);
        }
    }

    private void AddCollapsedClasses()
    {
        var appTitle = this.FindControl<TextBlock>("AppTitle");
        appTitle?.Classes.Add("collapsed");

        var sectionTitles = this.GetLogicalDescendants()
            .OfType<TextBlock>()
            .Where(tb => tb.Classes.Contains("section-title"));
        foreach (var title in sectionTitles)
        {
            title.Classes.Add("collapsed");
        }

        var navTexts = this.GetLogicalDescendants()
            .OfType<TextBlock>()
            .Where(tb => tb.Classes.Contains("nav-text"));
        foreach (var text in navTexts)
        {
            text.Classes.Add("collapsed");
        }

        var navButtons = this.GetLogicalDescendants()
            .OfType<Button>()
            .Where(b => b.Classes.Contains("nav-item"));
        foreach (var button in navButtons)
        {
            button.Classes.Add("collapsed");
        }
    }

    private void RemoveCollapsedClasses()
    {
        var appTitle = this.FindControl<TextBlock>("AppTitle");
        appTitle?.Classes.Remove("collapsed");

        var sectionTitles = this.GetLogicalDescendants()
            .OfType<TextBlock>()
            .Where(tb => tb.Classes.Contains("section-title"));
        foreach (var title in sectionTitles)
        {
            title.Classes.Remove("collapsed");
        }

        var navTexts = this.GetLogicalDescendants()
            .OfType<TextBlock>()
            .Where(tb => tb.Classes.Contains("nav-text"));
        foreach (var text in navTexts)
        {
            text.Classes.Remove("collapsed");
        }

        var navButtons = this.GetLogicalDescendants()
            .OfType<Button>()
            .Where(b => b.Classes.Contains("nav-item"));
        foreach (var button in navButtons)
        {
            button.Classes.Remove("collapsed");
        }
    }

    private void NavigationItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button clickedButton)
        {
            _activeButton.Classes.Remove("active");

            clickedButton.Classes.Add("active");
            _activeButton = clickedButton;

            var navigationTarget = clickedButton.Tag!.ToString();

            var args = new NavigationEventArgs(NavigationRequestedEvent, navigationTarget!);
            RaiseEvent(args);
        }
    }
}

public class NavigationEventArgs(RoutedEvent routedEvent, string navigationTarget) : RoutedEventArgs(routedEvent)
{
    public string NavigationTarget { get; } = navigationTarget;
}