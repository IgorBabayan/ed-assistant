namespace EdAssistant.Services.SystemStructure;

class CelestialStructure(ILogger<CelestialStructure> logger) : ICelestialStructure
{
    private readonly Dictionary<int, CelestialBody> _bodyLookup = new();
    private readonly Dictionary<string, Station> _stationLookup = new();
    private readonly Dictionary<int, int> _directParentMap = new();
    private readonly HashSet<int> _barycenterIds = new();
    private readonly List<ScanEvent> _allScans = new();
    private SystemNode? _currentSystem;
    private string _currentSystemName = string.Empty;

    public IReadOnlyList<Star> Stars =>
        _currentSystem?.Children.OfType<Star>().ToList().AsReadOnly() ?? new List<Star>().AsReadOnly();

    public required long SystemAddress { get; set; }
    public required string SystemName { get; set; }
    public SystemNode SystemRoot => _currentSystem!;

    public void AddScanBaryCentreEvent(ScanBaryCentreEvent barycenterEvent)
    {
        _barycenterIds.Add(barycenterEvent.BodyId);
        logger.LogInformation(Localization.Instance["SystemPage.Information.NotedBarycenter"], 
            barycenterEvent.BodyId, barycenterEvent.StarSystem);
    }

    public void AddScanEvent(ScanEvent scanEvent)
    {
        if (!string.Equals(_currentSystemName, scanEvent.StarSystem, StringComparison.OrdinalIgnoreCase))
        {
            _currentSystemName = scanEvent.StarSystem;
            _bodyLookup.Clear();
            _allScans.Clear();
            _directParentMap.Clear();

            _currentSystem = new SystemNode(scanEvent.StarSystem, scanEvent.SystemAddress);
            SystemName = scanEvent.StarSystem;
            SystemAddress = scanEvent.SystemAddress;

            logger.LogInformation(Localization.Instance["SystemPage.Information.SwitchedToSystem"], scanEvent.StarSystem);
        }

        if (string.Equals(_currentSystemName, scanEvent.StarSystem, StringComparison.OrdinalIgnoreCase))
        {
            if (_allScans.Any(s => s.BodyId == scanEvent.BodyId))
            {
                logger.LogInformation(Localization.Instance["SystemPage.Information.DuplicateBodyDetected"], scanEvent.BodyName,
                    scanEvent.BodyId);
                return;
            }

            _allScans.Add(scanEvent);
            var body = CreateBodyFromScan(scanEvent);
            _bodyLookup[scanEvent.BodyId] = body;

            logger.LogInformation(Localization.Instance["SystemPage.Information.AddedBody"], body.BodyName, body.BodyId,
                scanEvent.StarSystem);
        }
    }

    public void AddFSSSignalDiscoveredEvent(FSSSignalDiscoveredEvent fssSignal)
    {
        if (SystemAddress != fssSignal.SystemAddress)
            return;

        var station = CreateStationFromScan(fssSignal);
        _stationLookup[fssSignal.SignalName] = station;

        logger.LogInformation(Localization.Instance["SystemPage.Information.AddedStation"],
            fssSignal.SignalName, fssSignal.SignalTypeEnum);
    }

    public void BuildHierarchy()
    {
        if (_currentSystem is null)
            return;

        logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.BuildingHierarchy"]);

        // Clear existing hierarchy
        _currentSystem.Children.Clear();
        foreach (var body in _bodyLookup.Values)
        {
            body.Children.Clear();
        }

        // Clear and rebuild parent-child maps
        _directParentMap.Clear();

        // Step 1: Build parent-child relationships from scan data
        BuildParentChildMaps();

        // Step 2: Add all stars to system root
        AddStarsToSystem();

        // Step 3: Build the complete hierarchy tree
        BuildHierarchyTree();

        // Step 4: Add stations to appropriate celestial bodies
        AddStationsToHierarchy();

        logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.HierarchyBuildingComplete"]);
    }

    private static Station CreateStationFromScan(FSSSignalDiscoveredEvent scanEvent) =>
        scanEvent.SignalTypeEnum switch
        {
            StationTypeEnum.Outpost => new Outpost { BodyName = scanEvent.SignalName },
            StationTypeEnum.AsteroidBase => new Asteroid { BodyName = scanEvent.SignalName },
            StationTypeEnum.Coriolis => new Coriolis { BodyName = scanEvent.SignalName },
            StationTypeEnum.Orbis => new Orbis { BodyName = scanEvent.SignalName },
            StationTypeEnum.Ocellus => new Ocellus { BodyName = scanEvent.SignalName },
            _ => new UnknownStation { BodyName = scanEvent.SignalName }
        };

