using ED.Assistant.Services.SystemBuilder;
using System.Collections.ObjectModel;

namespace ED.Assistant.Models;

public partial class SystemBodyNodeViewModel : ObservableObject
{
	[ObservableProperty]
	private bool _isExpanded = true;

	public SystemBodyNode Model { get; }
	public string Name => Model.Name;
	public string Type => Model.Type;
	public ObservableCollection<SystemBodyNodeViewModel> Children { get; }

	public SystemBodyNodeViewModel(SystemBodyNode model)
	{
		Model = model;
		Children = new(model.Children.Select(x => new SystemBodyNodeViewModel(x)));
	}
}
