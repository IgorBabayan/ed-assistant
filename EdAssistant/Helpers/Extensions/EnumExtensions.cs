namespace EdAssistant.Helpers.Extensions;

public static class EnumExtensions
{
    public static string GetLocalizedDisplayName(this Enum enumValue)
    {
        var displayAttribute = enumValue.GetType()
            .GetMember(enumValue.ToString())[0]
            .GetCustomAttribute<DisplayAttribute>();

        if (displayAttribute?.Name is not null)
        {
            return Localization.Instance[displayAttribute.Name];
        }

        return enumValue.ToString();
    }

    public static string GetRankTitle(this RankEnum rank, int value)
    {
        var unknowRank = Localization.Instance["Rank.Unknown"];
        if (value < 0)
            return unknowRank;

        return rank switch
        {
            RankEnum.Combat => Enum.TryParse<CombatRankEnum>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.Trade => Enum.TryParse<TradeRankEnum>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.Explore => Enum.TryParse<ExplorationRankEnum>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.Soldier => Enum.TryParse<SoldierRankEnum>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.Exobiologist => Enum.TryParse<ExobiologistRankEnum>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.Empire => Enum.TryParse<EmpireRankEnum>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.Federation => Enum.TryParse<FederationRankEnum>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.CQC => Enum.TryParse<CQCRankEnum>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            _ => unknowRank
        };
    }
}