namespace EdAssistant.Helpers.Factory.Journal;

class JournalEventFactory(ILogger<JournalEventFactory> logger) : IJournalEventFactory
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    public JournalEvent? CreateEvent(string jsonLine)
    {
        using var doc = JsonDocument.Parse(jsonLine);
        var eventType = doc.RootElement.GetProperty("event").GetString();

        return eventType switch
        {
            "Fileheader" => CreateEvent<FileHeaderEvent>(jsonLine),
            "Commander" => CreateEvent<CommanderEvent>(jsonLine),
            "Materials" => CreateEvent<MaterialsEvent>(jsonLine),
            "Rank" => CreateEvent<RankEvent>(jsonLine),
            "Progress" => CreateEvent<ProgressEvent>(jsonLine),
            "Reputation" => CreateEvent<ReputationEvent>(jsonLine),
            "EngineerProgress" => CreateEvent<EngineerProgressEvent>(jsonLine),
            "LoadGame" => CreateEvent<LoadGameEvent>(jsonLine),
            "Location" => CreateEvent<LocationEvent>(jsonLine),
            "Docked" => CreateEvent<DockedEvent>(jsonLine),
            "Undocked" => CreateEvent<UndockedEvent>(jsonLine),
            "MarketBuy" => CreateEvent<MarketBuyEvent>(jsonLine),
            "Cargo" => CreateEvent<CargoEvent>(jsonLine),
            "ReceiveText" => CreateEvent<ReceiveTextEvent>(jsonLine),
            _ => HandleUnknownEvent(eventType, jsonLine)
        };
    }

    private JournalEvent? HandleUnknownEvent(string? eventType, string jsonLine)
    {
        logger.LogWarning(Localization.Instance["Exceptions.UnknownJournalEvent"],
            eventType, jsonLine.Length > 100 ? $"{jsonLine.Substring(0, 100)}..." : jsonLine);
        return null;
    }

    private T? CreateEvent<T>(string json) where T : JournalEvent
    {
        try
        {
            return JsonSerializer.Deserialize<T>(json, _options);
        }
        catch (JsonException exception)
        {
            logger.LogError(exception, string.Format(Localization.Instance["Exceptions.ParsingJournalEvent"], typeof(T).Name, exception.Message));
            return null;
        }
    }
}