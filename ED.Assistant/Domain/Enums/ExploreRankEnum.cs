using System.ComponentModel.DataAnnotations;

namespace ED.Assistant.Domain.Enums;

public enum ExploreRankEnum
{
	[Display(Name = "Aimless")] Aimless = 0,
	[Display(Name = "Mostly Aimless")] MostlyAimless,
	[Display(Name = "Scout")] Scout,
	[Display(Name = "Surveyor")] Surveyor,
	[Display(Name = "Trailblazer")] Trailblazer,
	[Display(Name = "Pathfinder")] Pathfinder,
	[Display(Name = "Ranger")] Ranger,
	[Display(Name = "Pioneer")] Pioneer,
	[Display(Name = "Elite I")] EliteI,
	[Display(Name = "Elite II")] EliteII,
	[Display(Name = "Elite III")] EliteIII,
	[Display(Name = "Elite IV")] EliteIV,
	[Display(Name = "Elite V")] EliteV
}
