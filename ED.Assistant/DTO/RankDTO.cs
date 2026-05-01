namespace ED.Assistant.DTO;

public sealed class RankDTO
{
	public string? Name { get; set; } = string.Empty;
	public string Level { get; set; } = string.Empty;
	public int Progress { get; set; } = default;
}
