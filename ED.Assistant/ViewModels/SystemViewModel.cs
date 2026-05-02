using ED.Assistant.Data.Models.Events;
using ED.Assistant.Data.Services.Events;
using ED.Assistant.Data.Services.Path;
using ED.Assistant.Models;
using ED.Assistant.Services.Journal;
using ED.Assistant.Services.SystemBuilder;
using System.Collections.ObjectModel;

namespace ED.Assistant.ViewModels;

public partial class SystemViewModel : BaseViewModel, ILoadableViewModel
{
	private readonly ISystemStructureBuilder _structureBuilder;

	[ObservableProperty]
	private FSDJumpEvent? _currentSystem;

	[ObservableProperty]
	private SystemBodyNodeViewModel? _selectedBody;

	public ObservableCollection<SystemBodyNodeViewModel> Bodies { get; } = [];

	public SystemViewModel(ILogStorage logStorage, IPathFinder pathFinder,
		IJournalStateStore stateStore, ISystemStructureBuilder structureBuilder)
		: base(logStorage, pathFinder, stateStore) => _structureBuilder = structureBuilder;

	protected override void UpdateFromState(JournalState state)
	{
		if (state.FSDJump is null)
			return;

		CurrentSystem = state.FSDJump;
		Bodies.Clear();
		
		var structure = _structureBuilder.Build(state);
	}

	[RelayCommand]
	private async Task Load(CancellationToken cancellationToken = default)
	{
		var folder = _pathFinder.GetPathToLogs();
		var state = await _logStorage.LoadLastLogsAsync(folder, cancellationToken);

		_stateStore.Update(state);
	}
}
