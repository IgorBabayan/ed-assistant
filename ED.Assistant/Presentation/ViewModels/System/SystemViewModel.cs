using ED.Assistant.Domain.System;
using ED.Assistant.Presentation.Collections;

namespace ED.Assistant.Presentation.ViewModels.System;

public partial class SystemViewModel : LoadableViewModel
{
    private readonly ISystemStructureBuilder _structureBuilder;

    private long? _lastAddress;
    private ScanEvent[] _lastScans = [];
    private FSSBodySignalsEvent[] _lastSignals = [];

    [ObservableProperty]
    public partial SystemBodyNodeViewModel? SelectedBody { get; set; }
	
    public BulkObservableCollection<SystemBodyNodeViewModel> Bodies { get; } = [];

    public SystemViewModel(IJournalLoaderService journalLoader, IJournalStateStore stateStore,
        ISystemStructureBuilder structureBuilder, IMemoryCache memoryCache)
        : base(journalLoader, stateStore, memoryCache) => _structureBuilder = structureBuilder;

    protected override void UpdateFromState(JournalState state)
    {
        var address = state.CurrentSystemAddress;
        var scans = state.Scans.Values.ToArray();
        var signals = state.FSSSignals.Values.ToArray();
        // Replacements can change scan/signal details without changing dictionary counts.
        if (address == _lastAddress && scans.SequenceEqual(_lastScans) && signals.SequenceEqual(_lastSignals))
            return;

        // Build on the calling thread while the state is consistent, apply on the UI thread
        var structure = _structureBuilder.Build(state);
        var bodies = structure.Roots
            .Select(root => new SystemBodyNodeViewModel(root))
            .ToList();

        var sameSystem = address == _lastAddress;
        _lastAddress = address;
        _lastScans = scans;
        _lastSignals = signals;

        RunOnUIThread(() =>
        {
            var selectedId = sameSystem ? SelectedBody?.BodyId : null;
            Bodies.ReplaceAll(bodies);
            SelectedBody = FindBody(bodies, selectedId) ?? Bodies.FirstOrDefault();
        });
    }
    private static SystemBodyNodeViewModel? FindBody(IEnumerable<SystemBodyNodeViewModel> bodies, int? id)
    {
        if (id is null)
            return null;
        foreach (var body in bodies)
        {
            if (body.BodyId == id)
                return body;
            if (FindBody(body.Children, id) is { } child)
                return child;
        }
        return null;
    }
}
