namespace EdAssistant.Services.SystemStructure;

class CelestialStructure(ILogger<CelestialStructure> logger, ICelestialBodyFactory celestialBodyFactory) : ICelestialStructure
{
    private readonly List<ScanEvent> _allScans = new();
    private readonly Dictionary<int, CelestialBody> _bodies = new();
    private readonly Dictionary<string, CelestialBody> _stations = new();
    private readonly Dictionary<string, CelestialBody> _signals = new();
    private readonly Dictionary<int, int> _map = new();

    public SystemNode? SystemRoot { get; private set; }
    
    public void AddLocationScan(LocationEvent locationEvent)
    {
        _bodies.Clear();
        _stations.Clear();
        _signals.Clear();
        _map.Clear();
        
        SystemRoot = new SystemNode(locationEvent.StarSystem, locationEvent.SystemAddress);
        logger.LogInformation(Localization.Instance["SystemPage.Information.SwitchedToSystem"], locationEvent.StarSystem);
    }

    public void AddScanEvent(ScanEvent scanEvent)
    {
        if (scanEvent.SystemAddress != SystemRoot!.SystemAddress &&
            !string.Equals(scanEvent.StarSystem, SystemRoot!.BodyName, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        
        if (_allScans.Any(x => x.BodyId == scanEvent.BodyId))
        {
            logger.LogInformation(Localization.Instance["SystemPage.Information.DuplicateBodyDetected"], 
                scanEvent.BodyName, scanEvent.BodyId);
            return;
        }
            
        _allScans.Add(scanEvent);

        var body = celestialBodyFactory.Create(scanEvent);
        if (body is not null)
        {
            _bodies[scanEvent.BodyId] = body;
            
            logger.LogInformation(Localization.Instance["SystemPage.Information.AddedBody"], body.BodyName, body.BodyId,
                scanEvent.StarSystem);   
        }
    }

    public void AddFSSSignalDiscoveredEvent(FSSSignalDiscoveredEvent fssSignal)
    {
        if (fssSignal.SystemAddress != SystemRoot!.SystemAddress)
            return;
        
        var scanEvent = celestialBodyFactory.Create(fssSignal);
        switch (scanEvent)
        {
            case Station:
                _stations[fssSignal.SignalName] = scanEvent;
                break;
            
            case Signal:
                _signals[fssSignal.SignalName] = scanEvent;
                break;
        }

        logger.LogInformation(Localization.Instance["SystemPage.Information.AddedStation"],
            fssSignal.SignalName, fssSignal.SignalType);
    }

    public void BuildHierarchy()
    {
        if (SystemRoot is null)
            return;
        
        SystemRoot.Children.Clear();
        
        // Step 0: Build parent-child relationships from scan data
        BuildParentChildMaps();
        
        // Step 1: Add all stars to system root
        AddStars();
        
        // Step 2: Add all planets to corresponding star
        AddPlanets();
        
        // Step 3: Add belt clusters to corresponding star
        AddBeltClusters();
        
        // Step 4: Add stations to appropriate celestial bodies
        //AddStations();
        
        // Step 5: Add signals to system root
        //AddSignals();

        // Step 6: Build the complete hierarchy tree
        //BuildHierarchyTree();

        logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.HierarchyBuildingComplete"]);
    }
    
    private IReadOnlyList<T> GetCelestialBody<T>() where T : CelestialBody => 
        _bodies.Values.OfType<T>().OrderBy(x => x.BodyId).ToList();

    private int? FindDirectParentId(IReadOnlyList<ParentInfo>? parents)
    {
        if (parents == null || !parents.Any())
            return null;

        foreach (var parent in parents)
        {
            // Direct star parent - this is what we want
            if (parent.Star.HasValue && _bodies.ContainsKey(parent.Star.Value))
            {
                return parent.Star.Value;
            }

            // Direct planet parent - this is what we want for moons
            if (parent.Planet.HasValue && _bodies.ContainsKey(parent.Planet.Value))
            {
                return parent.Planet.Value;
            }

            // Direct ring parent
            if (parent.Ring.HasValue && _bodies.ContainsKey(parent.Ring.Value))
            {
                return parent.Ring.Value;
            }
        }

        return FindStarInBarycenter(parents);
    }

    private int? FindStarInBarycenter(IReadOnlyList<ParentInfo> parents)
    {
        var stars = _bodies.Values.OfType<Star>().ToList();
        if (!stars.Any())
            return null;
        
        if (stars.Count == 1)
            return stars.First().BodyId;
        
        var barycenterIds = parents.Where(p => p.Null.HasValue).Select(p => p.Null!.Value).ToList();
        if (barycenterIds.Any())
        {
            var candidateStars = FindStarsInBarycenter(barycenterIds, stars);
            if (candidateStars.Any())
            {
                return candidateStars.OrderBy(s => s.BodyId).First().BodyId;
            }
        }
        
        return stars.OrderBy(s => s.BodyId).First().BodyId;
    }
    
    private List<Star> FindStarsInBarycenter(List<int> childBarycenterIds, List<Star> allStars)
    {
        var candidateStars = new List<Star>();
        foreach (var star in allStars)
        {
            var starScan = _allScans.FirstOrDefault(s => s.BodyId == star.BodyId);
            if (starScan?.Parents != null)
            {
                var starBarycenterIds = starScan.Parents
                    .Where(p => p.Null.HasValue)
                    .Select(p => p.Null!.Value)
                    .ToList();

                if (starBarycenterIds.Intersect(childBarycenterIds).Any())
                {
                    candidateStars.Add(star);
                }
            }
            else
            {
                if (!candidateStars.Any())
                {
                    candidateStars.Add(star);
                }
            }
        }

        return candidateStars;
    }

    private void BuildParentChildMaps()
    {
        foreach (var scan in _allScans)
        {
            var bodyId = scan.BodyId;
            var directParentId = FindDirectParentId(scan.Parents);
            if (directParentId.HasValue)
            {
                _map[bodyId] = directParentId.Value;
            }
        }
    }
    
    private void AddStars()
    {
        var stars = GetCelestialBody<Star>();
        foreach (var star in stars)
        {
            SystemRoot!.Children.Add(star);
            logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.AddedStar"], star.BodyName);
        }
    }

    private void AddPlanets()
    {
        var planets = GetCelestialBody<Planet>();
        foreach (var planet in planets)
        {
            if (_map.TryGetValue(planet.BodyId, out var parentId)
                && _bodies.TryGetValue(parentId, out var parentBody))
            {
                if (parentBody is Star)
                {
                    var insertIndex = parentBody.Children.TakeWhile(c => c is Planet).Count();
                    parentBody.Children.Insert(insertIndex, planet);
                    logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.AddedBeltCluster"],
                        planet.BodyName, parentBody.BodyName);
                }
                else
                {
                    parentBody.Children.Add(planet);
                    logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.AddedPlanet"],
                        planet.BodyName, parentBody.BodyName);
                }
            }
            else
            {
                var star = GetCelestialBody<Star>().FirstOrDefault();
                if (star is not null)
                {
                    star.Children.Add(planet);
                    logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.AddedOrphanedBody"],
                        planet.BodyName);
                }
            } 
        }
    }

    private void AddBeltClusters()
    {
        var beltClusters = GetCelestialBody<BeltCluster>();
        foreach (var beltCluster in beltClusters)
        {
            var parentId = FindDirectParentId(beltCluster.Parents);
            if (parentId is null)
                continue;
            
            var parent = SystemRoot!.Children.FirstOrDefault(p => p.BodyId == parentId);
            //if (parent is null)
        }
    }
}
