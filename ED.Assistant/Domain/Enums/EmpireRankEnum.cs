using System.ComponentModel.DataAnnotations;

namespace ED.Assistant.Domain.Enums;

public enum EmpireRankEnum
{
	[Display(Name = "None")] None = 0,
	[Display(Name = "Outsider")] Outsider,
	[Display(Name = "Serf")] Serf,
	[Display(Name = "Master")] Master,
	[Display(Name = "Squire")] Squire,
	[Display(Name = "Knight")] Knight,
	[Display(Name = "Lord")] Lord,
	[Display(Name = "Baron")] Baron,
	[Display(Name = "Viscount")] Viscount,
	[Display(Name = "Count")] Count,
	[Display(Name = "Earl")] Earl,
	[Display(Name = "Marquis")] Marquis,
	[Display(Name = "Duke")] Duke,
	[Display(Name = "Prince")] Prince,
	[Display(Name = "King")] King
}
