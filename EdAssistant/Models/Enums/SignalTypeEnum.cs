namespace EdAssistant.Models.Enums;

public enum SignalTypeEnum
{
    UnknownStation,
    UnknownSignal,
    
    NavBeacon,
    
    // Traditional Stations
    Outpost,
    AsteroidBase,
    StationCoriolis,
    StationONeilOrbis,
    StationONeilCylinder,
    
    // Installations and Settlements
    Installation,
    StationMegaShip,
    Megaship,
    
    // Player-owned structures
    FleetCarrier,
    SquadronCarrier,
    
    // Combat zones and extraction sites
    ConflictZone,
    ResourceExtraction,
    Combat
}