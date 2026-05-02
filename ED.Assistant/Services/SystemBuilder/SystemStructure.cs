using ED.Assistant.Data.Types;

namespace ED.Assistant.Services.SystemBuilder;

public sealed class SystemStructure
{
	public string Name { get; init; } = string.Empty;
	public SystemBodyNode Root { get; init; } = new();
}

public sealed class SystemBodyNode
{
	public int BodyId { get; init; }
	public string Name { get; init; } = string.Empty;
	public string Type { get; init; } = string.Empty;
	public SystemBodyKind Kind { get; init; }
	public List<SystemBodyNode> Children { get; init; } = [];
}