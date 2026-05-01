namespace ED.Assistant.ViewModels;

public interface ILoadableViewModel
{
	IAsyncRelayCommand LoadCommand { get; }
}
