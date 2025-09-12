namespace EdAssistant.Helpers.Converters;

public class RankToIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is RankEnum rank)
        {
            return rank switch
            {
                RankEnum.Combat => "avares://EdAssistant/Assets/Icons/Ranks/Combat.svg",
                RankEnum.Trade => "avares://EdAssistant/Assets/Icons/Ranks/Trader.svg",
                RankEnum.Explore => "avares://EdAssistant/Assets/Icons/Ranks/Explore.svg",
                RankEnum.Soldier => "avares://EdAssistant/Assets/Icons/Ranks/Soldier.svg",
                RankEnum.Exobiologist => "avares://EdAssistant/Assets/Icons/Ranks/Exobiologist.svg",
                RankEnum.Empire => "avares://EdAssistant/Assets/Icons/Ranks/Empire.svg",
                RankEnum.Federation => "avares://EdAssistant/Assets/Icons/Ranks/Federation.svg",
                RankEnum.CQC => "avares://EdAssistant/Assets/Icons/Ranks/CQC.svg",
                _ => null,
            };
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}