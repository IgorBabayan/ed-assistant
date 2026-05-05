namespace ED.Assistant.Domain.System;

public sealed class SystemStructure
{
	public string Name { get; init; } = string.Empty;
	public List<SystemBodyNode> Roots { get; } = new();
}