    private static CelestialBody CreateBodyFromScan(ScanEvent scanEvent)
    {
        if (scanEvent.StarType is not null)
        {
            return new Star
            {
                BodyName = scanEvent.BodyName,
                BodyId = scanEvent.BodyId,
                BodyType = Localization.Instance["CelestialInfo.Star"],
                DistanceFromArrivalLS = scanEvent.DistanceFromArrivalLS,
                WasDiscovered = scanEvent.WasDiscovered,
                WasMapped = scanEvent.WasMapped,
                StarType = scanEvent.StarType,
                Subclass = scanEvent.Subclass,
                StellarMass = scanEvent.StellarMass,
                Radius = scanEvent.Radius,
                AbsoluteMagnitude = scanEvent.AbsoluteMagnitude,
                AgeMY = scanEvent.AgeMY,
                SurfaceTemperature = scanEvent.SurfaceTemperature,
                Luminosity = scanEvent.Luminosity,
                RotationPeriod = scanEvent.RotationPeriod,
                AxialTilt = scanEvent.AxialTilt,
                SemiMajorAxis = scanEvent.SemiMajorAxis,
                Eccentricity = scanEvent.Eccentricity,
                OrbitalInclination = scanEvent.OrbitalInclination,
                Periapsis = scanEvent.Periapsis,
                OrbitalPeriod = scanEvent.OrbitalPeriod,
                AscendingNode = scanEvent.AscendingNode,
                MeanAnomaly = scanEvent.MeanAnomaly,
                Rings = scanEvent.Rings?.Select(r => new Ring
                {
                    BodyName = r.Name,
                    RingClass = r.RingClass,
                    MassMT = r.MassMT,
                    InnerRad = r.InnerRad,
                    OuterRad = r.OuterRad
                }).ToList()
            };
        }

        if (scanEvent.BodyName.Contains(Localization.Instance["CelestialInfo.BeltCluster"]))
        {
            return new BeltCluster
            {
                BodyName = scanEvent.BodyName,
                BodyId = scanEvent.BodyId,
                BodyType = Localization.Instance["CelestialInfo.BeltCluster"],
                DistanceFromArrivalLS = scanEvent.DistanceFromArrivalLS,
                WasDiscovered = scanEvent.WasDiscovered,
                WasMapped = scanEvent.WasMapped,
                SemiMajorAxis = scanEvent.SemiMajorAxis,
                Eccentricity = scanEvent.Eccentricity,
                OrbitalInclination = scanEvent.OrbitalInclination,
                Periapsis = scanEvent.Periapsis,
                OrbitalPeriod = scanEvent.OrbitalPeriod,
                AscendingNode = scanEvent.AscendingNode,
                MeanAnomaly = scanEvent.MeanAnomaly
            };
        }

        if (scanEvent.BodyName.Contains(Localization.Instance["CelestialInfo.Ring"]))
        {
            return new Ring
            {
                BodyName = scanEvent.BodyName,
                BodyId = scanEvent.BodyId,
                BodyType = Localization.Instance["CelestialInfo.Ring"],
                DistanceFromArrivalLS = scanEvent.DistanceFromArrivalLS,
                WasDiscovered = scanEvent.WasDiscovered,
                WasMapped = scanEvent.WasMapped,
                SemiMajorAxis = scanEvent.SemiMajorAxis,
                Eccentricity = scanEvent.Eccentricity,
                OrbitalInclination = scanEvent.OrbitalInclination,
                Periapsis = scanEvent.Periapsis,
                OrbitalPeriod = scanEvent.OrbitalPeriod,
                AscendingNode = scanEvent.AscendingNode,
                MeanAnomaly = scanEvent.MeanAnomaly
            };
        }

        return new Planet
        {
            BodyName = scanEvent.BodyName,
            BodyId = scanEvent.BodyId,
            BodyType = Localization.Instance["CelestialInfo.Planet"],
            DistanceFromArrivalLS = scanEvent.DistanceFromArrivalLS,
            WasDiscovered = scanEvent.WasDiscovered,
            WasMapped = scanEvent.WasMapped,
            TidalLock = scanEvent.TidalLock,
            TerraformState = scanEvent.TerraformState,
            PlanetClass = scanEvent.PlanetClass,
            Atmosphere = scanEvent.Atmosphere,
            AtmosphereType = scanEvent.AtmosphereType,
            AtmosphereComposition = scanEvent.AtmosphereComposition,
            Volcanism = scanEvent.Volcanism,
            MassEM = scanEvent.MassEM,
            SurfaceGravity = scanEvent.SurfaceGravity,
            SurfaceTemperature = scanEvent.SurfaceTemperature,
            SurfacePressure = scanEvent.SurfacePressure,
            Landable = scanEvent.Landable,
            Materials = scanEvent.Materials,
            Composition = scanEvent.Composition,
            RotationPeriod = scanEvent.RotationPeriod,
            AxialTilt = scanEvent.AxialTilt,
            SemiMajorAxis = scanEvent.SemiMajorAxis,
            Eccentricity = scanEvent.Eccentricity,
            OrbitalInclination = scanEvent.OrbitalInclination,
            Periapsis = scanEvent.Periapsis,
            OrbitalPeriod = scanEvent.OrbitalPeriod,
            AscendingNode = scanEvent.AscendingNode,
            MeanAnomaly = scanEvent.MeanAnomaly,
            ReserveLevel = scanEvent.ReserveLevel,
            Rings = scanEvent.Rings?.Select(r => new Ring
            {
                BodyName = r.Name,
                RingClass = r.RingClass,
                MassMT = r.MassMT,
                InnerRad = r.InnerRad,
                OuterRad = r.OuterRad
            }).ToList()
        };
    }

