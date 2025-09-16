namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.System)]
public sealed partial class SystemViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;
    private readonly ICelestialStructure _celestialStructure;
    private readonly ILogger<SystemViewModel> _logger;
    private readonly ITemplateCacheManager _templateCacheManager;
    private string _currentSystemName = string.Empty;

    [ObservableProperty]
    private HierarchicalTreeDataGridSource<CelestialBody>? starSystem;

    public SystemViewModel(IGameDataService gameDataService, ICelestialStructure celestialStructure, ILogger<SystemViewModel> logger, ITemplateCacheManager templateCacheManager)
    {
        _logger = logger;
        _celestialStructure = celestialStructure;
        _templateCacheManager = templateCacheManager;

        _gameDataService = gameDataService;
        _gameDataService.JournalLoaded += OnJournalEventLoaded;

        var existingData = _gameDataService.GetLatestJournals<ScanEvent>();
        if (existingData.Any())
        {
            ProcessScans(existingData);
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

        // Then build hierarchy using name-based logic
        _celestialStructure.BuildHierarchy();

        RefreshSystemDisplay();
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
                    Text = value.DisplayName,
                    VerticalAlignment = VerticalAlignment.Center,
                    UseLayoutRounding = true,
                    TextWrapping = TextWrapping.NoWrap,
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    [!TextBlock.TextProperty] = new Binding(nameof(CelestialBody.DisplayName))
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
            
            default:
                return new IconData("avares://EdAssistant/Assets/Icons/Default/Unknown.png");
        }
    }
}