using Avalonia.Platform.Storage;
using ED.Assistant.Extensions;

namespace ED.Assistant.Application.Dialog;

internal class FolderPickerService : IFolderPickerService
{
	public async Task<string?> PickFolderAsync(string title, Window? owner = null)
	{
		var windowOwner = owner ?? Utils.GetMainWindow();
		var folders = await windowOwner.StorageProvider.OpenFolderPickerAsync(
			new FolderPickerOpenOptions
			{
				Title = title,
				AllowMultiple = false
			});

		try
		{
			return folders.Count > 0 ? folders[0].TryGetLocalPath() : null;
		}
		finally
		{
			foreach (var folder in folders)
				folder.Dispose();
		}
	}
}
