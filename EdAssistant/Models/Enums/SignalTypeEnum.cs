namespace EdAssistant.Models.Enums;

public enum SignalTypeEnum
{
    Unknown,
    
    // Traditional Stations
    Outpost,
    AsteroidBase,
    StationCoriolis,
    Orbis,
    Ocellus,
    StationONeilOrbis,
    StationONeilCylinder,
    
    // Installations and Settlements
    Installation,
    Settlement,
    StationMegaShip,
    Megaship,
    
    // Player-owned structures
    FleetCarrier,
    Carrier,
    SquadronCarrier,
    
    // Combat zones and extraction sites
    ConflictZone,
    ResourceExtraction,
    
    // Signal sources
    USS,
    
    // Points of Interest
    NotableStellarPhenomena,
    ListeningPost,
    NumberStation,
    
    // Mission-related signals
    MissionTarget,
    NavBeacon,
    
    // Thargoid-related
    ThargoidStructure,
    ThargoidBarnacle,
    ThargoidSite,
    
    // Guardian-related
    GuardianStructure,
    GuardianSite,
    GuardianBeacon,
    
    // Other special signals
    AnomalonNebula,
    CrashSite,
    DistressCall,
    Tourist,
    Mining,
    PowerPlay,
    
    // Salvage and wrecks
    Wreck,
    SalvageableWreck,
    
    // Event-specific signals
    Community,
    Ceremony,
    
    // Surface installations (Odyssey)
    PlanetarySettlement,
    PlanetaryInstallation,
    PlanetaryPort,
    
    // Other signal types that may appear
    Checkpoint,
    Scenario,
    Combat,
    Generic
}