    private void BuildParentChildMaps()
    {
        foreach (var scan in _allScans)
        {
            var bodyId = scan.BodyId;
            var directParentId = FindDirectParentId(scan.Parents);
            if (directParentId.HasValue)
            {
                _directParentMap[bodyId] = directParentId.Value;
            }
        }
    }

    private int? FindDirectParentId(List<ParentInfo>? parents)
    {
        if (parents == null || !parents.Any())
            return null;

        // Walk through parent chain to find the most direct resolvable parent
        foreach (var parent in parents)
        {
            // Direct star parent - this is what we want
            if (parent.Star.HasValue && _bodyLookup.ContainsKey(parent.Star.Value))
            {
                return parent.Star.Value;
            }

            // Direct planet parent - this is what we want for moons
            if (parent.Planet.HasValue && _bodyLookup.ContainsKey(parent.Planet.Value))
            {
                return parent.Planet.Value;
            }

            // Direct ring parent
            if (parent.Ring.HasValue && _bodyLookup.ContainsKey(parent.Ring.Value))
            {
                return parent.Ring.Value;
            }
        }

        // If we only have Null parents (barycenters), we need to find the best star
        // Since we can't place objects under barycenters, find the most appropriate star
        return FindBestStarForBarycenterChild(parents);
    }

    private int? FindBestStarForBarycenterChild(List<ParentInfo> parents)
    {
        // Get all available stars
        var stars = _bodyLookup.Values.OfType<Star>().ToList();
        if (!stars.Any())
            return null;

        // Single star system - assign to that star
        if (stars.Count == 1)
            return stars.First().BodyId;

        // For binary/multiple star systems, analyze barycenter relationships
        // Find which stars are part of the same barycenter as the child
        var barycenterIds = parents.Where(p => p.Null.HasValue).Select(p => p.Null!.Value).ToList();
        
        if (barycenterIds.Any())
        {
            // Find stars that share the same immediate barycenter as this child
            var candidateStars = FindStarsInSameBarycenter(barycenterIds, stars);
            if (candidateStars.Any())
            {
                // Return the primary star (lowest BodyId) from the barycenter group
                return candidateStars.OrderBy(s => s.BodyId).First().BodyId;
            }
        }

        // Fallback: assign to the primary star (lowest BodyId)
        return stars.OrderBy(s => s.BodyId).First().BodyId;
    }

