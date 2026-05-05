using System.ComponentModel.DataAnnotations;

namespace ED.Assistant.Domain.Enums;

public enum SoldierRankEnum
{
	[Display(Name = "Defenceless")] Defenceless = 0,
	[Display(Name = "Mostly Defenceless")] MostlyDefenceless,
	[Display(Name = "Rookie")] Rookie,
	[Display(Name = "Soldier")] Soldier,
	[Display(Name = "Gunslinger")] Gunslinger,
	[Display(Name = "Warrior")] Warrior,
	[Display(Name = "Gladiator")] Gladiator,
	[Display(Name = "Deadeye")] Deadeye,
	[Display(Name = "Elite I")] EliteI,
	[Display(Name = "Elite II")] EliteII,
	[Display(Name = "Elite III")] EliteIII,
	[Display(Name = "Elite IV")] EliteIV,
	[Display(Name = "Elite V")] EliteV
}
