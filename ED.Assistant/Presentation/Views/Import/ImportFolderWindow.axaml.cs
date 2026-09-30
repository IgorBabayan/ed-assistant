using Avalonia.Controls;
using ED.Assistant.Presentation.ViewModels.Import;

namespace ED.Assistant.Presentation.Views.Import;

public partial class ImportFolderWindow : Window
{
    private ImportFolderViewModel? _viewModel;

    public ImportFolderWindow()
    {
        InitializeComponent();
        Opened += OnOpened;
        Closed += OnClosed;
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        if (_viewModel is not null)
        {
            _viewModel.CloseRequested -= OnCloseRequested;
        }

        base.OnDataContextChanged(e);

        _viewModel = DataContext as ImportFolderViewModel;
        if (_viewModel is not null)
        {
            _viewModel.CloseRequested += OnCloseRequested;
        }
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        PathBox.Focus();
        PathBox.SelectAll();
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        if (_viewModel is not null)
        {
            _viewModel.CloseRequested -= OnCloseRequested;
            _viewModel = null;
        }

        Opened -= OnOpened;
        Closed -= OnClosed;
    }

    private void OnCloseRequested(string? result) => Close(result);
}