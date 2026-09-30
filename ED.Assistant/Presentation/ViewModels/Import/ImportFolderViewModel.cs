using System.IO;
using ED.Assistant.Application.Dialog;

namespace ED.Assistant.Presentation.ViewModels.Import;

/// <summary>
/// Text-field replacement for the native folder picker, used where the picker
/// isn't available (Hyprland). The dialog result is the full folder path, or null.
/// </summary>
public partial class ImportFolderViewModel : BaseViewModel
{
    private readonly IFolderPickerService _folderPickerService;
    
    [ObservableProperty]
    public partial string FolderPath { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    public partial string? ErrorMessage { get; set; }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public event Action<string?>? CloseRequested;

    public ImportFolderViewModel(IFolderPickerService folderPickerService, string? initialPath = null)
    {
        _folderPickerService = folderPickerService;
        FolderPath = initialPath ?? string.Empty;
    }

    partial void OnFolderPathChanged(string value) => ErrorMessage = null;

    [RelayCommand]
    private void Import()
    {
        var path = ExpandHome(FolderPath?.Trim() ?? string.Empty);

        if (string.IsNullOrEmpty(path))
        {
            ErrorMessage = "Enter the path to the folder with journal logs.";
            return;
        }

        if (!Directory.Exists(path))
        {
            ErrorMessage = $"Folder '{path}' does not exist.";
            return;
        }

        if (!Directory.EnumerateFiles(path, "Journal.*.log").Any())
        {
            ErrorMessage = "No Journal.*.log files found in this folder.";
            return;
        }

        CloseRequested?.Invoke(IOPath.GetFullPath(path));
    }

    [RelayCommand]
    private void Cancel() => CloseRequested?.Invoke(null);

    // Paths typed by hand on Linux usually start with ~
    private static string ExpandHome(string path)
    {
        if (path != "~" && !path.StartsWith("~/", StringComparison.Ordinal))
            return path;

        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return IOPath.Join(home, path.TrimStart('~').TrimStart('/'));
    }
    
    [RelayCommand]
    private async Task OpenFolder(Window? owner)
    {
        var folder = await _folderPickerService.PickFolderAsync("Select Elite Dangerous log folder");
        if (folder is not null)
        {
            FolderPath = folder;
        }
    }
}