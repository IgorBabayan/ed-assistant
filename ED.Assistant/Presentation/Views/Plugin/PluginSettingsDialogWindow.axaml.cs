using ED.Assistant.Presentation.ViewModels.Plugin;

namespace ED.Assistant.Presentation.Views.Plugin;

public partial class PluginSettingsDialogWindow : Window
{
	private PluginSettingsDialogViewModel? _viewModel;

	public PluginSettingsDialogWindow()
	{
		InitializeComponent();
		Closed += OnClosed;
	}

	protected override void OnDataContextChanged(EventArgs e)
	{
		if (_viewModel is not null)
			_viewModel.CloseRequested -= OnCloseRequested;

		base.OnDataContextChanged(e);

		_viewModel = DataContext as PluginSettingsDialogViewModel;
		if (_viewModel is not null)
			_viewModel.CloseRequested += OnCloseRequested;
	}

	private void OnClosed(object? sender, EventArgs e)
	{
		if (_viewModel is not null)
		{
			_viewModel.CloseRequested -= OnCloseRequested;
			_viewModel = null;
		}

		Closed -= OnClosed;
	}

	private void OnCloseRequested(bool? result) => Close(result);
}
