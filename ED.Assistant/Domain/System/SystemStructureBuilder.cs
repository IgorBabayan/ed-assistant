using ED.Assistant.Domain.Types;

namespace ED.Assistant.Domain.System;

sealed class SystemStructureBuilder : ISystemStructureBuilder
{
	public SystemStructure Build(JournalState state)
	{
		ArgumentNullException.ThrowIfNull(state);

		if (state.CurrentSystemAddress is not { } currentSystemAddress)
			return new SystemStructure();
		var structure = new SystemStructure
		{
			Name = state.Location?.StarSystem ?? state.FSDJump?.StarSystem ?? string.Empty
		};

		var scans = state.Scans.Values
			.Where(x => x.SystemAddress == currentSystemAddress)
			.ToList();

		var nodes = new Dictionary<int, SystemBodyNode>();
		foreach (var scan in scans)
		{
			var node = GetOrCreateNode(nodes, scan);
			node.Scan = scan;

			if (state.FSSSignals.TryGetValue(scan.BodyId, out var signals) &&
				signals.SystemAddress == scan.SystemAddress)
			{
				node.Signals = signals;
			}
		}

		foreach (var scan in scans)
		{
			if (!nodes.TryGetValue(scan.BodyId, out var node))
				continue;

			var parentId = GetParentId(scan);
			if (parentId is not null && nodes.TryGetValue(parentId.Value, out var parent)
			    && !CreatesCycle(node, parent))
			{
				parent.Children.Add(node);
				node.Parent = parent;
			}
			else
			{
				structure.Roots.Add(node);
			}
		}

		return structure;
	}

	private static bool CreatesCycle(SystemBodyNode node, SystemBodyNode? parent)
	{
		for (; parent is not null; parent = parent.Parent)
			if (ReferenceEquals(parent, node))
				return true;
		return false;
	}

	private static SystemBodyNode GetOrCreateNode(Dictionary<int, SystemBodyNode> nodes, ScanEvent scan)
	{
		if (nodes.TryGetValue(scan.BodyId, out var existing))
			return existing;

		var node = new SystemBodyNode
		{
			Name = scan.BodyName,
			BodyId = scan.BodyId,
			Type = GetType(scan)
		};

		nodes[scan.BodyId] = node;
		return node;
	}

	private static int? GetParentId(ScanEvent scan)
	{
		if (scan.Parents is null)
			return null;

		foreach (var parent in scan.Parents)
		{
			if (parent.Type == ParentType.Null)
				continue;

			return parent.BodyId;
		}

		return null;
	}

	private static string GetType(ScanEvent scan)
	{
		if (!string.IsNullOrEmpty(scan.StarType))
			return "Star";

		if (!string.IsNullOrEmpty(scan.PlanetClass))
			return "Planet";

		if (scan.BodyName.Contains("Belt Cluster"))
			return "Belt Cluster";

		return "Unknown";
	}
}