namespace EdAssistant.Models.Journal;

public sealed class StatisticsEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }

    public BankAccountStats? Bank_Account { get; set; }
    public CombatStats? Combat { get; set; }
    public CrimeStats? Crime { get; set; }
    public SmugglingStats? Smuggling { get; set; }
    public TradingStats? Trading { get; set; }
    public MiningStats? Mining { get; set; }
    public ExplorationStats? Exploration { get; set; }
    public PassengersStats? Passengers { get; set; }
    public SearchRescueStats? Search_And_Rescue { get; set; }
    public SquadronStats? Squadron { get; set; }
    public ThargoidEncounterStats? TG_ENCOUNTERS { get; set; }
    public CraftingStats? Crafting { get; set; }
    public CrewStats? Crew { get; set; }
    public MulticrewStats? Multicrew { get; set; }
    public MaterialTraderStats? Material_Trader_Stats { get; set; }
    public FleetCarrierStats? FLEETCARRIER { get; set; }
    public ExobiologyStats? Exobiology { get; set; }

    public override JournalEventType EventType => JournalEventType.Statistics;

    #region Nested Stat Blocks

    public sealed class BankAccountStats
    {
        public long Current_Wealth { get; set; }
        public long Spent_On_Ships { get; set; }
        public long Spent_On_Outfitting { get; set; }
        public long Spent_On_Repairs { get; set; }
        public long Spent_On_Fuel { get; set; }
        public long Spent_On_Ammo_Consumables { get; set; }
        public int Insurance_Claims { get; set; }
        public long Spent_On_Insurance { get; set; }
        public int Owned_Ship_Count { get; set; }
        public long Spent_On_Suits { get; set; }
        public long Spent_On_Weapons { get; set; }
        public long Spent_On_Suit_Consumables { get; set; }
        public int Suits_Owned { get; set; }
        public int Weapons_Owned { get; set; }
        public long Spent_On_Premium_Stock { get; set; }
        public int Premium_Stock_Bought { get; set; }
    }

    public sealed class CombatStats
    {
        public int Bounties_Claimed { get; set; }
        public long Bounty_Hunting_Profit { get; set; }
        public int Combat_Bonds { get; set; }
        public long Combat_Bond_Profits { get; set; }
        public int Assassinations { get; set; }
        public long Assassination_Profits { get; set; }
        public long Highest_Single_Reward { get; set; }
        public int Skimmers_Killed { get; set; }
        public int OnFoot_Combat_Bonds { get; set; }
        public long OnFoot_Combat_Bonds_Profits { get; set; }
        public int OnFoot_Vehicles_Destroyed { get; set; }
        public int OnFoot_Ships_Destroyed { get; set; }
        public int Dropships_Taken { get; set; }
        public int Dropships_Booked { get; set; }
        public int Dropships_Cancelled { get; set; }
        public int ConflictZone_High { get; set; }
        public int ConflictZone_Medium { get; set; }
        public int ConflictZone_Low { get; set; }
        public int ConflictZone_Total { get; set; }
        public int ConflictZone_High_Wins { get; set; }
        public int ConflictZone_Medium_Wins { get; set; }
        public int ConflictZone_Low_Wins { get; set; }
        public int ConflictZone_Total_Wins { get; set; }
        public int Settlement_Defended { get; set; }
        public int Settlement_Conquered { get; set; }
        public int OnFoot_Skimmers_Killed { get; set; }
        public int OnFoot_Scavs_Killed { get; set; }
    }

    public sealed class CrimeStats
    {
        public int Notoriety { get; set; }
        public int Fines { get; set; }
        public long Total_Fines { get; set; }
        public int Bounties_Received { get; set; }
        public long Total_Bounties { get; set; }
        public long Highest_Bounty { get; set; }
        public int Malware_Uploaded { get; set; }
        public int Settlements_State_Shutdown { get; set; }
        public int Production_Sabotage { get; set; }
        public int Production_Theft { get; set; }
        public int Total_Murders { get; set; }
        public int Citizens_Murdered { get; set; }
        public int Omnipol_Murdered { get; set; }
        public int Guards_Murdered { get; set; }
        public int Data_Stolen { get; set; }
        public int Goods_Stolen { get; set; }
        public int Sample_Stolen { get; set; }
        public int Total_Stolen { get; set; }
        public int Turrets_Destroyed { get; set; }
        public int Turrets_Overloaded { get; set; }
        public int Turrets_Total { get; set; }
        public long Value_Stolen_StateChange { get; set; }
        public int Profiles_Cloned { get; set; }
    }

    public sealed class SmugglingStats
    {
        public int Black_Markets_Traded_With { get; set; }
        public long Black_Markets_Profits { get; set; }
        public int Resources_Smuggled { get; set; }
        public long Average_Profit { get; set; }
        public long Highest_Single_Transaction { get; set; }
    }

    public sealed class TradingStats
    {
        public int Markets_Traded_With { get; set; }
        public long Market_Profits { get; set; }
        public int Resources_Traded { get; set; }
        public double Average_Profit { get; set; }
        public long Highest_Single_Transaction { get; set; }
        public int Data_Sold { get; set; }
        public int Goods_Sold { get; set; }
        public int Assets_Sold { get; set; }
    }

    public sealed class MiningStats
    {
        public long Mining_Profits { get; set; }
        public int Quantity_Mined { get; set; }
        public int Materials_Collected { get; set; }
    }

    public sealed class ExplorationStats
    {
        public int Systems_Visited { get; set; }
        public long Exploration_Profits { get; set; }
        public int Planets_Scanned_To_Level_2 { get; set; }
        public int Planets_Scanned_To_Level_3 { get; set; }
        public int Efficient_Scans { get; set; }
        public long Highest_Payout { get; set; }
        public long Total_Hyperspace_Distance { get; set; }
        public int Total_Hyperspace_Jumps { get; set; }
        public double Greatest_Distance_From_Start { get; set; }
        public long Time_Played { get; set; }
        public long OnFoot_Distance_Travelled { get; set; }
        public int Shuttle_Journeys { get; set; }
        public double Shuttle_Distance_Travelled { get; set; }
        public long Spent_On_Shuttles { get; set; }
        public int First_Footfalls { get; set; }
        public int Planet_Footfalls { get; set; }
        public int Settlements_Visited { get; set; }
    }

    public sealed class PassengersStats
    {
        public int Passengers_Missions_Accepted { get; set; }
        public int Passengers_Missions_Bulk { get; set; }
        public int Passengers_Missions_VIP { get; set; }
        public int Passengers_Missions_Delivered { get; set; }
        public int Passengers_Missions_Ejected { get; set; }
    }

    public sealed class SearchRescueStats
    {
        public int SearchRescue_Traded { get; set; }
        public long SearchRescue_Profit { get; set; }
        public int SearchRescue_Count { get; set; }
        public long Salvage_Legal_POI { get; set; }
        public long Salvage_Legal_Settlements { get; set; }
        public long Salvage_Illegal_POI { get; set; }
        public long Salvage_Illegal_Settlements { get; set; }
        public int Maglocks_Opened { get; set; }
        public int Panels_Opened { get; set; }
        public int Settlements_State_FireOut { get; set; }
        public int Settlements_State_Reboot { get; set; }
    }

    public sealed class SquadronStats
    {
        public long Squadron_Bank_Credits_Deposited { get; set; }
        public long Squadron_Bank_Credits_Withdrawn { get; set; }
        public int Squadron_Bank_Commodities_Deposited_Num { get; set; }
        public long Squadron_Bank_Commodities_Deposited_Value { get; set; }
        public int Squadron_Bank_Commodities_Withdrawn_Num { get; set; }
        public long Squadron_Bank_Commodities_Withdrawn_Value { get; set; }
        public int Squadron_Bank_PersonalAssets_Deposited_Num { get; set; }
        public long Squadron_Bank_PersonalAssets_Deposited_Value { get; set; }
        public int Squadron_Bank_PersonalAssets_Withdrawn_Num { get; set; }
        public long Squadron_Bank_PersonalAssets_Withdrawn_Value { get; set; }
        public int Squadron_Bank_Ships_Deposited_Num { get; set; }
        public long Squadron_Bank_Ships_Deposited_Value { get; set; }
        public int Squadron_Leaderboard_aegis_highestcontribution { get; set; }
        public int Squadron_Leaderboard_bgs_highestcontribution { get; set; }
        public int Squadron_Leaderboard_bounty_highestcontribution { get; set; }
        public int Squadron_Leaderboard_colonisation_contribution_highestcontribution { get; set; }
        public int Squadron_Leaderboard_combat_highestcontribution { get; set; }
        public int Squadron_Leaderboard_cqc_highestcontribution { get; set; }
        public int Squadron_Leaderboard_exploration_highestcontribution { get; set; }
        public int Squadron_Leaderboard_mining_highestcontribution { get; set; }
        public int Squadron_Leaderboard_powerplay_highestcontribution { get; set; }
        public int Squadron_Leaderboard_trade_highestcontribution { get; set; }
        public int Squadron_Leaderboard_trade_illicit_highestcontribution { get; set; }
        public int Squadron_Leaderboard_podiums { get; set; }
    }

    public sealed class ThargoidEncounterStats
    {
        public int TG_ENCOUNTER_KILLED { get; set; }
        public int TG_ENCOUNTER_TOTAL { get; set; }
        public string? TG_ENCOUNTER_TOTAL_LAST_SYSTEM { get; set; }
        public string? TG_ENCOUNTER_TOTAL_LAST_TIMESTAMP { get; set; }
        public string? TG_ENCOUNTER_TOTAL_LAST_SHIP { get; set; }
    }

    public sealed class CraftingStats
    {
        public int Count_Of_Used_Engineers { get; set; }
        public int Recipes_Generated { get; set; }
        public int Recipes_Generated_Rank_1 { get; set; }
        public int Recipes_Generated_Rank_2 { get; set; }
        public int Recipes_Generated_Rank_3 { get; set; }
        public int Recipes_Generated_Rank_4 { get; set; }
        public int Recipes_Generated_Rank_5 { get; set; }
        public int Suit_Mods_Applied { get; set; }
        public int Weapon_Mods_Applied { get; set; }
        public int Suits_Upgraded { get; set; }
        public int Weapons_Upgraded { get; set; }
        public int Suits_Upgraded_Full { get; set; }
        public int Weapons_Upgraded_Full { get; set; }
        public int Suit_Mods_Applied_Full { get; set; }
        public int Weapon_Mods_Applied_Full { get; set; }
    }

    public sealed class CrewStats
    {
        public long NpcCrew_TotalWages { get; set; }
        public int NpcCrew_Hired { get; set; }
        public int NpcCrew_Fired { get; set; }
        public int NpcCrew_Died { get; set; }
    }

    public sealed class MulticrewStats
    {
        public long Multicrew_Time_Total { get; set; }
        public long Multicrew_Gunner_Time_Total { get; set; }
        public long Multicrew_Fighter_Time_Total { get; set; }
        public long Multicrew_Credits_Total { get; set; }
        public long Multicrew_Fines_Total { get; set; }
    }

    public sealed class MaterialTraderStats
    {
        public int Trades_Completed { get; set; }
        public int Materials_Traded { get; set; }
        public int Encoded_Materials_Traded { get; set; }
        public int Raw_Materials_Traded { get; set; }
        public int Grade_1_Materials_Traded { get; set; }
        public int Grade_2_Materials_Traded { get; set; }
        public int Grade_3_Materials_Traded { get; set; }
        public int Grade_4_Materials_Traded { get; set; }
        public int Grade_5_Materials_Traded { get; set; }
        public int Assets_Traded_In { get; set; }
        public int Assets_Traded_Out { get; set; }
    }

    public sealed class FleetCarrierStats
    {
        public int FLEETCARRIER_EXPORT_TOTAL { get; set; }
        public int FLEETCARRIER_IMPORT_TOTAL { get; set; }
        public long FLEETCARRIER_TRADEPROFIT_TOTAL { get; set; }
        public long FLEETCARRIER_TRADESPEND_TOTAL { get; set; }
        public long FLEETCARRIER_STOLENPROFIT_TOTAL { get; set; }
        public long FLEETCARRIER_STOLENSPEND_TOTAL { get; set; }
        public double FLEETCARRIER_DISTANCE_TRAVELLED { get; set; }
        public int FLEETCARRIER_TOTAL_JUMPS { get; set; }
        public int FLEETCARRIER_SHIPYARD_SOLD { get; set; }
        public long FLEETCARRIER_SHIPYARD_PROFIT { get; set; }
        public int FLEETCARRIER_OUTFITTING_SOLD { get; set; }
        public long FLEETCARRIER_OUTFITTING_PROFIT { get; set; }
        public int FLEETCARRIER_REARM_TOTAL { get; set; }
        public int FLEETCARRIER_REFUEL_TOTAL { get; set; }
        public long FLEETCARRIER_REFUEL_PROFIT { get; set; }
        public int FLEETCARRIER_REPAIRS_TOTAL { get; set; }
        public int FLEETCARRIER_VOUCHERS_REDEEMED { get; set; }
        public long FLEETCARRIER_VOUCHERS_PROFIT { get; set; }
    }

    public sealed class ExobiologyStats
    {
        public int Organic_Genus_Encountered { get; set; }
        public int Organic_Species_Encountered { get; set; }
        public int Organic_Variant_Encountered { get; set; }
        public long Organic_Data_Profits { get; set; }
        public int Organic_Data { get; set; }
        public long First_Logged_Profits { get; set; }
        public int First_Logged { get; set; }
        public int Organic_Systems { get; set; }
        public int Organic_Planets { get; set; }
        public int Organic_Genus { get; set; }
        public int Organic_Species { get; set; }
    }

    #endregion
}