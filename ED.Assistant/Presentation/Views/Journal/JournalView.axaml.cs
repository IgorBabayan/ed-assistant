using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using ED.Assistant.Presentation.ViewModels.Journal;

namespace ED.Assistant.Presentation.Views.Journal;

public partial class JournalView : UserControl
{
    private JournalViewModel? _viewModel;

    public JournalView() => InitializeComponent();

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