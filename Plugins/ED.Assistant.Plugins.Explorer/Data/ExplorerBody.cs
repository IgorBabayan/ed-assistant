namespace ED.Assistant.Plugins.Explorer.Data;

/// <summary>One scanned body. A body is stored once; it's re-armed only if its data was lost on death.</summary>
public sealed class ExplorerBody
{
    public int Id { get; set; }

    public long SystemAddress { get; set; }
    public int BodyId { get; set; }
    public string StarSystem { get; set; } = string.Empty;
    public string BodyName { get; set; } = string.Empty;

    /// <summary>Journal value, e.g. "Earthlike body", "Sudarsky class II gas giant".</summary>
    public string PlanetClass { get; set; } = string.Empty;
    public bool IsTerraformable { get; set; }
    public double? MassEm { get; set; }

    /// <summary>From the Scan event: true means someone else discovered it first.</summary>
    public bool WasDiscovered { get; set; }
    /// <summary>From the Scan event: true means someone else mapped it first.</summary>
    public bool WasMapped { get; set; }

    public bool IsMapped { get; set; }
    public bool IsEfficientMapping { get; set; }

    public DateTime DateScanned { get; set; }
    public DateTime? DateMapped { get; set; }
    public DateTime? DateSold { get; set; }

    /// <summary>Unsold data destroyed by a Died event.</summary>
    public bool IsLost { get; set; }

    /// <summary>Unsold and not lost.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Estimated value in credits.</summary>
    public long Value { get; set; }
}
