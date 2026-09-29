using ED.Assistant.Domain.System;
using ED.Assistant.Presentation.Collections;

namespace ED.Assistant.Presentation.ViewModels.System;

public partial class SystemViewModel : LoadableViewModel
{
    private readonly ISystemStructureBuilder _structureBuilder;

    // What the current tree was built from; most journal lines don't touch any of it
    private FSDJumpEvent? _lastJump;
    private int _lastScanCount = -1;
    private int _lastSignalCount = -1;

    [ObservableProperty]
    public partial FSDJumpEvent? CurrentSystem { get; set; }

    [ObservableProperty]
    public partial SystemBodyNodeViewModel? SelectedBody { get; set; }
	
    public BulkObservableCollection<SystemBodyNodeViewModel> Bodies { get; } = new();

    public SystemViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
        ISystemStructureBuilder structureBuilder, IMemoryCache memoryCache)
        : base(journalLoader, stateStore, memoryCache) => _structureBuilder = structureBuilder;

    protected override void UpdateFromState(JournalState state)
    {
        var jump = state.FSDJump;
        if (jump is null)
            return;

        var scanCount = state.Scans.Count;
        var signalCount = state.FSSSignals.Count;

        if (ReferenceEquals(jump, _lastJump) &&
            scanCount == _lastScanCount &&
            signalCount == _lastSignalCount)
        {
            return;
        }

        // Build on the calling thread while the state is consistent, apply on the UI thread
        var structure = _structureBuilder.Build(state);
        var bodies = structure.Roots
            .Select(root => new SystemBodyNodeViewModel(root))
            .ToList();

        _lastJump = jump;
        _lastScanCount = scanCount;
        _lastSignalCount = signalCount;

        RunOnUIThread(() =>
        {
            CurrentSystem = jump;
            Bodies.ReplaceAll(bodies);
            SelectedBody = Bodies.FirstOrDefault();
        });
    }
}