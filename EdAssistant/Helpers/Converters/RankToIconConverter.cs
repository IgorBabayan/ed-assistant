namespace EdAssistant.Helpers.Converters;

public class RankToIconConverter : IValueConverter
{
    private static readonly Dictionary<RankEnum, Bitmap> _iconCache = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is RankEnum rank)
        {
            if (_iconCache.TryGetValue(rank, out var cachedBitmap))
                return cachedBitmap;

            var iconPath = rank switch
            {
                RankEnum.Combat => "avares://EdAssistant/Assets/Icons/Ranks/Combat.png",
                RankEnum.Trade => "avares://EdAssistant/Assets/Icons/Ranks/Trader.png",
                RankEnum.Explore => "avares://EdAssistant/Assets/Icons/Ranks/Explorer.png",
                RankEnum.Soldier => "avares://EdAssistant/Assets/Icons/Ranks/Soldier.png",
                RankEnum.Exobiologist => "avares://EdAssistant/Assets/Icons/Ranks/Exobiologist.png",
                RankEnum.Empire => "avares://EdAssistant/Assets/Icons/Ranks/Empire.png",
                RankEnum.Federation => "avares://EdAssistant/Assets/Icons/Ranks/Federation.png",
                RankEnum.CQC => "avares://EdAssistant/Assets/Icons/Ranks/CQC.png",
                _ => null
            };

            if (iconPath is not null)
            {
                try
                {
                    var uri = new Uri(iconPath);
                    var bitmap = new Bitmap(AssetLoader.Open(uri));
                    _iconCache[rank] = bitmap;

                    return bitmap;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(Localization.Instance["Exceptions.LoadingIconError"], exception.Message);
                    return null;
                }
            }
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}