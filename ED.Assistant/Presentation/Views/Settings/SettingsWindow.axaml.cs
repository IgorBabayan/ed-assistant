using ED.Assistant.Presentation.ViewModels.Settings;

namespace ED.Assistant.Presentation.Views.Settings;

public partial class SettingsWindow : Window
{
	private SettingsViewModel? _viewModel;

	public SettingsWindow()
	{
		InitializeComponent();
		Closed += OnClosed;
	}

	protected override void OnDataContextChanged(EventArgs e)
	{
		if (_viewModel is not null)
		{
			_viewModel.CloseRequested -= OnCloseRequested;
		}

		base.OnDataContextChanged(e);

		_viewModel = DataContext as SettingsViewModel;
		if (_viewModel is not null)
		{
			_viewModel.CloseRequested += OnCloseRequested;
		}
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