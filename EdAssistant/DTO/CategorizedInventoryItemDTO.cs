namespace EdAssistant.DTO;

public class CategorizedInventoryItemDTO
{
    public required string Name { get; set; }
    public required string NameLocalised { get; set; }
    public string Icon { get; set; }
    public ItemCategory Category { get; set; }

    public SolidColorBrush CategoryColor => Category switch
    {
        ItemCategory.Items => new SolidColorBrush(Color.FromRgb(52, 152, 219)),
        ItemCategory.Components => new SolidColorBrush(Color.FromRgb(155, 89, 182)),
        ItemCategory.Consumables => new SolidColorBrush(Color.FromRgb(46, 204, 113)),
        ItemCategory.Data => new SolidColorBrush(Color.FromRgb(230, 126, 34)),
        _ => new SolidColorBrush(Colors.Gray)
    };
}