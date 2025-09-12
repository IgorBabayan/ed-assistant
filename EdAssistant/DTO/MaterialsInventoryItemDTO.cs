namespace EdAssistant.DTO;

public sealed class MaterialsInventoryItemDTO
{
    public required string Name { get; set; }
    public string Icon { get; set; }
    public MaterialCategory Category { get; set; }

    public SolidColorBrush CategoryColor => Category switch
    {
        MaterialCategory.Raw => new SolidColorBrush(Color.FromRgb(52, 152, 219)),
        MaterialCategory.Manufactured => new SolidColorBrush(Color.FromRgb(155, 89, 182)),
        MaterialCategory.Encoded => new SolidColorBrush(Color.FromRgb(46, 204, 113)),
        _ => new SolidColorBrush(Colors.Gray)
    };
}