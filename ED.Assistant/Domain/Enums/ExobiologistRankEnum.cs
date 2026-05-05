using System.ComponentModel.DataAnnotations;

namespace ED.Assistant.Domain.Enums;

public enum ExobiologistRankEnum
{
	[Display(Name = "Directionless")] Directionless = 0,
	[Display(Name = "Mostly Directionless")] MostlyDirectionless,
	[Display(Name = "Compiler")] Compiler,
	[Display(Name = "Collector")] Collector,
	[Display(Name = "Cataloguer")] Cataloguer,
	[Display(Name = "Taxonomist")] Taxonomist,
	[Display(Name = "Ecologist")] Ecologist,
	[Display(Name = "Geneticist")] Geneticist,
	[Display(Name = "Elite I")] EliteI,
	[Display(Name = "Elite II")] EliteII,
	[Display(Name = "Elite III")] EliteIII,
	[Display(Name = "Elite IV")] EliteIV,
	[Display(Name = "Elite V")] EliteV
}
