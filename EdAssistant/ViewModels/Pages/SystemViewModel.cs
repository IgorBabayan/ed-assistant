namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.System)]
public sealed partial class SystemViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;
    private readonly ICelestialStructure _celestialStructure;
    private string _currentSystemName = string.Empty;

    [ObservableProperty]
    private HierarchicalTreeDataGridSource<CelestialBody>? starSystem;

    public SystemViewModel(IGameDataService gameDataService, ICelestialStructure celestialStructure)
    {
        _celestialStructure = celestialStructure;

        _gameDataService = gameDataService;
        _gameDataService.JournalLoaded += OnJournalEventLoaded;

        var existingData = _gameDataService.GetLatestJournals<ScanEvent>();
        if (existingData?.Any() == true)
        {
            ProcessScans(existingData);
        }
    }

    public override void Dispose() => _gameDataService.JournalLoaded -= OnJournalEventLoaded;

    private void OnJournalEventLoaded(object? sender, JournalEventLoadedEventArgs e)
    {
        if (e is { EventType: JournalEventType.Scan, Event: ScanEvent scanEvent })
        {
            // Check if we need to switch to a new system
            if (_currentSystemName != scanEvent.StarSystem)
            {
                Console.WriteLine($"System changed from '{_currentSystemName}' to '{scanEvent.StarSystem}'");
                _currentSystemName = scanEvent.StarSystem;

                // Process all scans for the new system
                var systemScans = _gameDataService.GetLatestJournals<ScanEvent>()
                    ?.Where(s => s.StarSystem == _currentSystemName)
                    .ToList();

                if (systemScans?.Any() == true)
                {
                    ProcessScans(systemScans);
                }
            }
            else
            {
                // Add new scan to existing system
                _celestialStructure.AddScanEvent(scanEvent);
                _celestialStructure.BuildHierarchy();
                RefreshSystemDisplay();
            }
        }
    }

    private void ProcessScans(IEnumerable<ScanEvent> scans)
    {
        // Get the most recent system from the scans
        var latestScan = scans.OrderByDescending(s => s.Timestamp).FirstOrDefault();
        if (latestScan != null)
        {
            _currentSystemName = latestScan.StarSystem;
            Console.WriteLine($"Processing scans for system: {_currentSystemName}");
        }

        // Filter scans to only include those from the current system
        var systemScans = scans.Where(s => s.StarSystem == _currentSystemName).ToList();

        // Remove duplicates based on BodyId
        var uniqueScans = systemScans
            .GroupBy(s => s.BodyId)
            .Select(g => g.First()) // Take first occurrence of each BodyId
            .OrderBy(s => s.BodyId)
            .ToList();

        Console.WriteLine($"Processing {uniqueScans.Count} unique scan events for system '{_currentSystemName}' (removed {systemScans.Count - uniqueScans.Count} duplicates)...");

        // Add all scans first
        foreach (var scan in uniqueScans)
        {
            _celestialStructure.AddScanEvent(scan);
        }

        // Then build hierarchy using name-based logic
        _celestialStructure.BuildHierarchy();

        RefreshSystemDisplay();
    }

    private void RefreshSystemDisplay()
    {
        // Only create TreeDataGrid if we have a system
        if (_celestialStructure.SystemRoot != null)
        {
            // Create a collection with the system root as the single item
            var systemRootCollection = new List<CelestialBody> { _celestialStructure.SystemRoot };

            StarSystem = new HierarchicalTreeDataGridSource<CelestialBody>(systemRootCollection)
            {
                Columns =
                {
                    new HierarchicalExpanderColumn<CelestialBody>(
                        new TextColumn<CelestialBody, string>("Name", x => x.DisplayName),
                        x => x.SubItems),
                    new TextColumn<CelestialBody, string>("Type", x => x.TypeInfo),
                    new TextColumn<CelestialBody, string>("Distance", x => x.DistanceInfo),
                    new TextColumn<CelestialBody, string>("Status", x => x.StatusInfo),
                    new TextColumn<CelestialBody, string>("Landable", x => x.LandableInfo),
                    new TextColumn<CelestialBody, string>("Mass", x => x.MassInfo)
                }
            };
        }
    }

    // Method to manually set the current system (useful for testing or when user selects a system)
    public void SetCurrentSystem(string systemName)
    {
        if (_currentSystemName != systemName)
        {
            _currentSystemName = systemName;

            var systemScans = _gameDataService.GetLatestJournals<ScanEvent>()
                ?.Where(s => s.StarSystem == systemName)
                .ToList();

            if (systemScans?.Any() == true)
            {
                ProcessScans(systemScans);
            }
        }
    }
}