namespace EdAssistant.DTO;

public sealed class RankDTO
{
    public required RankEnum Rank { get; set; }
    public required string Title { get; set; }
    public required int Progress { get; set; }
}