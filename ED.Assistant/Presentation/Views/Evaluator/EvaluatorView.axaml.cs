namespace ED.Assistant.Presentation.Views.Evaluator;

public partial class EvaluatorView : UserControl
{
    // Start fetching the next page this far (px) before reaching the bottom
    private const double LoadMoreThreshold = 300;

    public EvaluatorView()
    {
        InitializeComponent();

        // ScrollChanged bubbles, so the list's templated ScrollViewer can be observed from here
        AddHandler(ScrollViewer.ScrollChangedEvent, OnScrollChanged);
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (e.Source is not ScrollViewer { TemplatedParent: ItemsControl } viewer ||
            DataContext is not global::ED.Assistant.Presentation.ViewModels.Evaluator.EvaluatorViewModel vm)
            return;

        // Also raised when the extent changes, so a page that doesn't fill the viewport pulls the next one
        if (viewer.Offset.Y + viewer.Viewport.Height < viewer.Extent.Height - LoadMoreThreshold)
            return;

        // The async command refuses to run again while a page is still loading
        if (vm.LoadMoreCommand.CanExecute(null))
            vm.LoadMoreCommand.Execute(null);
    }
}