    private List<Star> FindStarsInSameBarycenter(List<int> childBarycenterIds, List<Star> allStars)
    {
        var candidateStars = new List<Star>();

        // Find stars that belong to the same barycenter as the child
        foreach (var star in allStars)
        {
            var starScan = _allScans.FirstOrDefault(s => s.BodyId == star.BodyId);
            if (starScan?.Parents != null)
            {
                var starBarycenterIds = starScan.Parents
                    .Where(p => p.Null.HasValue)
                    .Select(p => p.Null!.Value)
                    .ToList();

                // Check if this star shares any barycenter with the child
                if (starBarycenterIds.Intersect(childBarycenterIds).Any())
                {
                    candidateStars.Add(star);
                }
            }
            else
            {
                // Stars without parents are primary stars - include them if no specific barycenter match
                if (!candidateStars.Any())
                {
                    candidateStars.Add(star);
                }
            }
        }

        return candidateStars;
    }

    private void AddStarsToSystem()
    {
        if (_currentSystem is null)
            return;
        
        var stars = _bodyLookup.Values.OfType<Star>().OrderBy(s => s.BodyId).ToList();
        foreach (var star in stars)
        {
            _currentSystem.Children.Add(star);
            logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.AddedStar"], star.BodyName);
        }
    }

    private void BuildHierarchyTree()
    {
        // Process non-star bodies and place them under their direct parents
        var nonStarBodies = _bodyLookup.Values.Where(body => !(body is Star)).ToList();
        foreach (var body in nonStarBodies)
        {
            if (_directParentMap.TryGetValue(body.BodyId, out var parentId) &&
                _bodyLookup.TryGetValue(parentId, out var parentBody))
            {
                // Add to correct parent
                if (body is BeltCluster && parentBody is Star)
                {
                    // Insert belt clusters before other children
                    var insertIndex = parentBody.Children.TakeWhile(c => c is BeltCluster).Count();
                    parentBody.Children.Insert(insertIndex, body);
                    logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.AddedBeltCluster"], body.BodyName, parentBody.BodyName);
                }
                else
                {
                    parentBody.Children.Add(body);

                    switch (body)
                    {
                        case Planet:
                            logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.AddedPlanet"], body.BodyName, parentBody.BodyName);
                            break;
                        case BeltCluster:
                            logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.AddedBeltCluster"], body.BodyName, parentBody.BodyName);
                            break;
                        default:
                            logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.AddedMoon"], body.BodyName, parentBody.BodyName);
                            break;
                    }
                }
            }
            else
            {
                // Fallback: add to first star
                var fallbackStar = _bodyLookup.Values.OfType<Star>().OrderBy(s => s.BodyId).FirstOrDefault();
                if (fallbackStar != null)
                {
                    fallbackStar.Children.Add(body);
                    logger.LogInformation(Localization.Instance["SystemPage.ScanProcess.AddedOrphanedBody"], body.BodyName);
                }
            }
        }
    }

    private void AddStationsToHierarchy()
    {
        foreach (var station in _stationLookup.Values)
        {
            // Find the best celestial body to attach this station to
            var targetBody = FindBestBodyForStation(station);
            if (targetBody is not null)
            {
                targetBody.Children.Add(station);
                logger.LogInformation(Localization.Instance["SystemPage.Information.AddedStation"],
                    station.BodyName, targetBody.BodyName);
            }
            else
            {
                // Fallback: add to the main star or system root
                var mainStar = _bodyLookup.Values.OfType<Star>().FirstOrDefault();
                if (mainStar is not null)
                {
                    mainStar.Children.Add(station);
                    logger.LogInformation(Localization.Instance["SystemPage.Information.AddedOrphanedStation"], station.BodyName);
                }
                else
                {
                    _currentSystem?.Children.Add(station);
                    logger.LogInformation(Localization.Instance["SystemPage.Information.AddedOrphanedStation"], station.BodyName);
                }
            }
        }
    }

    private CelestialBody? FindBestBodyForStation(Station station)
    {
        // Strategy 1: Try to match by name similarity
        // For example, if station is "Babbage Base" and there's a body "Babbage", match them
        var stationNameParts = station.BodyName.ToLowerInvariant()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (var body in _bodyLookup.Values)
        {
            var bodyNameParts = body.BodyName.ToLowerInvariant()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Check if any significant part of the names match
            if (stationNameParts.Intersect(bodyNameParts).Any())
            {
                return body;
            }
        }

        // Strategy 2: Default placement near star (don't know yet how to place it correctly)
        return _bodyLookup.Values.OfType<Star>()
            .OrderBy(x => x.BodyId)
            .FirstOrDefault();
    }
}