using System.Text.Json;
using ED.Assistant.Plugins.Explorer.Data;
using ED.Assistant.Plugins.Explorer.Valuation;

namespace ED.Assistant.Plugins.Explorer.Journal;

/// <summary>
/// Applies journal events to an in-memory copy of the ExplorerBody table and tracks
/// which rows changed, so the caller can write them in batches.
/// </summary>
internal sealed class ExplorerJournalProcessor
{
    // Cheap filter before parsing JSON: most journal lines are irrelevant here
    private static readonly string[] Markers =
    [
        "\"Scan\"", "SAAScanComplete", "ExplorationData", "\"Died\"",
        "FSDJump", "\"Location\"", "CarrierJump"
    ];

    private readonly Dictionary<(long SystemAddress, int BodyId), ExplorerBody> _bodies = new();
    private readonly Dictionary<string, HashSet<ExplorerBody>> _unsoldBySystem = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<ExplorerBody> _added = [];
    private readonly HashSet<ExplorerBody> _modified = [];

    // Fallback for old journals whose Scan events lack StarSystem/SystemAddress
    private string? _currentSystem;
    private long? _currentSystemAddress;

    public ExplorerJournalProcessor(IEnumerable<ExplorerBody> existing)
    {
        foreach (var body in existing.OrderBy(b => b.Id))
        {
            _bodies[(body.SystemAddress, body.BodyId)] = body;
            if (body.IsActive)
                AddUnsold(body);
        }
    }

    public int PendingChanges => _added.Count + _modified.Count;

    public (IReadOnlyList<ExplorerBody> Added, IReadOnlyList<ExplorerBody> Modified) GetChanges() =>
        (_added.ToList(), _modified.ToList());

    /// <summary>Call after the changes were saved.</summary>
    public void AcceptChanges()
    {
        _added.Clear();
        _modified.Clear();
    }

