using ED.Assistant.Application.JournalLoading;
using ED.Assistant.Application.State;
using ED.Assistant.Domain.Events;
using ED.Assistant.Domain.System;
using System.Collections.ObjectModel;

namespace ED.Assistant.Presentation.ViewModels.System;

public partial class SystemViewModel : LoadableViewModel
{
	private readonly ISystemStructureBuilder _structureBuilder;

	[ObservableProperty]
	public partial FSDJumpEvent? CurrentSystem { get; set; }

	[ObservableProperty]
	public partial SystemBodyNodeViewModel? SelectedBody { get; set; }
	
	public ObservableCollection<SystemBodyNodeViewModel> Bodies { get; } = [];

	public SystemViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
		ISystemStructureBuilder structureBuilder, IMemoryCache memoryCache)
		: base(journalLoader, stateStore, memoryCache) => _structureBuilder = structureBuilder;

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
