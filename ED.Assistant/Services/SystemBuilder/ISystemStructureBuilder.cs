using ED.Assistant.Data.Models.Events;
using ED.Assistant.Data.Types;

namespace ED.Assistant.Services.SystemBuilder;

public interface ISystemStructureBuilder
{
	SystemStructure Build(JournalState state);
}

class SystemStructureBuilder : ISystemStructureBuilder
{
	public SystemStructure Build(JournalState state)
	{
		ArgumentNullException.ThrowIfNull(state);

		if (state.FSDJump is null)
			throw new ArgumentException($"{nameof(state.FSDJump)} event is required to build system structure.", nameof(state));

		var root = new SystemBodyNode
		{
			BodyId = -1,
			Name = state.FSDJump.StarSystem,
			Kind = SystemBodyKind.System
		};

		var structure = new SystemStructure
		{
			Root = root
		};

		BuildHierarchy(structure, state);

		return structure;
	}

	private static void BuildHierarchy(SystemStructure structure, JournalState state)
	{
		var systemAddress = state.FSDJump!.SystemAddress;

		var barycentres = state.BaryCentres?.Values
			.Where(x => x.SystemAddress == systemAddress)
			.ToList() ?? [];

		var scans = state.Scans?.Values
			.Where(x => x.SystemAddress == systemAddress)
			.ToList() ?? [];

		var nodesByBodyId = new Dictionary<int, SystemBodyNode>();

		foreach (var barycentre in barycentres)
		{
			nodesByBodyId[barycentre.BodyId] = new SystemBodyNode
			{
				BodyId = barycentre.BodyId,
				Name = $"Barycentre {barycentre.BodyId}",
				Kind = SystemBodyKind.Barycentre
			};
		}

		foreach (var scan in scans)
		{
			nodesByBodyId[scan.BodyId] = new SystemBodyNode
			{
				BodyId = scan.BodyId,
				Name = scan.BodyName,
				Kind = GetBodyKind(scan)
			};
		}

		foreach (var barycentre in barycentres)
		{
			var node = nodesByBodyId[barycentre.BodyId];

			var parentId = GetNearestParentBodyId(barycentre.Parents);

			AttachNode(structure.Root, nodesByBodyId, node, parentId);
		}

		foreach (var scan in scans)
		{
			var node = nodesByBodyId[scan.BodyId];

			var parentId = GetNearestParentBodyId(scan.Parents);

			AttachNode(structure.Root, nodesByBodyId, node, parentId);
		}
	}

	private static void AttachNode(
		SystemBodyNode root,
		Dictionary<int, SystemBodyNode> nodesByBodyId,
		SystemBodyNode node,
		int? parentId)
	{
		if (parentId is not null &&
			nodesByBodyId.TryGetValue(parentId.Value, out var parentNode) &&
			parentNode != node)
		{
			parentNode.Children.Add(node);
			return;
		}

		root.Children.Add(node);
	}

	private static int? GetNearestParentBodyId(IEnumerable<Parent>? parents)
	{
		if (parents?.Any() != true)
			return null;

		return parents!.First().BodyId;
	}

	private static SystemBodyKind GetBodyKind(ScanEvent scan)
	{
		if (!string.IsNullOrWhiteSpace(scan.StarType))
			return SystemBodyKind.Star;

		if (!string.IsNullOrWhiteSpace(scan.PlanetClass))
			return SystemBodyKind.Planet;

		if (scan.BodyName.Contains("Belt", StringComparison.OrdinalIgnoreCase))
			return SystemBodyKind.AsteroidBelt;

		return SystemBodyKind.Unknown;
	}
}