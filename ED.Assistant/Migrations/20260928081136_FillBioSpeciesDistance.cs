using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED.Assistant.Migrations
{
    /// <inheritdoc />
    public partial class FillBioSpeciesDistance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Rule");

            migrationBuilder.AddColumn<double>(
                name: "Distance",
                table: "Genus",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1001,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1002,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1003,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1004,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1005,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1101,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1102,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1103,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1104,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1105,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1106,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1107,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1108,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1201,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1202,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1203,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1204,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1205,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1206,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1207,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1208,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1209,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1210,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1211,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1212,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1213,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1301,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1302,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1303,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1304,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1305,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1306,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1307,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1308,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1401,
                column: "Distance",
                value: 300.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1402,
                column: "Distance",
                value: 300.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1403,
                column: "Distance",
                value: 300.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1404,
                column: "Distance",
                value: 300.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1405,
                column: "Distance",
                value: 300.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1501,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1502,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1503,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1601,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1602,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1603,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1604,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1701,
                column: "Distance",
                value: 1000.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1702,
                column: "Distance",
                value: 1000.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1801,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1802,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1803,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1804,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1805,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1806,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1901,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1902,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1903,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1904,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1905,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1906,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1907,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2001,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2002,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2003,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2004,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2101,
                column: "Distance",
                value: 300.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2102,
                column: "Distance",
                value: 300.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2103,
                column: "Distance",
                value: 300.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2104,
                column: "Distance",
                value: 300.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2201,
                column: "Distance",
                value: 800.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2202,
                column: "Distance",
                value: 800.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2203,
                column: "Distance",
                value: 800.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2204,
                column: "Distance",
                value: 800.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2205,
                column: "Distance",
                value: 800.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2206,
                column: "Distance",
                value: 800.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2301,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2302,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2303,
                column: "Distance",
                value: 150.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2401,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2501,
                column: "Distance",
                value: 0.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2502,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2503,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2504,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2505,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2506,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2507,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2508,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2509,
                column: "Distance",
                value: 500.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2601,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2602,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2603,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2604,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2605,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2606,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2607,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2608,
                column: "Distance",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2701,
                column: "Distance",
                value: 800.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2702,
                column: "Distance",
                value: 800.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2703,
                column: "Distance",
                value: 800.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2704,
                column: "Distance",
                value: 800.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2705,
                column: "Distance",
                value: 800.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2801,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2802,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2803,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2804,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2805,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2806,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2807,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2808,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2809,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2810,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2811,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2812,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2813,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2814,
                column: "Distance",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2815,
                column: "Distance",
                value: 200.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Genus");

            migrationBuilder.AddColumn<double>(
                name: "Distance",
                table: "Rule",
                type: "REAL",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1000,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1001,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1002,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1003,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1004,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1100,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1101,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1102,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1103,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1104,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1105,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1106,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1107,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1108,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1200,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1201,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1202,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1203,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1204,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1205,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1206,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1207,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1208,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1209,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1210,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1211,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1212,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1213,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1214,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1215,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1216,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1217,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1218,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1219,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1220,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1221,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1222,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1223,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1224,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1225,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1226,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1227,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1228,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1229,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1230,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1231,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1232,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1233,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1234,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1235,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1236,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1237,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1238,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1239,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1240,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1241,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1242,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1243,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1244,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1245,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1246,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1247,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1248,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1249,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1250,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1251,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1252,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1253,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1254,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1255,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1300,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1301,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1302,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1303,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1304,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1305,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1306,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1307,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1400,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1401,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1402,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1403,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1404,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1405,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1406,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1500,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1501,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1502,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1503,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1504,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1505,
                column: "Distance",
                value: 2000.0);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1506,
                column: "Distance",
                value: 2000.0);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1507,
                column: "Distance",
                value: 2000.0);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1600,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1601,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1602,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1603,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1604,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1605,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1606,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1607,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1700,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1701,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1702,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1703,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1800,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1801,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1802,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1803,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1804,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1805,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1900,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1901,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1902,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1903,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1904,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1905,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1906,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1907,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1908,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1909,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1910,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1911,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2000,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2001,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2002,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2003,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2004,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2005,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2006,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2007,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2008,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2009,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2010,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2011,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2012,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2013,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2014,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2015,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2016,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2017,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2018,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2019,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2020,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2021,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2022,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2023,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2024,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2025,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2026,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2100,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2101,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2102,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2103,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2104,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2105,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2106,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2107,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2108,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2109,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2110,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2111,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2112,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2113,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2114,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2115,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2116,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2200,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2201,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2202,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2203,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2204,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2205,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2206,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2207,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2208,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2209,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2210,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2211,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2212,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2213,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2300,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2301,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2302,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2303,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2304,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2305,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2306,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2307,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2308,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2309,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2310,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2400,
                column: "Distance",
                value: 12000.0);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2500,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2501,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2502,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2503,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2504,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2505,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2506,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2507,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2508,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2509,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2510,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2511,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2512,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2513,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2514,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2515,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2516,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2517,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2518,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2519,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2520,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2521,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2522,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2523,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2524,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2525,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2600,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2601,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2602,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2603,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2604,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2605,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2606,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2607,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2608,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2609,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2610,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2611,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2700,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2701,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2702,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2703,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2704,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2705,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2800,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2801,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2802,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2803,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2804,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2805,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2806,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2807,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2808,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2809,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2810,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2811,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2812,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2813,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2814,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2815,
                column: "Distance",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2816,
                column: "Distance",
                value: null);
        }
    }
}
