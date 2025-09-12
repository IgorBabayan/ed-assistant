namespace EdAssistant.Views;

public partial class SidebarNavigation : UserControl
{
    private bool _isCollapsed;
    private Button _activeButton;

    public SidebarNavigation()
    {
        InitializeComponent();
        _activeButton = this.FindControl<Button>("HomeBtn")!;

        InitializeHeaderState();
    }

    private void InitializeHeaderState()
    {
        var expandedHeader = this.FindControl<Grid>("ExpandedHeader");
        var collapsedButton = this.FindControl<Button>("CollapsedToggleButton");

        if (expandedHeader is not null)
            expandedHeader.IsVisible = true;
        
        if (collapsedButton is not null)
            collapsedButton.IsVisible = false;

        Width = 280;

        var mainPanel = this.FindControl<StackPanel>("MainStackPanel");
        if (mainPanel is not null)
        {
            mainPanel.Margin = new Thickness(20, 16);
        }
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

        var headerBorder = this.FindControl<Border>("HeaderBorder");
        if (headerBorder is not null)
        {
            headerBorder.Padding = new Thickness(12, 16);
        }

        var expandedHeader = this.FindControl<Grid>("ExpandedHeader");
        var collapsedButton = this.FindControl<Button>("CollapsedToggleButton");

        if (expandedHeader is not null)
            expandedHeader.IsVisible = false;
        
        if (collapsedButton is not null)
            collapsedButton.IsVisible = true;

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

        var headerBorder = this.FindControl<Border>("HeaderBorder");
        if (headerBorder is not null)
        {
            headerBorder.Padding = new Thickness(16, 16, 8, 16);
        }

        var expandedHeader = this.FindControl<Grid>("ExpandedHeader");
        var collapsedButton = this.FindControl<Button>("CollapsedToggleButton");

        if (expandedHeader is not null)
            expandedHeader.IsVisible = true;
        
        if (collapsedButton is not null)
            collapsedButton.IsVisible = false;

        RemoveCollapsedClasses();

        var mainPanel = this.FindControl<StackPanel>("MainStackPanel");
        if (mainPanel is not null)
        {
            mainPanel.Margin = new Thickness(20, 16);
        }
    }

    private void AddCollapsedClasses()
    {
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
        }
    }
}