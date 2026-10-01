namespace ED.Assistant.Plugins.MarketConnector.Settings;

/// <summary>EDMC "EDDN" tab.</summary>
public sealed record EddnSettings
{
    /// <summary>Market, outfitting and shipyard data (from Market.json / Outfitting.json / Shipyard.json).</summary>
    public bool SendStationData { get; init; } = true;

    /// <summary>System, body and scan data (FSDJump, Scan, FSSDiscoveryScan, ...).</summary>
    public bool SendSystemData { get; init; } = true;

    /// <summary>Queue messages and send them only after docking (bad connections).</summary>
    public bool DelayUntilDocked { get; init; }
}

/// <summary>EDMC "EDSM" tab.</summary>
public sealed record EdsmSettings
{
    public bool Enabled { get; init; }
    public string? CommanderName { get; init; }
    public string? ApiKey { get; init; }
}
