using ED.Assistant.Data.Models.Events;
using ED.Assistant.Models;
using ED.Assistant.Services.Journal;
using ED.Assistant.Services.SystemBuilder;
using System.Collections.ObjectModel;

namespace ED.Assistant.ViewModels;

public partial class SystemViewModel : LoadableViewModel
{
	private readonly ISystemStructureBuilder _structureBuilder;

	[ObservableProperty]
	private FSDJumpEvent? _currentSystem;

	[ObservableProperty]
	private SystemBodyNodeViewModel? _selectedBody;

	public ObservableCollection<SystemBodyNodeViewModel> Bodies { get; } = [];

	public SystemViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		ISystemStructureBuilder structureBuilder) : base(journalLoader, stateStore) 
		=> _structureBuilder = structureBuilder;

	protected override void UpdateFromState(JournalState state)
	{
		if (state.FSDJump is null)
			return;

		CurrentSystem = state.FSDJump;
		Bodies.Clear();

		var structure = _structureBuilder.Build(state);
		foreach (var root in structure.Roots)
			Bodies.Add(new SystemBodyNodeViewModel(root));
		SelectedBody = Bodies.FirstOrDefault();
	}
}
