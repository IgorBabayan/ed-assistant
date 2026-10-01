using System.IO;
using Avalonia.Platform.Storage;
using ED.Assistant.Extensions;

namespace ED.Assistant.Application.Dialog;

internal class FolderPickerService : IFolderPickerService
{
	public async Task<string?> PickFolderAsync(string title, Window? owner = null, string? startFolder = null)
	{
		var windowOwner = owner ?? Utils.GetMainWindow();
		var storage = windowOwner.StorageProvider;
		
		using var startLocation = await TryGetFolderAsync(storage, startFolder);
		
		var folders = await windowOwner.StorageProvider.OpenFolderPickerAsync(
			new FolderPickerOpenOptions
			{
				Title = title,
				AllowMultiple = false,
				SuggestedStartLocation = startLocation
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
	
	private static async Task<IStorageFolder?> TryGetFolderAsync(IStorageProvider storage, string? path)
	{
		if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
			return null;

		try
		{
			return await storage.TryGetFolderFromPathAsync(path);
		}
		catch (Exception)
		{
			return null;
		}
	}
}
