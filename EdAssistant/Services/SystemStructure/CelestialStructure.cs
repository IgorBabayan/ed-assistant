namespace EdAssistant.Services.SystemStructure;

// System root node that contains the entire star system
public class SystemNode : CelestialBody
{
    public SystemNode(string systemName, long systemAddress)
    {
        BodyName = systemName;
        BodyType = "System";
        SystemAddress = systemAddress;
        DistanceFromArrivalLS = 0;
    }

    public long SystemAddress { get; set; }

    public override string DisplayName => BodyName;
    public override string TypeInfo => $"System ({GetStarCount()} stars)";
    public override string DistanceInfo => "";
    public override string StatusInfo => $"{GetTotalBodies()} bodies";

    private int GetStarCount()
    {
        return Children.Count(c => c is Star);
    }

    private int GetTotalBodies()
    {
        int count = 0;
        foreach (var child in Children)
        {
            count += 1 + CountChildren(child.Children);
        }
        return count;
    }

    private int CountChildren(IList<CelestialBody> children)
    {
        int count = children.Count;
        foreach (var child in children)
        {
            count += CountChildren(child.Children);
        }
        return count;
    }
}

class CelestialStructure : ICelestialStructure
{
    private readonly Dictionary<int, CelestialBody> _bodyLookup = new();
    private readonly List<ScanEvent> _allScans = new();
    private SystemNode? _currentSystem;
    private string _currentSystemName = string.Empty;

    public IReadOnlyList<Star> Stars => _currentSystem?.Children.OfType<Star>().ToList().AsReadOnly() ?? new List<Star>().AsReadOnly();
    public required long SystemAddress { get; set; }
    public required string SystemName { get; set; }

    // Get the system as root node for TreeDataGrid
    public SystemNode? SystemRoot => _currentSystem;

    public void AddScanEvent(ScanEvent scanEvent)
    {
        // Check if this scan event is for the current system we're displaying
        if (_currentSystemName != scanEvent.StarSystem)
        {
            // Switch to new system - clear previous data
            _currentSystemName = scanEvent.StarSystem;
            _bodyLookup.Clear();
            _allScans.Clear();

            // Create system node
            _currentSystem = new SystemNode(scanEvent.StarSystem, scanEvent.SystemAddress);
            SystemName = scanEvent.StarSystem;
            SystemAddress = scanEvent.SystemAddress;

            Console.WriteLine($"Switched to system: {scanEvent.StarSystem}");
        }

        // Only process if this scan event belongs to the current system
        if (scanEvent.StarSystem == _currentSystemName)
        {
            // Check for duplicates
            if (_allScans.Any(s => s.BodyId == scanEvent.BodyId))
            {
                Console.WriteLine($"Duplicate body detected: {scanEvent.BodyName} (ID: {scanEvent.BodyId}) - skipping");
                return;
            }

            _allScans.Add(scanEvent);
            var body = CreateBodyFromScan(scanEvent);
            _bodyLookup[scanEvent.BodyId] = body;

            Console.WriteLine($"Added body: {body.BodyName} (ID: {body.BodyId}) to system: {scanEvent.StarSystem}");
        }
    }

    // Simple name-based hierarchy building
    public void BuildHierarchy()
    {
        if (_currentSystem == null) return;

        Console.WriteLine("Building hierarchy using name-based approach...");
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
            Console.WriteLine($"Added star: {star.BodyName}");
        }

        // Step 2: Add planets to stars (bodies without letter suffixes like 'a', 'b', etc.)
        var planets = _bodyLookup.Values.Where(b => b is not Star && !IsMoonOrBeltCluster(b.BodyName))
            .OrderBy(b => b.BodyId).ToList();

        foreach (var planet in planets)
        {
            var parentStar = FindStarForBody(planet.BodyName);
            if (parentStar != null)
            {
                parentStar.Children.Add(planet);
                Console.WriteLine($"Added planet: {planet.BodyName} to star: {parentStar.BodyName}");
            }
        }

        // Step 3: Add moons and belt clusters to planets
        var moonsAndClusters = _bodyLookup.Values.Where(b => b is not Star && IsMoonOrBeltCluster(b.BodyName))
            .OrderBy(b => b.BodyId).ToList();

