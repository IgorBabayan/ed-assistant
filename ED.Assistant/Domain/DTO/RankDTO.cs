namespace ED.Assistant.Domain.DTO;

public sealed class RankDTO
{
	public string? Name { get; set; } = string.Empty;
	public string Level { get; set; } = string.Empty;
	public ushort Value { get; set; } = default;
	public ushort Maximum { get; set; } = default;
}
