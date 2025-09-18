namespace EdAssistant.DTO;

public sealed class StorageInventoryItemDTO
{
    public required string Name { get; set; }
    public required string NameLocalised { get; set; }
    public string Icon { get; set; }
    public ItemCategoryEnum CategoryEnum { get; set; }

    public SolidColorBrush CategoryColor => CategoryEnum switch
    {
        ItemCategoryEnum.Items => new SolidColorBrush(Color.FromRgb(52, 152, 219)),
        ItemCategoryEnum.Components => new SolidColorBrush(Color.FromRgb(155, 89, 182)),
        ItemCategoryEnum.Consumables => new SolidColorBrush(Color.FromRgb(46, 204, 113)),
        ItemCategoryEnum.Data => new SolidColorBrush(Color.FromRgb(230, 126, 34)),
        _ => new SolidColorBrush(Colors.Gray)
    };
}