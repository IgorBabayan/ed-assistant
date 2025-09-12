namespace EdAssistant.Models.Enums;

public enum CombatRank
{
    [Display(Name = "Rank.Combat.Harmless")]
    Harmless = 0,

    [Display(Name = "Rank.Combat.MostlyHarmless")]
    MostlyHarmless,

    [Display(Name = "Rank.Combat.Novice")]
    Novice,

    [Display(Name = "Rank.Combat.Competent")]
    Competent,

    [Display(Name = "Rank.Combat.Expert")]
    Expert,

    [Display(Name = "Rank.Combat.Master")]
    Master,

    [Display(Name = "Rank.Combat.Dangerous")]
    Dangerous,

    [Display(Name = "Rank.Combat.Deadly")]
    Deadly,

    [Display(Name = "Rank.Combat.Elite")]
    Elite,

    [Display(Name = "Rank.Combat.EliteI")]
    EliteI,

    [Display(Name = "Rank.Combat.EliteII")]
    EliteII,

    [Display(Name = "Rank.Combat.EliteIII")]
    EliteIII,

    [Display(Name = "Rank.Combat.EliteIV")]
    EliteIV,

    [Display(Name = "Rank.Combat.Gunslinger")]
    EliteV
}