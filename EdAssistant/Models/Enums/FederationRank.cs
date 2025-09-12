namespace EdAssistant.Models.Enums;

public enum FederationRank
{
    [Display(Name = "Rank.Federation.None")]
    None = 0,

    [Display(Name = "Rank.Federation.Recruit")]
    Recruit,

    [Display(Name = "Rank.Federation.Cadet")]
    Cadet,

    [Display(Name = "Rank.Federation.Midshipman")]
    Midshipman,

    [Display(Name = "Rank.Federation.PettyOfficer")]
    PettyOfficer,

    [Display(Name = "Rank.Federation.ChiefPettyOfficer")]
    ChiefPettyOfficer,

    [Display(Name = "Rank.CQC.WarrantOfficer")]
    WarrantOfficer,

    [Display(Name = "Rank.Federation.Ensign")]
    Ensign,

    [Display(Name = "Rank.Federation.Lieutenant")]
    Lieutenant,

    [Display(Name = "Rank.Federation.LtCommander")]
    LtCommander,

    [Display(Name = "Rank.Federation.PostCommander")]
    PostCommander,

    [Display(Name = "Rank.Federation.PostCaptain")]
    PostCaptain,

    [Display(Name = "Rank.Federation.RearAdmiral")]
    RearAdmiral,

    [Display(Name = "Rank.Federation.ViceAdmiral")]
    ViceAdmiral,

    [Display(Name = "Rank.Federation.Admiral")]
    Admiral
}