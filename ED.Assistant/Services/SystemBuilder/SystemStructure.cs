namespace ED.Assistant.Services.SystemBuilder;

public sealed class SystemStructure
{
	public string Name { get; init; } = "Unknown system";
	public List<SystemBodyNode> Bodies { get; init; } = [];
}

public sealed class SystemBodyNode
{
	public int BodyId { get; init; }
	public string Name { get; init; } = string.Empty;
	public string Type { get; init; } = string.Empty;
	public List<SystemBodyNode> Children { get; init; } = [];
}