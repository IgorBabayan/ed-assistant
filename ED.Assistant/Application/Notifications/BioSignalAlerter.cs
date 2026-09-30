using ED.Assistant.Domain.Types;
using Material.Icons;

namespace ED.Assistant.Application.Notifications;

sealed class BioSignalAlerter(AlertService alerts)
{
    // Survives state snapshots; one alert per body per stage (FSS / DSS)
    private readonly HashSet<string> _seen = [];
    private readonly Lock _lock = new();

    public void Publish(IEnumerable<BioAlert> pending)
    {
        foreach (var alert in pending)
        {
            var stage = alert.Genera is { Length: > 0 } ? "dss" : "fss";

            lock (_lock)
            {
                if (!_seen.Add($"{alert.SystemAddress}:{alert.BodyId}:{stage}"))
                    continue;
            }

            alerts.Notify(
                $"Biological signals: {alert.Count}",
                alert.Genera is { Length: > 0 } g
                    ? $"{alert.BodyName} — {string.Join(", ", g)}"
                    : alert.BodyName,
                MaterialIconKind.Bacteria);
        }
    }
}