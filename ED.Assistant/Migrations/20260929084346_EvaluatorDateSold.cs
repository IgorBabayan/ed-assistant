using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED.Assistant.Migrations
{
    /// <inheritdoc />
    public partial class EvaluatorDateSold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Evaluator_GenusId",
                table: "Evaluator");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSold",
                table: "Evaluator",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluator_GenusId_DateCreation",
                table: "Evaluator",
                columns: new[] { "GenusId", "DateCreation" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Evaluator_GenusId_DateCreation",
                table: "Evaluator");

            migrationBuilder.DropColumn(
                name: "DateSold",
                table: "Evaluator");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluator_GenusId",
                table: "Evaluator",
                column: "GenusId");
        }
    }
}