        foreach (var moon in moonsAndClusters)
        {
            var parentPlanet = FindPlanetForMoon(moon.BodyName);
            if (parentPlanet != null)
            {
                // Insert belt clusters first
                if (moon is BeltCluster)
                {
                    var insertIndex = parentPlanet.Children.TakeWhile(c => c is BeltCluster).Count();
                    parentPlanet.Children.Insert(insertIndex, moon);
                    Console.WriteLine($"Added belt cluster: {moon.BodyName} to planet: {parentPlanet.BodyName}");
                }
                else
                {
                    parentPlanet.Children.Add(moon);
                    Console.WriteLine($"Added moon: {moon.BodyName} to planet: {parentPlanet.BodyName}");
                }
            }
            else
            {
                // Fallback: add to main star
                var mainStar = stars.FirstOrDefault();
                if (mainStar != null)
                {
                    mainStar.Children.Add(moon);
                    Console.WriteLine($"Added orphaned body: {moon.BodyName} to main star");
                }
            }
        }

        Console.WriteLine("Hierarchy building complete!");
        DebugHierarchy();
    }

    private bool IsMoonOrBeltCluster(string bodyName)
    {
        // Check if this is a moon (has letter suffix) or belt cluster
        if (bodyName.Contains("Belt Cluster")) return true;

        // Check for moon pattern: ends with " a", " b", " c", etc.
        var parts = bodyName.Split(' ');
        if (parts.Length > 0)
        {
            var lastPart = parts[parts.Length - 1];
            return lastPart.Length == 1 && char.IsLetter(lastPart[0]);
        }

        return false;
    }

    private Star? FindStarForBody(string bodyName)
    {
        // For bodies in this system, they belong to the main star
        return _bodyLookup.Values.OfType<Star>().FirstOrDefault();
    }

    private Planet? FindPlanetForMoon(string moonName)
    {
        if (moonName.Contains("Belt Cluster"))
        {
            // Belt cluster names like "NGC 6124 Sector QT-R c4-7 A Belt Cluster 1"
            // Find the planet by removing "Belt Cluster X" part
            var baseName = moonName;
            var beltIndex = baseName.IndexOf(" Belt Cluster");
            if (beltIndex > 0)
            {
                baseName = baseName.Substring(0, beltIndex);
            }

            return _bodyLookup.Values.OfType<Planet>().FirstOrDefault(p => p.BodyName == baseName);
        }
        else
        {
            // Moon names like "NGC 6124 Sector QT-R c4-7 1 a"
            // Remove the last part (" a", " b", etc.) to get planet name
            var parts = moonName.Split(' ');
            if (parts.Length > 0)
            {
                var planetName = string.Join(" ", parts.Take(parts.Length - 1));
                return _bodyLookup.Values.OfType<Planet>().FirstOrDefault(p => p.BodyName == planetName);
            }
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
                BodyType = "Star",
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

        if (scanEvent.BodyName.Contains("Belt Cluster"))
        {
            return new BeltCluster
            {
                BodyName = scanEvent.BodyName,
                BodyId = scanEvent.BodyId,
                BodyType = "Belt Cluster",
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

        if (scanEvent.BodyName.Contains("Ring"))
        {
            return new Ring
            {
                BodyName = scanEvent.BodyName,
                BodyId = scanEvent.BodyId,
                BodyType = "Ring",
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
            BodyType = "Planet",
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

    public void DebugHierarchy()
    {
        if (_currentSystem == null)
        {
            Console.WriteLine("No current system");
            return;
        }

        Console.WriteLine($"=== FINAL HIERARCHY ===");
        Console.WriteLine($"System: {_currentSystem.BodyName} ({_currentSystem.Children.Count} direct children)");

        foreach (var star in _currentSystem.Children.OfType<Star>())
        {
            Console.WriteLine($"  Star: {star.BodyName} ({star.Children.Count} children)");
            foreach (var planet in star.Children.OfType<Planet>())
            {
                Console.WriteLine($"    Planet: {planet.BodyName} ({planet.Children.Count} children)");
                foreach (var moon in planet.Children)
                {
                    Console.WriteLine($"      Moon/Cluster: {moon.BodyName}");
                }
            }
            // Also show non-planet children of star
            foreach (var other in star.Children.Where(c => c is not Planet))
            {
                Console.WriteLine($"    Other: {other.BodyName}");
            }
        }
    }
}