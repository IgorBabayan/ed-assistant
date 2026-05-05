namespace ED.Assistant.Application.Dialog;

public interface IFolderPickerService
{
	Task<string?> PickFolderAsync(string title);
}
