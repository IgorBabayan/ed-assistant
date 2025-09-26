namespace EdAssistant.Helpers.Factory.StarSystem;

class CelestialBodyFactory(ILogger<CelestialBodyFactory> logger)  : ICelestialBodyFactory
{
    public CelestialBody? Create(JournalEvent scan)
    {
        if (scan is ScanEvent scanEvent)
            return CreateBody(scanEvent);

        if (scan is FSSSignalDiscoveredEvent discoveredScan)
            return CreateSignalBody(discoveredScan);
        
        logger.LogInformation(Localization.Instance["CelestialBodyFactory.Exceptions.Unknown"]);
        return null;
    }

    private static CelestialBody CreateStar(ScanEvent scanEvent) =>
        new Star
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

    private static CelestialBody CreateBeltCluster(ScanEvent scanEvent) =>
        new BeltCluster
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
            MeanAnomaly = scanEvent.MeanAnomaly,
            Parents = scanEvent.Parents is not null
                ? new ReadOnlyCollection<ParentInfo>(scanEvent.Parents!)
                : null
        };

    private static CelestialBody CreateRing(ScanEvent scanEvent) =>
        new Ring
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
            MeanAnomaly = scanEvent.MeanAnomaly,
            Parents = scanEvent.Parents is not null
                ? new ReadOnlyCollection<ParentInfo>(scanEvent.Parents!)
                : null
        };

    private static CelestialBody CreatePlanet(ScanEvent scanEvent) =>
        new Planet
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
            }).ToList(),
            Parents = scanEvent.Parents is not null
                ? new ReadOnlyCollection<ParentInfo>(scanEvent.Parents!)
                : null
        };

    private static CelestialBody CreateBody(ScanEvent scanEvent)
    {
        if (scanEvent.StarType is not null)
            return CreateStar(scanEvent);
        
        if (scanEvent.BodyName.Contains(Localization.Instance["CelestialInfo.BeltCluster"]))
            return CreateBeltCluster(scanEvent);
        
        if (scanEvent.BodyName.Contains(Localization.Instance["CelestialInfo.Ring"]))
            return CreateRing(scanEvent);
        
        return CreatePlanet(scanEvent);
    }

    private static CelestialBody CreateSignalBody(FSSSignalDiscoveredEvent scanEvent) =>
        scanEvent.SignalType switch
        {
            SignalTypeEnum.ConflictZone or SignalTypeEnum.ResourceExtraction
                or SignalTypeEnum.Combat => CreateSignal(scanEvent),
            
            SignalTypeEnum.Outpost or SignalTypeEnum.AsteroidBase or SignalTypeEnum.StationCoriolis
                or SignalTypeEnum.Installation or SignalTypeEnum.StationMegaShip or SignalTypeEnum.FleetCarrier
                or SignalTypeEnum.StationONeilOrbis or SignalTypeEnum.StationONeilCylinder 
                or SignalTypeEnum.NavBeacon or SignalTypeEnum.SquadronCarrier => CreateStation(scanEvent),
            
            _ => throw new NotImplementedException(nameof(scanEvent.SignalType)) 
        };

    private static CelestialBody CreateSignal(FSSSignalDiscoveredEvent scanEvent) =>
        scanEvent.SignalType switch
        {
            SignalTypeEnum.ConflictZone => new ConflictZone { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.ResourceExtraction => new ResourceExtraction { BodyName = GetDisplayName(scanEvent) },
            _ => new UnknownSignal { BodyName = GetDisplayName(scanEvent) },
        };

    private static CelestialBody CreateStation(FSSSignalDiscoveredEvent scanEvent) =>
        scanEvent.SignalType switch
        {
            SignalTypeEnum.Outpost => new Outpost { BodyName = GetDisplayName(scanEvent) }, 
            SignalTypeEnum.AsteroidBase => new AsteroidBase { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.StationCoriolis => new Coriolis { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.StationONeilOrbis => new Orbis { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.StationONeilCylinder => new Ocellus { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Installation => new Installation { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Megaship => new MegaShip { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.StationMegaShip => new StationMegaShip { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.FleetCarrier => new FleetCarrier { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.SquadronCarrier => new SquadronCarrier { BodyName = GetDisplayName(scanEvent) },
            _ => new UnknownStation { BodyName = GetDisplayName(scanEvent) },
        };
    
    private static string GetDisplayName(FSSSignalDiscoveredEvent scanEvent) => 
        !string.IsNullOrEmpty(scanEvent.SignalNameLocalised)
            ? scanEvent.SignalNameLocalised
            : scanEvent.SignalName;
}