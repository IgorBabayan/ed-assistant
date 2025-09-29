namespace EdAssistant.ViewModels.Pages;

public sealed partial class SystemViewModel(IJournalService journalService, ILogger<SystemViewModel> logger,
    ICelestialStructure celestialStructure, ITemplateCacheManager templateCacheManager, IResourceService resourceService)
    : PageViewModel(logger)
{
    [ObservableProperty]
    private HierarchicalTreeDataGridSource<CelestialBody>? _starSystem;
    
    [ObservableProperty]
    private bool _isLoadingStarSystem;
    
    // Filter properties
    [ObservableProperty]
    private string _searchText = string.Empty;
    
    [ObservableProperty]
    private bool _showStars = true;
    
    [ObservableProperty]
    private bool _showPlanets = true;
    
    [ObservableProperty]
    private bool _showStations = true;
    
    [ObservableProperty]
    private bool _showSignals = true;
    
    public bool HasNoItems => !(StarSystem?.Items ?? []).Any();
    
    private static CelestialBody? FindBodyRecursive(CelestialBody? root, int bodyId)
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
    
    protected override async Task OnInitializeAsync()
    {
        logger.LogInformation(Localization.Instance["SystemPage.Initializing"]);
        IsLoadingStarSystem = true;
        try
        {
            await LoadAllScanDataAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, Localization.Instance["SystemPage.Exceptions.FailedToInitialize"]);
        }
        finally
        {
            IsLoadingStarSystem = false;
        }
    }
    
    partial void OnSearchTextChanged(string value) => ApplyFilters();
    partial void OnShowStarsChanged(bool value) => ApplyFilters();
    partial void OnShowPlanetsChanged(bool value) => ApplyFilters();
    partial void OnShowStationsChanged(bool value) => ApplyFilters();
    partial void OnShowSignalsChanged(bool value) => ApplyFilters();
    
    private CelestialBody? FindCelestialBodyByBodyId(int bodyId) =>
        FindBodyRecursive(celestialStructure.SystemRoot, bodyId);
    
    private async Task LoadAllScanDataAsync()
    {
        // Load all scan data types in a single journal pass
        var scanData = await journalService.GetLatestJournalEntriesBatchAsync(
            typeof(LocationEvent),
            typeof(FSSSignalDiscoveredEvent),
            typeof(ScanEvent),
            typeof(SAAScanCompleteEvent)
        );

        var locationScan = scanData[typeof(LocationEvent)].Cast<LocationEvent>().ToList().LastOrDefault();
        if (locationScan is null)
            return;
        
        celestialStructure.AddLocationScan(locationScan);
        
        var fssScans = scanData[typeof(FSSSignalDiscoveredEvent)].Cast<FSSSignalDiscoveredEvent>().ToList();
        foreach (var scan in fssScans)
        {
            celestialStructure.AddFSSSignalDiscoveredEvent(scan);
        }
        
        var systemScans = scanData[typeof(ScanEvent)].Cast<ScanEvent>().ToList();
        foreach (var scan in systemScans)
        {
            celestialStructure.AddScanEvent(scan);
        }

        // Process SAA scans
        var saaScans = scanData[typeof(SAAScanCompleteEvent)].Cast<SAAScanCompleteEvent>().ToList();
        if (saaScans.Any())
        {
            ProcessCompletedScans(saaScans);
        }

        celestialStructure.BuildHierarchy();
        RefreshSystemDisplay();
    }
    
    private void ApplyFilters()
    {
        RefreshSystemDisplay();
    }
    
    private bool ShouldIncludeBody(CelestialBody body)
    {
        // Search text filter
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var matchesSearch = (body.BodyName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (body.DisplayName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (body.TypeInfo?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false);
            
            if (!matchesSearch)
                return false;
        }
        
        // Type filters - check concrete type
        var typeMatch = body switch
        {
            Star => ShowStars,
            Planet => ShowPlanets,
            Station => ShowStations,
            Signal => ShowSignals,
            SystemNode => true,
            BeltCluster => ShowPlanets,
            Ring => ShowPlanets,
            _ => true 
        };
        
        return typeMatch;
    }
    
    private IEnumerable<CelestialBody> GetFilteredChildren(CelestialBody body)
    {
        foreach (var child in body.Children)
        {
            // Check if child matches filters
            if (ShouldIncludeBody(child))
            {
                yield return child;
                continue;
            }
            
            // If child doesn't match, check if any of its descendants match
            var filteredGrandchildren = GetFilteredChildren(child).ToList();
            if (filteredGrandchildren.Any())
            {
                // Child doesn't match but has matching descendants, so include it
                yield return child;
            }
        }
    }

    private void SetDefaultColorRecursive(CelestialBody body)
    {
        body.ForegroundBrush ??= resourceService.GetBrush("Text.Primary");
        foreach (var child in body.SubItems)
        {
            SetDefaultColorRecursive(child);
        }
    }

    private void RefreshSystemDisplay()
    {
        if (celestialStructure.SystemRoot is null)
            return;
        
        var systemRootCollection = new List<CelestialBody> { celestialStructure.SystemRoot };
        StarSystem = new HierarchicalTreeDataGridSource<CelestialBody>(systemRootCollection)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<CelestialBody>(
                    new TemplateColumn<CelestialBody>("Name", 
                        GetNameColumnTemplate()),
                    GetFilteredChildren),
                new TemplateColumn<CelestialBody>("Type", GetTextColumnTemplate(nameof(CelestialBody.TypeInfo))),
                new TemplateColumn<CelestialBody>("Distance", GetTextColumnTemplate(nameof(CelestialBody.DistanceInfo))),
                new TemplateColumn<CelestialBody>("Status", GetTextColumnTemplate(nameof(CelestialBody.StatusInfo))),
                new TemplateColumn<CelestialBody>("Landable", GetTextColumnTemplate(nameof(CelestialBody.LandableInfo))),
                new TemplateColumn<CelestialBody>("Mass", GetTextColumnTemplate(nameof(CelestialBody.MassInfo)))
            }
        };
        
        SetDefaultColorRecursive(celestialStructure.SystemRoot);
        OnPropertyChanged(nameof(HasNoItems));
    }
    
    private void ProcessCompletedScans(IList<SAAScanCompleteEvent> scans)
    {
        if (celestialStructure.SystemRoot is null)
            return;
        
        SetDefaultColorRecursive(celestialStructure.SystemRoot);
        
        foreach (var scan in scans.Where(x => celestialStructure.SystemRoot.SystemAddress == x.SystemAddress))
        {
            var body = FindCelestialBodyByBodyId(scan.BodyId);
            if (body is not null)
            {
                if (scan.ProbesUsed < scan.EfficiencyTarget)
                {
                    body.ForegroundBrush = resourceService.GetBrush("Success.Brush");
                } 
                else if (scan.ProbesUsed == scan.EfficiencyTarget)
                {
                    body.ForegroundBrush = resourceService.GetBrush("Warning.Brush");
                }
                else
                {
                    body.ForegroundBrush = resourceService.GetBrush("Danger.Brush");
                }    
            }
        }
    }
    
    private IDataTemplate GetNameColumnTemplate() =>
        templateCacheManager.GetOrCreateTemplate("CelestialBodyNameTemplate", () =>
            new FuncDataTemplate<CelestialBody?>((value, _) =>
            {
                var stackPanel = new StackPanel();
                if (value is null)
                {
                    return stackPanel;
                }

                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Spacing = 4;

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
                    [!TextBlock.TextProperty] = new Binding(nameof(CelestialBody.DisplayName)),
                    [!TextBlock.ForegroundProperty] = new Binding(nameof(CelestialBody.ForegroundBrush))
                };

                stackPanel.Children.Add(icon);
                stackPanel.Children.Add(nameText);

                return stackPanel;
            }));
    
    private IDataTemplate GetTextColumnTemplate(string propertyName) =>
        templateCacheManager.GetOrCreateTemplate($"TextColumn_{propertyName}", () =>
            new FuncDataTemplate<CelestialBody?>((value, _) =>
            {
                var textBlock = new TextBlock();
                if (value is null)
                {
                    return textBlock;
                }

                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.UseLayoutRounding = true;
                textBlock.TextWrapping = TextWrapping.NoWrap;
                textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                textBlock[!TextBlock.TextProperty] = new Binding(propertyName);
                textBlock[!TextBlock.ForegroundProperty] = new Binding(nameof(CelestialBody.ForegroundBrush))
                {
                    FallbackValue = resourceService.GetBrush("Text.Primary")
                };

                return textBlock;
            }));

    private IconData GetCachedIconForCelestialBody(CelestialBody? body)
    {
        if (body?.TypeInfo is null)
            return new IconData("avares://EdAssistant/Assets/Icons/Default/Unknown.png");

        var cacheKey = $"icon_{body.TypeInfo.ToLowerInvariant()}";
        return templateCacheManager.GetOrCreateIcon(cacheKey, () => 
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
            
            case NavBeacon:
                return new IconData("avares://EdAssistant/Assets/Icons/Station/NavBeacon.png");
            
            case FleetCarrier:
            case SquadronCarrier:
                return new IconData("avares://EdAssistant/Assets/Icons/Station/FleetCarrier.png");
            
            case AsteroidBase:
                return new IconData("avares://EdAssistant/Assets/Icons/Station/AsteroidBase.png");
            
            case Coriolis:
                return new IconData("avares://EdAssistant/Assets/Icons/Station/Coriolis.png");
            
            case Orbis:
                return new IconData("avares://EdAssistant/Assets/Icons/Station/Orbis.png");
            
            case Ocellus:
                return new IconData("avares://EdAssistant/Assets/Icons/Station/Ocellus.png");
            
            case ConflictZone:
                return new IconData("avares://EdAssistant/Assets/Icons/Signals/Conflict Zone.png");
            
            case ResourceExtraction:
                return new IconData("avares://EdAssistant/Assets/Icons/Signals/Resources.png");
            
            case Megaship:
                return new IconData("avares://EdAssistant/Assets/Icons/Signals/Megaship.png");
            
            case StationMegaship:
                return new IconData("avares://EdAssistant/Assets/Icons/Signals/StationMegaship.png");
            
            case Installation:
                return new IconData("avares://EdAssistant/Assets/Icons/Signals/Installation.png");
            
            default:
                return new IconData(body is Signal 
                    ? "avares://EdAssistant/Assets/Icons/Signals/Signal.png"
                    : "avares://EdAssistant/Assets/Icons/Default/Unknown.png");
        }
    }
}