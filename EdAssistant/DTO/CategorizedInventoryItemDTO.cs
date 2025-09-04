namespace EdAssistant.DTO;

public class CategorizedInventoryItemDTO
{
    public required string Name { get; set; }
    public required string NameLocalised { get; set; }
    public string Icon { get; set; }
    public ItemCategory Category { get; set; }
}