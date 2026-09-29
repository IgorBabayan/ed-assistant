using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ED.Assistant.Migrations
{
    /// <inheritdoc />
    public partial class EvaluatorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rule_ParentBodyClass_ParentBodyClassId",
                table: "Rule");

            migrationBuilder.DropIndex(
                name: "IX_Rule_ParentBodyClassId",
                table: "Rule");

            migrationBuilder.DeleteData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 2501);

            migrationBuilder.DropColumn(
                name: "ParentBodyClassId",
                table: "Rule");

            migrationBuilder.CreateTable(
                name: "Evaluator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HasFirstFootStep = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false),
                    GenusId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evaluator_Genus_GenusId",
                        column: x => x.GenusId,
                        principalTable: "Genus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RuleParentBodyClass",
                columns: table => new
                {
                    RuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentBodyClassId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleParentBodyClass", x => new { x.RuleId, x.ParentBodyClassId });
                    table.ForeignKey(
                        name: "FK_RuleParentBodyClass_ParentBodyClass_ParentBodyClassId",
                        column: x => x.ParentBodyClassId,
                        principalTable: "ParentBodyClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RuleParentBodyClass_Rule_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1604,
                column: "Value",
                value: 19010800m);

            // ---- StarClass renumbering (hand-ordered for SQLite) ----
            // SQLite checks FKs and unique indexes per statement, so:
            // 1) move every StarClass name out of the way (unique index on Name),
            // 2) insert/rename StarClass rows so every target Id exists,
            // 3) repoint RuleStar rows,
            // 4) only then delete StarClass rows nothing references any more.
            migrationBuilder.Sql("UPDATE \"StarClass\" SET \"Name\" = '~' || \"Id\";");

            migrationBuilder.InsertData(
                table: "StarClass",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 12, "S" },
                    { 13, "M" }
                });

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "A");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "B");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "O");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "N");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "D");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "H");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "AeBe");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "F");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "G");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "K");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "MS");

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110001,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110002,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110101,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110102,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110103,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110201,
                column: "StarClassId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110301,
                column: "StarClassId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110401,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110402,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110403,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110404,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110501,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110502,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110503,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110504,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110505,
                column: "StarClassId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110601,
                column: "StarClassId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110701,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110702,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110703,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110801,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110802,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170001,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170002,
                column: "StarClassId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170003,
                column: "StarClassId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170004,
                column: "StarClassId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170005,
                column: "StarClassId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170101,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170102,
                column: "StarClassId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170103,
                column: "StarClassId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170104,
                column: "StarClassId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170105,
                column: "StarClassId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 240001,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 240002,
                column: "StarClassId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 240003,
                column: "StarClassId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 240004,
                column: "StarClassId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 240005,
                column: "StarClassId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 240006,
                column: "StarClassId",
                value: 12);

            migrationBuilder.DeleteData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 0);

            // ---- end StarClass renumbering ----

            migrationBuilder.CreateIndex(
                name: "IX_Evaluator_GenusId",
                table: "Evaluator",
                column: "GenusId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleParentBodyClass_ParentBodyClassId",
                table: "RuleParentBodyClass",
                column: "ParentBodyClassId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evaluator");

            migrationBuilder.DropTable(
                name: "RuleParentBodyClass");

            migrationBuilder.AddColumn<int>(
                name: "ParentBodyClassId",
                table: "Rule",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Genus",
                keyColumn: "Id",
                keyValue: 1604,
                column: "Value",
                value: 16777215m);

            migrationBuilder.InsertData(
                table: "Genus",
                columns: new[] { "Id", "CodexName", "CodexType", "Distance", "Name", "Value" },
                values: new object[] { 2501, "$Codex_Ent_Stratum_04_Name;", "$Codex_Ent_Stratum_04_Name;", 0.0, "Stratum Aranaemus", 2448900m });

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1000,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1001,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1002,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1003,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1004,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1100,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1101,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1102,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1103,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1104,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1105,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1106,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1107,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1108,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1200,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1201,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1202,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1203,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1204,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1205,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1206,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1207,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1208,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1209,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1210,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1211,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1212,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1213,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1214,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1215,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1216,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1217,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1218,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1219,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1220,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1221,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1222,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1223,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1224,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1225,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1226,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1227,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1228,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1229,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1230,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1231,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1232,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1233,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1234,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1235,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1236,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1237,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1238,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1239,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1240,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1241,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1242,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1243,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1244,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1245,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1246,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1247,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1248,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1249,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1250,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1251,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1252,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1253,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1254,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1255,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1300,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1301,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1302,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1303,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1304,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1305,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1306,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1307,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1400,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1401,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1402,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1403,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1404,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1405,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1406,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1500,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1501,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1502,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1503,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1504,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1505,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1506,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1507,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1600,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1601,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1602,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1603,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1604,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1605,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1606,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1607,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1700,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1701,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1702,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1703,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1800,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1801,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1802,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1803,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1804,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1805,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1900,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1901,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1902,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1903,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1904,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1905,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1906,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1907,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1908,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1909,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1910,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 1911,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2000,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2001,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2002,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2003,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2004,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2005,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2006,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2007,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2008,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2009,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2010,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2011,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2012,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2013,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2014,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2015,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2016,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2017,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2018,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2019,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2020,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2021,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2022,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2023,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2024,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2025,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2026,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2100,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2101,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2102,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2103,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2104,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2105,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2106,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2107,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2108,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2109,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2110,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2111,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2112,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2113,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2114,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2115,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2116,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2200,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2201,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2202,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2203,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2204,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2205,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2206,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2207,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2208,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2209,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2210,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2211,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2212,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2213,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2300,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2301,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2302,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2303,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2304,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2305,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2306,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2307,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2308,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2309,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2310,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2400,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2500,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2501,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2502,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2503,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2504,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2505,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2506,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2507,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2508,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2509,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2510,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2511,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2512,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2513,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2514,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2515,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2516,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2517,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2518,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2519,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2520,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2521,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2522,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2523,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2524,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2525,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2600,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2601,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2602,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2603,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2604,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2605,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2606,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2607,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2608,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2609,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2610,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2611,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2700,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2701,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2702,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2703,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2704,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2705,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2800,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2801,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2802,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2803,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2804,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2805,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2806,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2807,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2808,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2809,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2810,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2811,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2812,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2813,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2814,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2815,
                column: "ParentBodyClassId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rule",
                keyColumn: "Id",
                keyValue: 2816,
                column: "ParentBodyClassId",
                value: null);

            // ---- StarClass renumbering (hand-ordered for SQLite) ----
            // SQLite checks FKs and unique indexes per statement, so:
            // 1) move every StarClass name out of the way (unique index on Name),
            // 2) insert/rename StarClass rows so every target Id exists,
            // 3) repoint RuleStar rows,
            // 4) only then delete StarClass rows nothing references any more.
            migrationBuilder.Sql("UPDATE \"StarClass\" SET \"Name\" = '~' || \"Id\";");

            migrationBuilder.InsertData(
                table: "StarClass",
                columns: new[] { "Id", "Name" },
                values: new object[] { 0, "A" });

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "B");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "O");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "N");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "D");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "H");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "AeBe");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "F");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "G");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "K");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "MS");

            migrationBuilder.UpdateData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "S");

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110001,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110002,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110101,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110102,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110103,
                column: "StarClassId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110201,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110301,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110401,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110402,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110403,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110404,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110501,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110502,
                column: "StarClassId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110503,
                column: "StarClassId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110504,
                column: "StarClassId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110505,
                column: "StarClassId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110601,
                column: "StarClassId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110701,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110702,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110703,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110801,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 110802,
                column: "StarClassId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170001,
                column: "StarClassId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170002,
                column: "StarClassId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170003,
                column: "StarClassId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170004,
                column: "StarClassId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170005,
                column: "StarClassId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170101,
                column: "StarClassId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170102,
                column: "StarClassId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170103,
                column: "StarClassId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170104,
                column: "StarClassId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 170105,
                column: "StarClassId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 240001,
                column: "StarClassId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 240002,
                column: "StarClassId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 240003,
                column: "StarClassId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 240004,
                column: "StarClassId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 240005,
                column: "StarClassId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "RuleStar",
                keyColumn: "Id",
                keyValue: 240006,
                column: "StarClassId",
                value: 11);

            migrationBuilder.DeleteData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "StarClass",
                keyColumn: "Id",
                keyValue: 13);

            // ---- end StarClass renumbering ----

            migrationBuilder.CreateIndex(
                name: "IX_Rule_ParentBodyClassId",
                table: "Rule",
                column: "ParentBodyClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rule_ParentBodyClass_ParentBodyClassId",
                table: "Rule",
                column: "ParentBodyClassId",
                principalTable: "ParentBodyClass",
                principalColumn: "Id");

        }
    }
}