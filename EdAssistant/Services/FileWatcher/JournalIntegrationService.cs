namespace EdAssistant.Services.FileWatcher;

class JournalIntegrationService(IJournalMonitorService journalMonitorService, ILogger<JournalIntegrationService> logger)
    : IJournalIntegrationService
{
    public async Task InitializeAsync()
    {
        try
        {
            // Subscribe to monitor events before starting
            journalMonitorService.JournalEventReceived += OnJournalEventReceived;
            journalMonitorService.NewJournalFileCreated += OnNewJournalFileCreated;
            journalMonitorService.JournalPathChanged += OnJournalPathChanged;

            // Load existing journal data first (your existing logic)
            logger.LogInformation("Loading existing journal data...");
            // You can call your existing journal service methods here if needed
            // Example: await _journalService.RefreshCacheAsync();

            // Start real-time monitoring
            logger.LogInformation("Starting real-time journal monitoring...");
            journalMonitorService.Start();
            
            logger.LogInformation("Journal service orchestrator initialized successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to initialize journal service orchestrator");
            throw;
        }
    }

    private void OnJournalPathChanged(object? sender, JournalPathChangedEventArgs e)
    {
        logger.LogInformation("Journal monitoring path changed from '{OldPath}' to '{NewPath}' at {ChangedAt}", 
            e.OldPath, e.NewPath, e.ChangedAt);
    }

    public void Dispose()
    {
        journalMonitorService.JournalEventReceived -= OnJournalEventReceived;
        journalMonitorService.NewJournalFileCreated -= OnNewJournalFileCreated;
        journalMonitorService.JournalPathChanged -= OnJournalPathChanged;
        journalMonitorService.Dispose();
    }
    
    private async void OnJournalEventReceived(object? sender, JournalEventArgs e)
    {
        try
        {
            logger.LogDebug("Received journal event: {EventType} from {FileName}", 
                e.JournalEvent?.GetType().Name ?? "Unknown", e.FileName);

            // Process the new event - you can integrate with your existing ViewModels here
            await ProcessJournalEvent(e.JournalEvent, e.RawJson, e.FileName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing journal event");
        }
    }
    
    private void OnNewJournalFileCreated(object? sender, JournalFileEventArgs e)
    {
        logger.LogInformation("New Elite Dangerous session started: {FileName}", e.FileName);
        // I might want to trigger UI updates or notifications here
    }
    
    private async Task ProcessJournalEvent(JournalEvent? journalEvent, string rawJson, string fileName)
    {
        if (journalEvent == null)
        {
            logger.LogWarning("Received null journal event from {FileName}", fileName);
            return;
        }

        // Handle different event types and update your ViewModels accordingly
        switch (journalEvent)
        {
            case CargoEvent cargoEvent:
                // Trigger cargo ViewModel update
                WeakReferenceMessenger.Default.Send(new CargoUpdatedMessage(cargoEvent));
                break;

            case MaterialsEvent materialsEvent:
                // Trigger materials ViewModel update
                WeakReferenceMessenger.Default.Send(new MaterialsUpdatedMessage(materialsEvent));
                break;

            case ShipLockerEvent shipLockerEvent:
                // Trigger storage ViewModel update
                WeakReferenceMessenger.Default.Send(new StorageUpdatedMessage(shipLockerEvent));
                break;

            case ScanEvent scanEvent:
                // Trigger system ViewModel update
                WeakReferenceMessenger.Default.Send(new SystemScanMessage(scanEvent));
                break;

            case CommanderEvent commanderEvent:
                // Update commander info
                WeakReferenceMessenger.Default.Send(new CommanderMessage(commanderEvent.Name));
                break;

            case RankEvent rankEvent:
            case ProgressEvent progressEvent:
                // Trigger home ViewModel update
                WeakReferenceMessenger.Default.Send(new RankProgressMessage(journalEvent));
                break;

            default:
                logger.LogDebug("Unhandled journal event type: {EventType}", journalEvent.GetType().Name);
                break;
        }
    }
}

public record CargoUpdatedMessage(CargoEvent CargoEvent);
public record MaterialsUpdatedMessage(MaterialsEvent MaterialsEvent);
public record StorageUpdatedMessage(ShipLockerEvent ShipLockerEvent);
public record SystemScanMessage(ScanEvent ScanEvent);
public record RankProgressMessage(JournalEvent JournalEvent);