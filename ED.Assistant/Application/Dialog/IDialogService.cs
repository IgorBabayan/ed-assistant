namespace ED.Assistant.Application.Dialog;

public interface IDialogService
{
	Task<TResult?> ShowDialogAsync<TViewModel, TResult>(TViewModel viewModel)
		   where TViewModel : BaseViewModel;
}
