using Avalonia.Platform.Storage;
using ED.Assistant.Extensions;

namespace ED.Assistant.Application.Dialog;

class FolderPickerService : IFolderPickerService
{
	public async Task<string?> PickFolderAsync(string title)
	{
		var owner = Utils.GetMainWindow();
		var folders = await owner.StorageProvider.OpenFolderPickerAsync(
			new FolderPickerOpenOptions
			{
				Title = title,
				AllowMultiple = false
			});

		return folders.Count > 0
			? folders[0].Path.LocalPath
			: null;
	}
}
