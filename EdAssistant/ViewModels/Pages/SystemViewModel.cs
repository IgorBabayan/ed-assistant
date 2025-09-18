namespace EdAssistant.ViewModels.Pages;

public sealed partial class SystemViewModel(IJournalService journalService, ILogger<SystemViewModel> logger,
    ICelestialStructure celestialStructure, ITemplateCacheManager templateCacheManager, IResourceService resourceService)
    : PageViewModel(logger)
{
    private string _currentSystemName = string.Empty;
    
    [ObservableProperty]
    private HierarchicalTreeDataGridSource<CelestialBody>? _starSystem;
    
    [ObservableProperty]
    private bool _isLoadingStarSystem;
    
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
            await LoadBarycenterDataAsync();
            var fssScans = await LoadFSSScansDataAsync();
            await LoadSystemDataAsync(fssScans);
            await LoadSAAScanDataAsync();
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

    private async Task LoadBarycenterDataAsync()
    {
        var barycenterScans = (await journalService.GetLatestJournalEntriesAsync<ScanBaryCentreEvent>()).ToList();
        foreach (var scan in barycenterScans)
        {
            celestialStructure.AddScanBaryCentreEvent(scan);
        }
    }

    private async Task<IList<FSSSignalDiscoveredEvent>> LoadFSSScansDataAsync()
    {
        var fssScans = (await journalService.GetLatestJournalEntriesAsync<FSSSignalDiscoveredEvent>()).ToList();
        if (!fssScans.Any())
            return [];

        return fssScans;
    }

    private async Task LoadSystemDataAsync(IList<FSSSignalDiscoveredEvent> fssScans)
    {
        var systemScans = (await journalService.GetLatestJournalEntriesAsync<ScanEvent>()).ToList();
        if (!systemScans.Any())
            return;
        
        ProcessScans(systemScans, fssScans);
    }

    private async Task LoadSAAScanDataAsync()
    {
        var saaScans = (await journalService.GetLatestJournalEntriesAsync<SAAScanCompleteEvent>()).ToList();
        if (!saaScans.Any())
            return;
        ProcessCompletedScans(saaScans);
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
    
    private void ProcessScans(IList<ScanEvent> scans, IList<FSSSignalDiscoveredEvent> fssScans)
    {
        // Get the most recent system from the scans
        var latestScan = scans.OrderByDescending(s => s.Timestamp).FirstOrDefault();
        if (latestScan is not null)
        {
            _currentSystemName = latestScan.StarSystem;
            logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.ProcessingScans"], _currentSystemName);
        }

        // Filter scans to only include those from the current system
        var systemScans = scans.Where(s => string.Equals(s.StarSystem, _currentSystemName, 
            StringComparison.OrdinalIgnoreCase)).ToList();

        // Remove duplicates based on BodyId
        var uniqueScans = systemScans
            .GroupBy(s => s.BodyId)
            .Select(g => g.First()) // Take first occurrence of each BodyId
            .OrderBy(s => s.BodyId)
            .ToList();

        logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.ProcessingUniqueScan"], uniqueScans.Count, 
            _currentSystemName, systemScans.Count - uniqueScans.Count);

        // Add all scans first
        foreach (var scan in uniqueScans)
        {
            celestialStructure.AddScanEvent(scan);
        }
        
        if (fssScans.Any())
        {
            var systemFSSScans = fssScans
                .Where(f => f.SystemAddress == latestScan?.SystemAddress)
                .ToList();
            
            foreach (var fssScan in systemFSSScans)
            {
                celestialStructure.AddFSSSignalDiscoveredEvent(fssScan);
            }
        }

        // Then build hierarchy using name-based logic
        celestialStructure.BuildHierarchy();
        RefreshSystemDisplay();
        
        SetDefaultColorRecursive(celestialStructure.SystemRoot);
    }
    
    private CelestialBody? FindCelestialBodyByBodyId(int bodyId) =>
        FindBodyRecursive(celestialStructure.SystemRoot, bodyId);

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
        var systemRootCollection = new List<CelestialBody> { celestialStructure.SystemRoot };
        StarSystem = new HierarchicalTreeDataGridSource<CelestialBody>(systemRootCollection)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<CelestialBody>(
                    new TemplateColumn<CelestialBody>("Name", 
                        GetNameColumnTemplate()),
                    x => x.SubItems),
                new TemplateColumn<CelestialBody>("Type", GetTextColumnTemplate(nameof(CelestialBody.TypeInfo))),
                new TemplateColumn<CelestialBody>("Distance", GetTextColumnTemplate(nameof(CelestialBody.DistanceInfo))),
                new TemplateColumn<CelestialBody>("Status", GetTextColumnTemplate(nameof(CelestialBody.StatusInfo))),
                new TemplateColumn<CelestialBody>("Landable", GetTextColumnTemplate(nameof(CelestialBody.LandableInfo))),
                new TemplateColumn<CelestialBody>("Mass", GetTextColumnTemplate(nameof(CelestialBody.MassInfo)))
            }
        };
        
        SetDefaultColorRecursive(celestialStructure.SystemRoot);
    }
    
    private void ProcessCompletedScans(IList<SAAScanCompleteEvent> scans)
    {
        SetDefaultColorRecursive(celestialStructure.SystemRoot);
        
        foreach (var scan in scans.Where(x => celestialStructure.SystemAddress == x.SystemAddress))
        {
            var body = FindCelestialBodyByBodyId(scan.BodyId);
            if (body is not null)
            {
                if (scan.ProbesUsed < scan.EfficiencyTarget)
                {
                    body.ForegroundBrush = resourceService.GetBrush("Success.Brush");
                } else if (scan.ProbesUsed == scan.EfficiencyTarget)
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
            new FuncDataTemplate<CelestialBody>((value, _) =>
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
            new FuncDataTemplate<CelestialBody>((value, _) =>
            {
                var textBlock = new TextBlock();
                if (value == null)
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
}