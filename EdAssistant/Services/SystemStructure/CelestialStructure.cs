namespace EdAssistant.Services.SystemStructure;

class CelestialStructure(ILogger<CelestialStructure> logger) : ICelestialStructure
{
    private readonly Dictionary<int, CelestialBody> _bodyLookup = new();
    private readonly List<ScanEvent> _allScans = new();
    private SystemNode? _currentSystem;
    private string _currentSystemName = string.Empty;

    public IReadOnlyList<Star> Stars =>
        _currentSystem?.Children.OfType<Star>().ToList().AsReadOnly() ?? new List<Star>().AsReadOnly();
    public required long SystemAddress { get; set; }
    public required string SystemName { get; set; }

    public SystemNode SystemRoot => _currentSystem!;

    public void AddScanEvent(ScanEvent scanEvent)
    {
        // Check if this scan event is for the current system we're displaying
        if (!string.Equals(_currentSystemName, scanEvent.StarSystem, StringComparison.OrdinalIgnoreCase))
        {
            // Switch to new system - clear previous data
            _currentSystemName = scanEvent.StarSystem;
            _bodyLookup.Clear();
            _allScans.Clear();

            // Create system node
            _currentSystem = new SystemNode(scanEvent.StarSystem, scanEvent.SystemAddress);
            SystemName = scanEvent.StarSystem;
            SystemAddress = scanEvent.SystemAddress;

            logger.LogInformation(Localization.Instance["ScanProcess.SwitchedToSystem"], scanEvent.StarSystem);
        }

        // Only process if this scan event belongs to the current system
        if (string.Equals(_currentSystemName, scanEvent.StarSystem, StringComparison.OrdinalIgnoreCase))
        {
            // Check for duplicates
            if (_allScans.Any(s => s.BodyId == scanEvent.BodyId))
            {
                logger.LogInformation(Localization.Instance["ScanProcess.DuplicateBodyDetected"], scanEvent.BodyName, scanEvent.BodyId);
                return;
            }

            _allScans.Add(scanEvent);
            var body = CreateBodyFromScan(scanEvent);
            _bodyLookup[scanEvent.BodyId] = body;

            logger.LogInformation(Localization.Instance["ScanProcess.AddedBody"], body.BodyName, body.BodyId, scanEvent.StarSystem);
        }
    }

    // Simple name-based hierarchy building
    public void BuildHierarchy()
    {
        if (_currentSystem is null)
            return;

        logger.LogInformation(Localization.Instance["ScanProcess.BuildingHierarchy"]);
        _currentSystem.Children.Clear();

        // Clear all existing children
        foreach (var body in _bodyLookup.Values)
        {
            body.Children.Clear();
        }

        // Step 1: Add stars to system
        var stars = _bodyLookup.Values.OfType<Star>().OrderBy(s => s.BodyId).ToList();
        foreach (var star in stars)
        {
            _currentSystem.Children.Add(star);
            logger.LogInformation(Localization.Instance["ScanProcess.AddedStar"], star.BodyName);
        }

        // Step 2: Add planets to stars (bodies without letter suffixes like 'a', 'b', etc.)
        var planets = _bodyLookup.Values.Where(b => b is not Star && !IsMoonOrBeltCluster(b.BodyName))
            .OrderBy(b => b.BodyId).ToList();

        foreach (var planet in planets)
        {
            var parentStar = FindStarForBody(planet.BodyName);
            if (parentStar is not null)
            {
                parentStar.Children.Add(planet);
                logger.LogInformation(Localization.Instance["ScanProcess.AddedPlanet"], planet.BodyName, parentStar.BodyName);
            }
        }

        // Step 3: Add moons and belt clusters to planets
        var moonsAndClusters = _bodyLookup.Values.Where(b => b is not Star && IsMoonOrBeltCluster(b.BodyName))
            .OrderBy(b => b.BodyId).ToList();

        foreach (var moon in moonsAndClusters)
        {
            var parentPlanet = FindPlanetForMoon(moon.BodyName);
            if (parentPlanet is not null)
            {
                // Insert belt clusters first
                if (moon is BeltCluster)
                {
                    var insertIndex = parentPlanet.Children.TakeWhile(c => c is BeltCluster).Count();
                    parentPlanet.Children.Insert(insertIndex, moon);
                    logger.LogInformation(Localization.Instance["ScanProcess.AddedBeltCluster"], moon.BodyName, parentPlanet.BodyName);
                }
                else
                {
                    parentPlanet.Children.Add(moon);
                    logger.LogInformation(Localization.Instance["ScanProcess.AddedMoon"], moon.BodyName, parentPlanet.BodyName);
                }
            }
            else
            {
                // Fallback: add to main star
                var mainStar = stars.FirstOrDefault();
                if (mainStar is not null)
                {
                    mainStar.Children.Add(moon);
                    logger.LogInformation(Localization.Instance["ScanProcess.AddedOrphanedBody"], moon.BodyName);
                }
            }
        }
        logger.LogInformation(Localization.Instance["ScanProcess.HierarchyBuildingComplete"]);
    }

    private bool IsMoonOrBeltCluster(string bodyName)
    {
        // Check if this is a moon (has letter suffix) or belt cluster
        if (bodyName.Contains(Localization.Instance["CelestialInfo.BeltCluster"]))
            return true;

        // Check for moon pattern: ends with " a", " b", " c", etc.
        var parts = bodyName.Split(' ');
        if (parts.Length > 0)
        {
            var lastPart = parts[^1];
            return lastPart.Length == 1 && char.IsLetter(lastPart[0]);
        }

        return false;
    }

    private Star? FindStarForBody(string bodyName)
    {
        // For bodies in this system, they belong to the main star
        var parts = bodyName.Split(' ');
        if (parts.Length < 2)
            return _bodyLookup.Values.OfType<Star>().FirstOrDefault();

        // Get the star designation (A, B, C, etc.)
        var starDesignation = parts[^2]; // Second to last part should be the star designation

        // Find the star that matches this designation
        var targetStar = _bodyLookup.Values.OfType<Star>()
            .FirstOrDefault(s => s.BodyName.EndsWith(" " + starDesignation));

        // Fallback to main star if not found
        return targetStar ?? _bodyLookup.Values.OfType<Star>().FirstOrDefault();
    }

    private Planet? FindPlanetForMoon(string moonName)
    {
        if (moonName.Contains(Localization.Instance["CelestialInfo.BeltCluster"]))
        {
            // Belt cluster names like "NGC 6124 Sector QT-R c4-7 A Belt Cluster 1"
            // Find the planet by removing "Belt Cluster X" part
            var baseName = moonName;
            var beltIndex = baseName.IndexOf($" {Localization.Instance["CelestialInfo.BeltCluster"]}", StringComparison.Ordinal);
            if (beltIndex > 0)
            {
                baseName = baseName.Substring(0, beltIndex);
            }

            return _bodyLookup.Values.OfType<Planet>().FirstOrDefault(p => p.BodyName == baseName);
        }

        // Moon names like "NGC 6124 Sector QT-R c4-7 1 a"
        // Remove the last part (" a", " b", etc.) to get planet name
        var parts = moonName.Split(' ');
        if (parts.Length > 0)
        {
            var planetName = string.Join(" ", parts.Take(parts.Length - 1));
            return _bodyLookup.Values.OfType<Planet>().FirstOrDefault(p => p.BodyName == planetName);
        }

        return null;
    }

    private CelestialBody CreateBodyFromScan(ScanEvent scanEvent)
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
}