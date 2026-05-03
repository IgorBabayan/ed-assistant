using ED.Assistant.Data.Models.Events;
using ED.Assistant.Data.Types;

namespace ED.Assistant.Services.SystemBuilder;

public sealed class SystemStructure
{
	public string Name { get; init; } = string.Empty;
	public List<SystemBodyNode> Roots { get; } = new();
}

public sealed class SystemBodyNode
{
	public string Name { get; set; } = string.Empty;
	public int BodyId { get; set; }
	public string Type { get; set; } = string.Empty;

	public SystemBodyNode? Parent { get; set; }
	public List<SystemBodyNode> Children { get; } = new();

	public ScanEvent? Scan { get; set; }

	public FSSBodySignalsEvent? Signals { get; set; }
}