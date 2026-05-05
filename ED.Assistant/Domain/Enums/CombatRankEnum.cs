using System.ComponentModel.DataAnnotations;

namespace ED.Assistant.Domain.Enums;

public enum CombatRankEnum
{
	[Display(Name = "Harmless")] Harmless,
	[Display(Name = "Mostly Harmless")] MostlyHarmless,
	[Display(Name = "Novice")] Novice,
	[Display(Name = "Competent")] Competent,
	[Display(Name = "Expert")] Expert,
	[Display(Name = "Master")] Master,
	[Display(Name = "Dangerous")] Dangerous,
	[Display(Name = "Deadly")] Deadly,
	[Display(Name = "Elite I")] EliteI,
	[Display(Name = "Elite II")] EliteII,
	[Display(Name = "Elite III")] EliteIII,
	[Display(Name = "Elite IV")] EliteIV,
	[Display(Name = "Elite V")] EliteV
}
