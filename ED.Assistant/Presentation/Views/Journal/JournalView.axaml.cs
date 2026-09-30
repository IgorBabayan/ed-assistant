using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using ED.Assistant.Presentation.ViewModels.Journal;

namespace ED.Assistant.Presentation.Views.Journal;

public partial class JournalView : UserControl
{
    private const int MAX_ANCHOR_PASSES = 3;
    
    private JournalViewModel? _viewModel;

    public JournalView()
    {
        InitializeComponent();
        
        LogItems.AddHandler(Button.ClickEvent, OnRowHeaderClick,
            RoutingStrategies.Bubble, handledEventsToo: true);
    }
    
    private void OnRowHeaderClick(object? sender, RoutedEventArgs e)
    {
        if (e.Source is not Control source
            || source.FindAncestorOfType<Button>(includeSelf: true) is not { } header
            || !header.Classes.Contains("journal-row-header")
            || header.DataContext is not JournalEntryViewModel entry
            || LogItems.ContainerFromItem(entry) is not Control container
            || container.TranslatePoint(default, LogScroll) is not { } before)
            return;

        Dispatcher.UIThread.Post(() => RestoreAnchor(entry, before.Y, 0), DispatcherPriority.Background);
    }
    
    private void RestoreAnchor(JournalEntryViewModel entry, double targetY, int pass)
    {
        void ScheduleNextPass()
        {
            if (pass + 1 < MAX_ANCHOR_PASSES)
                Dispatcher.UIThread.Post(() => RestoreAnchor(entry, targetY, pass + 1),
                    DispatcherPriority.Background);
        }
        
        var index = LogItems.Items.IndexOf(entry);
        if (index < 0)
            return;

        if (LogItems.ContainerFromIndex(index) is not Control container)
        {
            // The jump pushed the row out of the realized range; bring it back, then fine-tune
            LogItems.ScrollIntoView(index);
            ScheduleNextPass();
            return;
        }

        if (container.TranslatePoint(default, LogScroll) is not { } now)
            return;

        var delta = now.Y - targetY;
        if (Math.Abs(delta) < 0.5)
            return;

        LogScroll.Offset = LogScroll.Offset.WithY(LogScroll.Offset.Y + delta);

        // Re-realizing rows can refine the estimate once more, so check again
        ScheduleNextPass();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        Attach(DataContext as JournalViewModel);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        Attach(DataContext as JournalViewModel);

        if (_viewModel?.IsFollowing == true)
            ScrollToEnd();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        // The view is recreated on every navigation; don't leak it through the VM event
        Attach(null);
        base.OnDetachedFromVisualTree(e);
    }

    private void Attach(JournalViewModel? viewModel)
    {
        if (ReferenceEquals(_viewModel, viewModel))
            return;

        if (_viewModel is not null)
            _viewModel.ScrollToEndRequested -= OnScrollToEndRequested;

        _viewModel = viewModel;

        if (_viewModel is not null)
            _viewModel.ScrollToEndRequested += OnScrollToEndRequested;
    }

    private void OnScrollToEndRequested(object? sender, EventArgs e) => ScrollToEnd();

    // Background priority: let the new items get measured before scrolling.
    // ScrollIntoView is reliable with VirtualizingStackPanel, where the extent is only an estimate.
    private void ScrollToEnd() => Dispatcher.UIThread.Post(() =>
    {
        var count = LogItems.ItemCount;
        if (count > 0)
            LogItems.ScrollIntoView(count - 1);
    }, DispatcherPriority.Background);
}