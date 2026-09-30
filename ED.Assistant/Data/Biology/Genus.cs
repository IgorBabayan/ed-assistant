// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
namespace ED.Assistant.Data.Biology;

internal enum AleoidaGenus
{
    Arcus = 1001,
    Coronamus = 1002,
    Spica = 1003,
    Laminiae = 1004,
    Gravis = 1005
}

internal enum AnemoneGenus
{
    Luteolum = 1101,
    Croceum = 1102,
    Puniceum = 1103,
    Roseum = 1104,
    RubeumBioluminescent = 1105,
    PrasinumBioluminescent = 1106,
    RoseumBioluminescent = 1107,
    BlatteumBioluminescent = 1108
}

internal enum BacteriumGenus
{
    Aurasus = 1201,
    Nebulus = 1202,
    Scopulum = 1203,
    Acies = 1204,
    Vesicula = 1205,
    Alcyoneum = 1206,
    Tela = 1207,
    Informem = 1208,
    Volu = 1209,
    Bullaris = 1210,
    Omentum = 1211,
    Cerbrus = 1212,
    Verrata = 1213
}

internal enum BrainTreeGenus
{
    Roseum = 1301,
    Gypseeum = 1302,
    Ostrinum = 1303,
    Viride = 1304,
    Aureum = 1305,
    Puniceum = 1306,
    Lindigoticum = 1307,
    Lividum = 1308
}

internal enum CactoidaGenus
{
    Cortexum = 1401,
    Lapis = 1402,
    Vermis = 1403,
    Pullulanta = 1404,
    Peperatis = 1405
}

internal enum ClypeusGenus
{
    Lacrimam = 1501,
    Margaritus = 1502,
    Speculumi = 1503
}

internal enum ConchaGenus
{
    Renibus = 1601,
    Aureolas = 1602,
    Labiata = 1603,
    Biconcavis = 1604
}

internal enum ElectricaeGenus
{
    Pluma = 1701,
    Radialem = 1702
}

internal enum FrutexaGenus
{
    Flabellum = 1901,
    Acus = 1902,
    Metallicum = 1903,
    Flammasis = 1904,
    Fera = 1905,
    Sponsae = 1906,
    Collum = 1907
}

internal enum FumerolaGenus
{
    Carbosis = 2001,
    Extremus = 2002,
    Nitris = 2003,
    Aquatis = 2004
}

internal enum FungoidaGenus
{
    Setisis = 2101,
    Stabitis = 2102,
    Bullarum = 2103,
    Gelata = 2104
}

internal enum FonticuluaGenus
{
    Segmentatus = 1801,
    Campestris = 1802,
    Upupam = 1803,
    Lapida = 1804,
    Fluctus = 1805,
    Digitos = 1806
}

internal enum OsseusGenus
{
    Fractus = 2201,
    Discus = 2202,
    Spiralis = 2203,
    Pumice = 2204,
    Cornibus = 2205,
    Pellebantus = 2206
}

internal enum ReceptaGenus
{
    Umbrux = 2301,
    Deltahedronix = 2302,
    Conditivus = 2303
}

internal enum ShardGenus
{
    CrystallineShards = 2401
}

internal enum StratumGenus
{
    Excutitus = 2502,
    Paleas = 2503,
    Laminamus = 2504,
    Araneamus = 2505,
    Limaxus = 2506,
    Cucumisis = 2507,
    Tectonicas = 2508,
    Frigus = 2509
}

internal enum TubersGenus
{
    Roseum = 2601,
    Prasinum = 2602,
    Albidum = 2603,
    Caeruleum = 2604,
    Lindigoticum = 2605,
    Violaceum = 2606,
    Viride = 2607,
    Blatteum = 2608
}

internal enum TubusGenus
{
    Conifer = 2701,
    Sororibus = 2702,
    Cavas = 2703,
    Rosarium = 2704,
    Compagibus = 2705
}

internal enum TussockGenus
{
    Pennata = 2801,
    Ventusa = 2802,
    Ignis = 2803,
    Cultro = 2804,
    Catena = 2805,
    Pennatis = 2806,
    Serrati = 2807,
    Albata = 2808,
    Propagito = 2809,
    Divisa = 2810,
    Caputus = 2811,
    Triticum = 2812,
    Stigmasis = 2813,
    Virgam = 2814,
    Capillum = 2815
}

public sealed class Genus
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Value { get; set; }
    public required string CodexType { get; set; }
    public required string CodexName { get; set; }
    public double Distance { get; set; }
    
    public ICollection<Rule> Rules { get; set; } = [];
    public ICollection<Evaluator.Evaluator> Evaluators { get; set; } = [];
}