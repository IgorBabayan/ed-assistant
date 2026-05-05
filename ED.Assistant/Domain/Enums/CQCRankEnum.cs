using System.ComponentModel.DataAnnotations;

namespace ED.Assistant.Domain.Enums;

public enum CQCRankEnum
{
	[Display(Name = "Winner")] Winner = 0,
	[Display(Name = "MVP")] MVP,
	[Display(Name = "Survivor")] Survivor,
	[Display(Name = "Assistant")] Assistant,
	[Display(Name = "Finisher")] Finisher,
	[Display(Name = "Bouncing Back")] BouncingBack,
	[Display(Name = "Good Effort")] GoodEffort,
	[Display(Name = "Payback")] Payback,
	[Display(Name = "Multi Kill")] MultiKill,
	[Display(Name = "Quick Kill")] QuickKill,
	[Display(Name = "Try Harder")] TryHarder
}
