using System.ComponentModel.DataAnnotations;

namespace ED.Assistant.Data.Models.Enums;

public enum FederationRankEnum
{
	[Display(Name = "None")] None = 0,
	[Display(Name = "Recruit")] Recruit,
	[Display(Name = "Cadet")] Cadet,
	[Display(Name = "Midshipman")] Midshipman,
	[Display(Name = "Petty Officer")] PettyOfficer,
	[Display(Name = "Chief Petty Officer")] ChiefPettyOfficer,
	[Display(Name = "Warrant Officer")] WarrantOfficer,
	[Display(Name = "Ensign")] Ensign,
	[Display(Name = "Lieutenant")] Lieutenant,
	[Display(Name = "Lieutenant Commander")] LieutenantCommander,
	[Display(Name = "Post Commander")] PostCommander,
	[Display(Name = "Post Captain")] PostCaptain,
	[Display(Name = "Rear Admiral")] RearAdmiral,
	[Display(Name = "Vice Admiral")] ViceAdmiral,
	[Display(Name = "Admiral")] Admiral
}
