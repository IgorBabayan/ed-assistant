namespace ED.Assistant.Domain.System;

public sealed class SystemBodyNode
{
	public string Name { get; init; } = string.Empty;
	public int BodyId { get; init; }
	public string Type { get; init; } = string.Empty;

	public SystemBodyNode? Parent { get; set; }
	public List<SystemBodyNode> Children { get; } = [];

	public ScanEvent? Scan { get; set; }

	public FSSBodySignalsEvent? Signals { get; set; }
}