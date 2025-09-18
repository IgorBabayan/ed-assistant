namespace EdAssistant.DTO;

public sealed class MaterialsInventoryItemDTO
{
    public required string Name { get; set; }
    public string Icon { get; set; }
    public MaterialCategoryEnum CategoryEnum { get; set; }

    public SolidColorBrush CategoryColor => CategoryEnum switch
    {
        MaterialCategoryEnum.Raw => new SolidColorBrush(Color.FromRgb(52, 152, 219)),
        MaterialCategoryEnum.Manufactured => new SolidColorBrush(Color.FromRgb(155, 89, 182)),
        MaterialCategoryEnum.Encoded => new SolidColorBrush(Color.FromRgb(46, 204, 113)),
        _ => new SolidColorBrush(Colors.Gray)
    };
}