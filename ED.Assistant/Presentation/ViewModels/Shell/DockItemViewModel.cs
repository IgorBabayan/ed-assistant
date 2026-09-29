using Material.Icons;
using System.Windows.Input;

namespace ED.Assistant.Presentation.ViewModels.Shell;

/// <summary>A clickable app icon in the dock.</summary>
public partial class DockItemViewModel : BaseViewModel
{
    public string Title { get; }
    public MaterialIconKind Icon { get; }
    public ICommand Command { get; }

    /// <summary>The view model this item navigates to; used to compute the active state.</summary>
    public Type TargetViewModel { get; }

    [ObservableProperty]
    public partial bool IsActive { get; set; }

    public DockItemViewModel(string title, MaterialIconKind icon, ICommand command, Type targetViewModel)
    {
        Title = title;
        Icon = icon;
        Command = command;
        TargetViewModel = targetViewModel;
    }
}

/// <summary>A divider between groups of dock items.</summary>
public sealed class DockSeparatorViewModel;