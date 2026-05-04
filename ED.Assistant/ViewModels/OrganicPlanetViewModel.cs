using System.Collections.ObjectModel;

namespace ED.Assistant.ViewModels;

public sealed class OrganicPlanetViewModel
{
	public int BodyId { get; init; }
	public string BodyName { get; init; } = string.Empty;

	public ObservableCollection<OrganicSignalViewModel> Signals { get; } = [];
}
