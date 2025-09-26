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
            SignalTypeEnum.ConflictZone or SignalTypeEnum.ResourceExtraction or SignalTypeEnum.NotableStellarPhenomena
                or SignalTypeEnum.ListeningPost or SignalTypeEnum.MissionTarget or SignalTypeEnum.NavBeacon
                or SignalTypeEnum.ThargoidStructure or SignalTypeEnum.ThargoidBarnacle or SignalTypeEnum.ThargoidSite
                or SignalTypeEnum.GuardianStructure or SignalTypeEnum.GuardianSite or SignalTypeEnum.GuardianBeacon
                or SignalTypeEnum.AnomalonNebula or SignalTypeEnum.CrashSite or SignalTypeEnum.DistressCall
                or SignalTypeEnum.Tourist or SignalTypeEnum.Mining or SignalTypeEnum.PowerPlay or SignalTypeEnum.Wreck
                or SignalTypeEnum.SalvageableWreck or SignalTypeEnum.Community or SignalTypeEnum.Ceremony
                or SignalTypeEnum.PlanetarySettlement or SignalTypeEnum.PlanetaryInstallation
                or SignalTypeEnum.PlanetaryPort or SignalTypeEnum.Checkpoint
                or SignalTypeEnum.Scenario => CreateSignal(scanEvent),
            
            SignalTypeEnum.Outpost or SignalTypeEnum.AsteroidBase or SignalTypeEnum.StationCoriolis or SignalTypeEnum.Orbis
                or SignalTypeEnum.Ocellus or SignalTypeEnum.Installation or SignalTypeEnum.Settlement
                or SignalTypeEnum.StationMegaShip or SignalTypeEnum.FleetCarrier or SignalTypeEnum.Carrier
                or SignalTypeEnum.USS or SignalTypeEnum.NumberStation or SignalTypeEnum.StationONeilOrbis
                or SignalTypeEnum.StationONeilCylinder => CreateStation(scanEvent),
            
            _ => new Generic { BodyName = GetDisplayName(scanEvent) }
        };

    private static CelestialBody CreateSignal(FSSSignalDiscoveredEvent scanEvent) =>
        scanEvent.SignalType switch
        {
            SignalTypeEnum.ConflictZone => new ConflictZone { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.ResourceExtraction => new ResourceExtraction { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.NotableStellarPhenomena => new NotableStellarPhenomena { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.ListeningPost => new ListeningPost { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.MissionTarget => new MissionTarget { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.NavBeacon => new NavBeacon { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.ThargoidStructure => new ThargoidStructure { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.ThargoidBarnacle => new ThargoidBarnacle { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.ThargoidSite => new ThargoidSite { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.GuardianStructure => new GuardianStructure { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.GuardianSite => new GuardianSite { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.GuardianBeacon => new GuardianBeacon { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.AnomalonNebula => new AnomalonNebula { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.CrashSite => new CrashSite { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.DistressCall => new DistressCall { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Tourist => new Tourist { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Mining => new Mining { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.PowerPlay => new PowerPlay { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Wreck => new Wreck { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.SalvageableWreck => new SalvageableWreck { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Community => new Community { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Ceremony => new Ceremony { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.PlanetarySettlement => new PlanetarySettlement { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.PlanetaryInstallation => new PlanetaryInstallation { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.PlanetaryPort => new PlanetaryPort { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Checkpoint => new Checkpoint { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Scenario => new Scenario { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Combat => new Combat { BodyName = GetDisplayName(scanEvent) },
            _ => new Generic { BodyName = GetDisplayName(scanEvent) },
        };

    private static CelestialBody CreateStation(FSSSignalDiscoveredEvent scanEvent) =>
        scanEvent.SignalType switch
        {
            SignalTypeEnum.Outpost => new Outpost { BodyName = GetDisplayName(scanEvent) }, 
            SignalTypeEnum.AsteroidBase => new AsteroidBase { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.StationCoriolis => new Coriolis { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.StationONeilOrbis or SignalTypeEnum.Orbis => new Orbis { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Ocellus or SignalTypeEnum.StationONeilCylinder => new Ocellus { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Installation => new Installation { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Settlement => new Settlement { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Megaship => new MegaShip { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.StationMegaShip => new StationMegaShip { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.FleetCarrier => new FleetCarrier { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.Carrier => new Carrier { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.SquadronCarrier => new SquadronCarrier { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.USS => new USS { BodyName = GetDisplayName(scanEvent) },
            SignalTypeEnum.NumberStation => new NumberStation { BodyName = GetDisplayName(scanEvent) },
            _ => new UnknownStation { BodyName = GetDisplayName(scanEvent) },
        };
    
    private static string GetDisplayName(FSSSignalDiscoveredEvent scanEvent) => 
        !string.IsNullOrEmpty(scanEvent.SignalNameLocalised)
            ? scanEvent.SignalNameLocalised
            : scanEvent.SignalName;
}