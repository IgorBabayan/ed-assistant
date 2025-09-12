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
            RankEnum.Combat => Enum.TryParse<CombatRank>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.Trade => Enum.TryParse<TradeRank>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.Explore => Enum.TryParse<ExplorationRank>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.Soldier => Enum.TryParse<SoldierRank>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.Exobiologist => Enum.TryParse<ExobiologistRank>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.Empire => Enum.TryParse<EmpireRank>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.Federation => Enum.TryParse<FederationRank>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            RankEnum.CQC => Enum.TryParse<CQCRank>(value.ToString(), out var result)
                ? result.GetLocalizedDisplayName()
                : unknowRank,

            _ => unknowRank
        };
    }
}