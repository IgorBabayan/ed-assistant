namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.System)]
public sealed partial class SystemViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;
    private readonly ICelestialStructure _celestialStructure;
    private readonly ILogger<SystemViewModel> _logger;
    private readonly ITemplateCacheManager _templateCacheManager;
    private readonly IResourceService _resourceService;
    private string _currentSystemName = string.Empty;

    [ObservableProperty]
    private HierarchicalTreeDataGridSource<CelestialBody>? starSystem;

    public SystemViewModel(IGameDataService gameDataService, ICelestialStructure celestialStructure,
        ILogger<SystemViewModel> logger, ITemplateCacheManager templateCacheManager, IResourceService  resourceService)
    {
        _logger = logger;
        _celestialStructure = celestialStructure;
        _templateCacheManager = templateCacheManager;
        _resourceService = resourceService;

        _gameDataService = gameDataService;
        _gameDataService.JournalLoaded += OnJournalEventLoaded;

        var existingData = _gameDataService.GetLatestJournals<ScanEvent>();
        if (existingData.Any())
        {
            ProcessScans(existingData);

            var existingFSSScans = _gameDataService.GetLatestJournals<FSSSignalDiscoveredEvent>();
            if (existingFSSScans.Any())
            {
                ProcessFSSScans(existingFSSScans);
            }
        }
        
        var completedScanData = _gameDataService.GetLatestJournals<SAAScanCompleteEvent>();
        if (completedScanData.Any())
        {
            ProcessCompletedScans(completedScanData);
        }
    }

    public override void Dispose()
    {
        _gameDataService.JournalLoaded -= OnJournalEventLoaded;
        _templateCacheManager.ClearExpired();
    }

    private void OnJournalEventLoaded(object? sender, JournalEventLoadedEventArgs e)
    {
        if (e is { EventType: JournalEventType.Scan, Event: ScanEvent scanEvent })
        {
            // Check if we need to switch to a new system
            if (!string.Equals(_currentSystemName, scanEvent.StarSystem, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(Localization.Instance["ScanProcess.SystemChanged"], _currentSystemName, scanEvent.StarSystem);
                _currentSystemName = scanEvent.StarSystem;

                // Process all scans for the new system
                var systemScans = _gameDataService.GetLatestJournals<ScanEvent>()
                    .Where(s => string.Equals(s.StarSystem, _currentSystemName, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (systemScans.Any())
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

        if (e is { EventType: JournalEventType.FSSSignalDiscovered, Event: FSSSignalDiscoveredEvent fssSignalEvent })
        {
            if (_celestialStructure.SystemAddress == fssSignalEvent.SystemAddress)
            {
                _celestialStructure.AddFSSSignalDiscoveredEvent(fssSignalEvent);
            }
        }
    }

    private void ProcessFSSScans(IList<FSSSignalDiscoveredEvent> scans)
    {
        foreach (var scan in scans)
        {
            _celestialStructure.AddFSSSignalDiscoveredEvent(scan);
        }
        
        if (_celestialStructure.SystemRoot?.Children?.Any() == true)
        {
            _celestialStructure.BuildHierarchy();
            RefreshSystemDisplay();
        }
    }

    private void ProcessCompletedScans(IList<SAAScanCompleteEvent> scans)
    {
        SetDefaultColorRecursive(_celestialStructure.SystemRoot);
        
        foreach (var scan in scans.Where(x => _celestialStructure.SystemAddress == x.SystemAddress))
        {
            var body = FindCelestialBodyByBodyId(scan.BodyId);
            if (body is not null)
            {
                if (scan.ProbesUsed < scan.EfficiencyTarget)
                {
                    body.ForegroundBrush = _resourceService.GetBrush("Success.Brush");
                } else if (scan.ProbesUsed == scan.EfficiencyTarget)
                {
                    body.ForegroundBrush = _resourceService.GetBrush("Warning.Brush");
                }
                else
                {
                    body.ForegroundBrush = _resourceService.GetBrush("Danger.Brush");
                }    
            }
        }
    }
    
    private CelestialBody? FindCelestialBodyByBodyId(int bodyId) => 
        FindBodyRecursive(_celestialStructure.SystemRoot, bodyId);

    private CelestialBody? FindBodyRecursive(CelestialBody? root, int bodyId)
    {
        if (root is null)
            return null;
    
        if (root.BodyId == bodyId)
            return root;

        foreach (var child in root.SubItems)
        {
            var result = FindBodyRecursive(child, bodyId);
            if (result is not null)
                return result;
        }

        return null;
    }

    private void ProcessScans(IList<ScanEvent> scans)
    {
        // Get the most recent system from the scans
        var latestScan = scans.OrderByDescending(s => s.Timestamp).FirstOrDefault();
        if (latestScan is not null)
        {
            _currentSystemName = latestScan.StarSystem;
            _logger.LogInformation(Localization.Instance["ScanProcess.ProcessingScans"], _currentSystemName);
        }

        // Filter scans to only include those from the current system
        var systemScans = scans.Where(s => string.Equals(s.StarSystem, _currentSystemName, StringComparison.OrdinalIgnoreCase)).ToList();

        // Remove duplicates based on BodyId
        var uniqueScans = systemScans
            .GroupBy(s => s.BodyId)
            .Select(g => g.First()) // Take first occurrence of each BodyId
            .OrderBy(s => s.BodyId)
            .ToList();

        _logger.LogInformation(Localization.Instance["ScanProcess.ProcessingUniqueScan"], uniqueScans.Count, _currentSystemName, systemScans.Count - uniqueScans.Count);

        // Add all scans first
        foreach (var scan in uniqueScans)
        {
            _celestialStructure.AddScanEvent(scan);
        }
        
        var existingFSSScans = _gameDataService.GetLatestJournals<FSSSignalDiscoveredEvent>();
        if (existingFSSScans.Any())
        {
            var systemFSSScans = existingFSSScans
                .Where(f => f.SystemAddress == latestScan?.SystemAddress)
                .ToList();
            
            foreach (var fssScan in systemFSSScans)
            {
                _celestialStructure.AddFSSSignalDiscoveredEvent(fssScan);
            }
        }

        // Then build hierarchy using name-based logic
        _celestialStructure.BuildHierarchy();
        RefreshSystemDisplay();
        
        SetDefaultColorRecursive(_celestialStructure.SystemRoot);
    }
    
    private void SetDefaultColorRecursive(CelestialBody body)
    {
        body.ForegroundBrush ??= _resourceService.GetBrush("Text.Primary");
    
        foreach (var child in body.SubItems)
        {
            SetDefaultColorRecursive(child);
        }
    }

    private void RefreshSystemDisplay()
    {
        var systemRootCollection = new List<CelestialBody> { _celestialStructure.SystemRoot };
        StarSystem = new HierarchicalTreeDataGridSource<CelestialBody>(systemRootCollection)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<CelestialBody>(
                    new TemplateColumn<CelestialBody>("Name", 
                        GetNameColumnTemplate()),
                    x => x.SubItems),
                new TextColumn<CelestialBody, string>("Type", x => x.TypeInfo),
                new TextColumn<CelestialBody, string>("Distance", x => x.DistanceInfo),
                new TextColumn<CelestialBody, string>("Status", x => x.StatusInfo),
                new TextColumn<CelestialBody, string>("Landable", x => x.LandableInfo),
                new TextColumn<CelestialBody, string>("Mass", x => x.MassInfo)
            }
        };
        
        SetDefaultColorRecursive(_celestialStructure.SystemRoot);
    }
    
    private IDataTemplate GetNameColumnTemplate() =>
        _templateCacheManager.GetOrCreateTemplate("CelestialBodyNameTemplate", () =>
            new FuncDataTemplate<CelestialBody>((value, _) =>
            {
                var stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 4
                };

                var iconData = GetCachedIconForCelestialBody(value);
                var icon = new Image
                {
                    Margin = new Thickness(0, 0, 4, 0),
                    UseLayoutRounding = true,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 28,
                    Height = 28,
                    Opacity = iconData.Opacity,
                    Source = new Bitmap(AssetLoader.Open(new Uri(iconData.Path)))
                };

                var nameText = new TextBlock
                {
                    Text = value?.DisplayName,
                    VerticalAlignment = VerticalAlignment.Center,
                    UseLayoutRounding = true,
                    TextWrapping = TextWrapping.NoWrap,
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    [!TextBlock.TextProperty] = new Binding(nameof(CelestialBody.DisplayName)),
                    [!TextBlock.ForegroundProperty] = new Binding(nameof(CelestialBody.ForegroundBrush))
                };

                stackPanel.Children.Add(icon);
                stackPanel.Children.Add(nameText);

                return stackPanel;
            }));

    private IconData GetCachedIconForCelestialBody(CelestialBody? body)
    {
        if (body?.TypeInfo is null)
            return new IconData("avares://EdAssistant/Assets/Icons/Default/Unknown.png");

        var cacheKey = $"icon_{body.TypeInfo.ToLowerInvariant()}";
        return _templateCacheManager.GetOrCreateIcon(cacheKey, () => 
            GetIconForCelestialBodyType(body));
    }

    private static IconData GetIconForCelestialBodyType(CelestialBody body)
    {
        switch (body)
        {
            case SystemNode:
                return new IconData("avares://EdAssistant/Assets/Icons/Star/System.png");
            
            case Star:
                return new IconData("avares://EdAssistant/Assets/Icons/Star/Star.png");

            case Planet planet:
            {
                var isLandable = planet.Landable!.Value;
                var fileName = isLandable ? "Landable" : "Non-Landable";
                return new IconData($"avares://EdAssistant/Assets/Icons/Star/{fileName}.png");
            }
            
            case BeltCluster:
            case Ring:
                return new IconData("avares://EdAssistant/Assets/Icons/Star/Asteroid.png");
            
            case Outpost:
                return new IconData("avares://EdAssistant/Assets/Icons/Station/Outpost.png");
            
            case Asteroid:
                return new IconData("avares://EdAssistant/Assets/Icons/Station/AsteroidBase.png");
            
            case Coriolis:
                return new IconData("avares://EdAssistant/Assets/Icons/Station/Coriolis.png");
            
            case Orbis:
                return new IconData("avares://EdAssistant/Assets/Icons/Station/Orbis.png");
            
            case Ocellus:
                return new IconData("avares://EdAssistant/Assets/Icons/Station/Ocellus.png");
            
            default:
                return new IconData("avares://EdAssistant/Assets/Icons/Default/Unknown.png");
        }
    }
}