    public void Apply(string line)
    {
        if (!Markers.Any(m => line.Contains(m, StringComparison.Ordinal)))
            return;

        JsonDocument document;
        try
        {
            document = JsonDocument.Parse(line);
        }
        catch (JsonException)
        {
            return;
        }

        using (document)
        {
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object || Str(root, "event") is not { } name)
                return;

            var timestamp = Date(root, "timestamp");
            if (timestamp is null)
                return;

            switch (name)
            {
                case "Scan":
                    OnScan(root, timestamp.Value);
                    break;
                case "SAAScanComplete":
                    OnMapped(root, timestamp.Value);
                    break;
                case "SellExplorationData":
                case "MultiSellExplorationData":
                    OnSold(root, timestamp.Value);
                    break;
                case "Died":
                    OnDied();
                    break;
                case "FSDJump":
                case "Location":
                case "CarrierJump":
                    _currentSystem = Str(root, "StarSystem") ?? _currentSystem;
                    _currentSystemAddress = Long(root, "SystemAddress") ?? _currentSystemAddress;
                    break;
            }
        }
    }

    private void OnScan(JsonElement root, DateTime timestamp)
    {
        // Stars and belt clusters have no PlanetClass
        if (Str(root, "PlanetClass") is not { Length: > 0 } planetClass)
            return;

        // Nav beacon data is not your own discovery
        if (Str(root, "ScanType") == "NavBeaconDetail")
            return;

        var isTerraformable = Str(root, "TerraformState") is "Terraformable" or "Terraforming";

        // Only bodies the valuation table knows about are tracked
        if (!BodyValueCatalog.TryGet(planetClass, isTerraformable, out _))
            return;

        if ((Long(root, "SystemAddress") ?? _currentSystemAddress) is not { } systemAddress ||
            Int(root, "BodyID") is not { } bodyId)
            return;

        if (_bodies.TryGetValue((systemAddress, bodyId), out var body))
        {
            // Already tracked (unsold or sold): re-scanning adds nothing new.
            // Only data lost on death becomes sellable again.
            if (!body.IsLost)
                return;
        }
        else
        {
            body = new ExplorerBody { SystemAddress = systemAddress, BodyId = bodyId };
            _bodies[(systemAddress, bodyId)] = body;
            _added.Add(body);
        }

        body.StarSystem = Str(root, "StarSystem")
                          ?? (systemAddress == _currentSystemAddress ? _currentSystem : null)
                          ?? string.Empty;
        body.BodyName = Str(root, "BodyName") ?? string.Empty;
        body.PlanetClass = planetClass;
        body.IsTerraformable = isTerraformable;
        body.MassEm = Double(root, "MassEM");
        // Missing flags (very old journals) count as "already discovered/mapped": never overestimate
        body.WasDiscovered = Bool(root, "WasDiscovered") ?? true;
        body.WasMapped = Bool(root, "WasMapped") ?? true;
        body.IsMapped = false;
        body.IsEfficientMapping = false;
        body.DateScanned = timestamp;
        body.DateMapped = null;
        body.DateSold = null;
        body.IsLost = false;
        body.IsActive = true;
        body.Value = BodyValueCatalog.Calculate(body);

        AddUnsold(body);
        MarkModified(body);
    }

    private void OnMapped(JsonElement root, DateTime timestamp)
    {
        if (Long(root, "SystemAddress") is not { } systemAddress || Int(root, "BodyID") is not { } bodyId)
            return;

        if (!_bodies.TryGetValue((systemAddress, bodyId), out var body) || !body.IsActive || body.IsMapped)
            return;

        var probesUsed = Int(root, "ProbesUsed");
        var target = Int(root, "EfficiencyTarget");

        body.IsMapped = true;
        body.IsEfficientMapping = probesUsed is not null && target is not null && probesUsed <= target;
        body.DateMapped = timestamp;
        body.Value = BodyValueCatalog.Calculate(body);

        MarkModified(body);
    }

    private void OnSold(JsonElement root, DateTime timestamp)
    {
        var systems = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // SellExplorationData: "Systems": ["name", ...]
        if (root.TryGetProperty("Systems", out var names) && names.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in names.EnumerateArray())
                if (item.ValueKind == JsonValueKind.String && item.GetString() is { } system)
                    systems.Add(system);
        }

        // Both events: "Discovered": [{ "SystemName": "...", "NumBodies": n }, ...]
        if (root.TryGetProperty("Discovered", out var discovered) && discovered.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in discovered.EnumerateArray())
                if (item.ValueKind == JsonValueKind.Object && Str(item, "SystemName") is { } system)
                    systems.Add(system);
        }

        foreach (var system in systems)
        {
            if (!_unsoldBySystem.TryGetValue(system, out var bodies))
                continue;

            foreach (var body in bodies.Where(b => b.DateScanned <= timestamp).ToList())
            {
                body.IsActive = false;
                body.DateSold = timestamp;
                bodies.Remove(body);
                MarkModified(body);
            }

            if (bodies.Count == 0)
                _unsoldBySystem.Remove(system);
        }
    }

    // Unsold exploration data is lost when the commander dies
    private void OnDied()
    {
        foreach (var body in _unsoldBySystem.Values.SelectMany(b => b))
        {
            body.IsActive = false;
            body.IsLost = true;
            MarkModified(body);
        }

        _unsoldBySystem.Clear();
    }

    private void AddUnsold(ExplorerBody body)
    {
        if (!_unsoldBySystem.TryGetValue(body.StarSystem, out var bodies))
            _unsoldBySystem[body.StarSystem] = bodies = [];

        bodies.Add(body);
    }

    // New rows are inserted as a whole; only rows that already exist in the DB need an update
    private void MarkModified(ExplorerBody body)
    {
        if (!_added.Contains(body))
            _modified.Add(body);
    }

    private static string? Str(JsonElement e, string name) =>
        e.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;

    private static long? Long(JsonElement e, string name) =>
        e.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetInt64(out var r) ? r : null;

    private static int? Int(JsonElement e, string name) =>
        e.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetInt32(out var r) ? r : null;

    private static double? Double(JsonElement e, string name) =>
        e.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetDouble(out var r) ? r : null;

    private static bool? Bool(JsonElement e, string name) =>
        e.TryGetProperty(name, out var v)
            ? v.ValueKind switch { JsonValueKind.True => true, JsonValueKind.False => false, _ => null }
            : null;

    private static DateTime? Date(JsonElement e, string name) =>
        e.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String && v.TryGetDateTime(out var d)
            ? d.ToUniversalTime()
            : null;
}
