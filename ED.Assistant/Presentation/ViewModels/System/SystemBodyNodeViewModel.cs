using ED.Assistant.Domain.System;

namespace ED.Assistant.Presentation.ViewModels.System;

public partial class SystemBodyNodeViewModel : BaseViewModel
{
	public string Name { get; }

	public string Type { get; }

	public int BodyId { get; }

	public ScanEvent? Scan { get; }

	public ObservableCollection<SystemBodyNodeViewModel> Children { get; } = [];

	public bool HasScan => Scan is not null;

	public bool IsStar => !string.IsNullOrWhiteSpace(Scan?.StarType);

	public bool IsPlanet => !string.IsNullOrWhiteSpace(Scan?.PlanetClass);

	public string DistanceLs =>
		Scan is null ? "-" : $"{Scan.DistanceFromArrivalLS:N0} ls";

	public string RadiusKm =>
		Scan is null ? "-" : $"{Scan.Radius / 1000:N0} km";

	public string SurfaceTemperatureK =>
		Scan is null ? "-" : $"{Scan.SurfaceTemperature:N0} K";

	public string Gravity =>
		Scan is null ? "-" : $"{Scan.SurfaceGravity:N2} g";

	public string Mass =>
		Scan is null ? "-" :
		IsStar ? $"{Scan.StellarMass:N2} SM"
			   : $"{Scan.MassEM:N2} EM";

	public ObservableCollection<SignalItemViewModel> Signals { get; } = [];

	public bool HasSignals => Signals.Count > 0;

	public int SignalsTotalCount => Signals.Sum(x => x.Count);

	public SystemBodyNodeViewModel(SystemBodyNode node)
	{
		Name = node.Name;
		Type = node.Type;
		BodyId = node.BodyId;
		Scan = node.Scan;

		foreach (var child in node.Children)
			Children.Add(new SystemBodyNodeViewModel(child));

		foreach (var signal in node.Signals?.Signals ?? [])
			Signals.Add(new SignalItemViewModel(signal));
	}
}

public sealed class SignalItemViewModel
{
	public string Type { get; }
	public string Name { get; }
	public int Count { get; }

	public SignalItemViewModel(SignalItem signal)
	{
		Type = signal.TypeId;
		Name = string.IsNullOrWhiteSpace(signal.Name)
			? signal.TypeId
			: signal.Name;
		Count = signal.Count;
	}
}
