namespace EdAssistant.Services.SystemStructure;

public interface ICelestialStructure
{
    SystemNode? SystemRoot { get; }
    void AddScanEvent(ScanEvent scanEvent);
    void AddFSSSignalDiscoveredEvent(FSSSignalDiscoveredEvent fssSignal);
    void AddLocationScan(LocationEvent locationEvent);
    void BuildHierarchy();
}