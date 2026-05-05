using System.ComponentModel.DataAnnotations;

namespace ED.Assistant.Domain.Enums;

public enum TradeRankEnum
{
	[Display(Name = "Penniless")] Penniless = 0,
	[Display(Name = "Mostly Penniless")] MostlyPenniless,
	[Display(Name = "Peddler")] Peddler,
	[Display(Name = "Dealer")] Dealer,
	[Display(Name = "Merchant")] Merchant,
	[Display(Name = "Broker")] Broker,
	[Display(Name = "Entrepreneur")] Entrepreneur,
	[Display(Name = "Tycoon")] Tycoon,
	[Display(Name = "Elite I")] EliteI,
	[Display(Name = "Elite II")] EliteII,
	[Display(Name = "Elite III")] EliteIII,
	[Display(Name = "Elite IV")] EliteIV,
	[Display(Name = "Elite V")] EliteV
}
