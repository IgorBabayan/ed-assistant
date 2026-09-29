namespace ED.Assistant.Domain.Events;

public sealed class JournalState
{
    public const int RecentEventsCapacity = 50;

    private readonly List<IJournalEvent> _recentEvents = [];

    public string? FileName { get; init; }
    public IJournalEvent? LastEvent { get; set; }

    public CommanderEvent? Commander { get; set; }
    public LoadGameEvent? LoadGame { get; set; }
    public MaterialsEvent? Materials { get; set; }
    public RankEvent? Ranks { get; set; }
    public FSDJumpEvent? FSDJump { get; set; }
    public ShipLockerEvent? ShipLocker { get; set; }

    public List<ScanOrganicEvent> Organics { get; } = [];
    public List<EvaluatorChange> PendingEvaluatorChanges { get; } = [];

    public Dictionary<int, ScanEvent> Scans { get; } = new();
    public Dictionary<int, SAASignalsFoundEvent> SAASignals { get; } = new();
    public Dictionary<int, BaryCentreEvent> BaryCentres { get; } = new();
    public Dictionary<int, FSSBodySignalsEvent> FSSSignals { get; } = new();

    /// <summary>System-level signals (USS, stations, carriers...) keyed by <see cref="FSSSignalDiscoveredEvent.Key"/>.</summary>
    public Dictionary<string, FSSSignalDiscoveredEvent> SystemSignals { get; } = new();

    /// <summary>Latest journal events, oldest first. Survives system changes.</summary>
    public IReadOnlyList<IJournalEvent> RecentEvents => _recentEvents;
    
    public JournalLog Log { get; } = new();

    public LocationEvent? Location { get; set; }

    public long? CurrentSystemAddress =>
        Location?.SystemAddress ?? FSDJump?.SystemAddress;

    public void AddRecentEvent(IJournalEvent journalEvent)
    {
        _recentEvents.Add(journalEvent);

        if (_recentEvents.Count > RecentEventsCapacity)
            _recentEvents.RemoveRange(0, _recentEvents.Count - RecentEventsCapacity);
    }

    public static string OrganicKey(long systemAddress, int bodyId, string genus, string species, string variant)
        => $"{systemAddress}:{bodyId}:{genus}:{species}:{variant}";
}